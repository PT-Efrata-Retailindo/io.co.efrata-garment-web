using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSample.SamplePreparings.Queries.GetMonitoringPrepareSample;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Dtos.GarmentSample.SamplePreparings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-preparings")]
    public class GarmentSamplePreparingController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;

        public GarmentSamplePreparingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSamplePreparingRepository = Storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = Storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSamplePreparingRepository.ReadOptimized(order, filter, keyword);
            var newQuery = _garmentSamplePreparingRepository.ReadExecute(query, keyword).ToList();
            int totalRows = query.Count();
            var data = newQuery.Skip((page - 1) * size).Take(size);
            await Task.Yield();
            return Ok(data, info: new
            {
                page,
                size,
                total = totalRows
            });
        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var query = _garmentSamplePreparingRepository.Read(order, select, filter);
            int totalRows = query.Count();
            var garmentSamplePreparingDto = _garmentSamplePreparingRepository.Find(query).Select(o => new GarmentSamplePreparingDto(o)).ToArray();

            if (!string.IsNullOrEmpty(keyword))
            {
                var garmentSamplePreparingDtoList = garmentSamplePreparingDto.Where(x => x.UENNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Unit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || (x.Article != null && x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    ).ToList();

                var garmentSamplePreparingDtoListArray = garmentSamplePreparingDtoList.ToArray();
                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentSamplePreparingDtoListArray = QueryHelper<GarmentSamplePreparingDto>.Order(garmentSamplePreparingDtoList.AsQueryable(), OrderDictionary).ToArray();
                }
                else
                {
                    garmentSamplePreparingDtoListArray = garmentSamplePreparingDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
                }

                garmentSamplePreparingDtoListArray = garmentSamplePreparingDtoListArray.Skip((page - 1) * size).Take(size).ToArray();

                await Task.Yield();
                return Ok(garmentSamplePreparingDtoListArray, info: new
                {
                    page,
                    size,
                    total = totalRows
                });
            }
            else
            {
                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentSamplePreparingDto = QueryHelper<GarmentSamplePreparingDto>.Order(garmentSamplePreparingDto.AsQueryable(), OrderDictionary).ToArray();
                }
                else
                {
                    garmentSamplePreparingDto = garmentSamplePreparingDto.OrderByDescending(x => x.LastModifiedDate).ToArray();
                }

                garmentSamplePreparingDto = garmentSamplePreparingDto.Skip((page - 1) * size).Take(size).ToArray();

                await Task.Yield();
                return Ok(garmentSamplePreparingDto, info: new
                {
                    page,
                    size,
                    total = totalRows
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var preparingId = Guid.Parse(id);
            VerifyUser();
            var preparingDto = _garmentSamplePreparingRepository.Find(o => o.Identity == preparingId).Select(o => new GarmentSamplePreparingDto(o)).FirstOrDefault();

            if (preparingDto == null)
                return NotFound();

            var itemConfigs = _garmentSamplePreparingItemRepository.Find(x => x.GarmentSamplePreparingId == preparingDto.Id).Select(o => new GarmentSamplePreparingItemDto(o)).ToList();
            preparingDto.Items = itemConfigs;

            preparingDto.Items = preparingDto.Items.OrderBy(x => x.Id).ToList();
            await Task.Yield();

            return Ok(preparingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSamplePreparingCommand command)
        {
            try
            {
                VerifyUser();

                var garmentSamplePreparingValidation = _garmentSamplePreparingRepository.Find(o => o.UENId == command.UENId && o.UENNo == command.UENNo && o.UnitId == command.Unit.Id
                                && o.ProcessDate == command.ProcessDate && o.RONo == command.RONo && o.Article == command.Article && o.IsCuttingIn == command.IsCuttingIn).Select(o => new GarmentSamplePreparingDto(o)).FirstOrDefault();
                if (garmentSamplePreparingValidation != null)
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        error = "Data sudah ada"
                    });

                var order = await Mediator.Send(command);

                await PutGarmentUnitExpenditureNoteCreate(command.UENId);

                return Ok(order.Identity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            VerifyUser();
            var preparingId = Guid.Parse(id);

            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            var garmentPreparing = _garmentSamplePreparingRepository.Find(x => x.Identity == preparingId).Select(o => new GarmentSamplePreparingDto(o)).FirstOrDefault();

            var command = new RemoveGarmentSamplePreparingCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);

            await PutGarmentUnitExpenditureNoteDelete(garmentPreparing.UENId);

            return Ok(order.Identity);
        }

        [HttpGet("loader/ro")]
        public async Task<IActionResult> GetLoaderByRO(string keyword, string filter = "{}")
        {
            var query = _garmentSamplePreparingRepository.Read(null, null, filter);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(o => o.RONo.Contains(keyword) && o.GarmentSamplePreparingItem.Any(a => a.RemainingQuantity > 0));
            }

            var rOs = query.Select(o => new { o.RONo, o.Article }).Distinct().ToList();

            await Task.Yield();

            return Ok(rOs);
        }
        [HttpGet("monitoring")]
        public async Task<IActionResult> GetMonitoring(int unit, DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            VerifyUser();
            GetMonitoringSamplePrepareQuery query = new GetMonitoringSamplePrepareQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentMonitorings, info: new
            {
                page,
                size,
                viewModel.count
            });
        }
        [HttpGet("download")]
        public async Task<IActionResult> GetXls(int unit, DateTime dateFrom, DateTime dateTo, string type, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsSamplePrepareQuery query = new GetXlsSamplePrepareQuery(type,page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Sample Prepare";

                if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

                if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
                filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentSamplePreparingCommand command)
        {
            VerifyUser();

            if (command.Date == null || command.Date == DateTimeOffset.MinValue)
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Tanggal harus diisi"
                });
            else if (command.Date.Date > DateTimeOffset.Now.Date)
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Tanggal tidak boleh lebih dari hari ini"
                });

            var order = await Mediator.Send(command);

            return Ok();
        }
    }
}
