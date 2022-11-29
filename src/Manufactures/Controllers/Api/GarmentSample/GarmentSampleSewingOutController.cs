using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsByRONo;
using Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsDynamic;
using Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.MonitoringSewing;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Dtos.GarmentSample.SampleSewingOuts;
using Manufactures.Helpers.PDFTemplates.GarmentSample;
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
    [Route("garment-sample-sewing-outs")]
    public class GarmentSampleSewingOutController : ControllerApiBase
    {
        private readonly IGarmentSampleSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSampleSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSampleSewingInItemRepository _garmentSewingInItemRepository;

        public GarmentSampleSewingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSewingOutRepository = Storage.GetRepository<IGarmentSampleSewingOutRepository>();
            _garmentSewingOutItemRepository = Storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = Storage.GetRepository<IGarmentSampleSewingOutDetailRepository>();
            _garmentSewingInItemRepository = Storage.GetRepository<IGarmentSampleSewingInItemRepository>();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSewingOutItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSampleSewingOutListDto> garmentSewingOutListDtos = _garmentSewingOutRepository
                .Find(query)
                .Select(SewOut => new GarmentSampleSewingOutListDto(SewOut))
                .ToList();

            var dtoIds = garmentSewingOutListDtos.Select(s => s.Id).ToList();
            var items = _garmentSewingOutItemRepository.Query
                .Where(o => dtoIds.Contains(o.SampleSewingOutId))
                .Select(s => new { s.Identity, s.SampleSewingOutId, s.ProductCode, s.Color, s.Quantity, s.RemainingQuantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            var details = _garmentSewingOutDetailRepository.Query
                .Where(o => itemIds.Contains(o.SampleSewingOutItemId))
                .Select(s => new { s.Identity, s.SampleSewingOutItemId })
                .ToList();

            Parallel.ForEach(garmentSewingOutListDtos, dto =>
            {
                var currentItems = items.Where(w => w.SampleSewingOutId == dto.Id);
                dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
                dto.Products = currentItems.Select(i => i.ProductCode).Distinct().ToList();
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
                dto.TotalRemainingQuantity = currentItems.Sum(i => i.RemainingQuantity);
            });

            await Task.Yield();
            return Ok(garmentSewingOutListDtos, info: new
            {
                page,
                size,
                total,
                totalQty
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSampleSewingOutDto garmentSewingOutDto = _garmentSewingOutRepository.Find(o => o.Identity == guid).Select(sewOut => new GarmentSampleSewingOutDto(sewOut)
            {
                Items = _garmentSewingOutItemRepository.Find(o => o.SampleSewingOutId == sewOut.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(sewOutItem => new GarmentSampleSewingOutItemDto(sewOutItem)
                {
                    Details = _garmentSewingOutDetailRepository.Find(o => o.SampleSewingOutItemId == sewOutItem.Identity).OrderBy(i => i.SizeName).Select(sewOutDetail => new GarmentSampleSewingOutDetailDto(sewOutDetail)
                    {
                    }).ToList()

                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSewingOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleSewingOutCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleSewingOutCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentSampleSewingOutCommand command = new RemoveGarmentSampleSewingOutCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 10, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingOutRepository.ReadComplete(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSewingOutDto = _garmentSewingOutRepository.Find(query).Select(o => new GarmentSampleSewingOutDto(o)).ToArray();

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSewingOutDto = QueryHelper<GarmentSampleSewingOutDto>.Order(garmentSewingOutDto.AsQueryable(), OrderDictionary).ToArray();
            }


            await Task.Yield();
            return Ok(garmentSewingOutDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 10, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSewingOutDto = _garmentSewingOutRepository.ReadExecute(query);

            //var garmentSewingOutDto = _garmentSewingOutRepository.Find(query).Select(o => new GarmentSewingOutDto(o)).ToArray();
            //var garmentSewingOutItemDto = _garmentSewingOutItemRepository.Find(_garmentSewingOutItemRepository.Query).Select(o => new GarmentSewingOutItemDto(o)).ToList();
            //var garmentSewingOutDetailDto = _garmentSewingOutDetailRepository.Find(_garmentSewingOutDetailRepository.Query).Select(o => new GarmentSewingOutDetailDto(o)).ToList();

            //Parallel.ForEach(garmentSewingOutDto, itemDto =>
            //{
            //    var garmentSewingOutItems = garmentSewingOutItemDto.Where(x => x.SewingOutId == itemDto.Id).OrderBy(x => x.Id).ToList();

            //    itemDto.Items = garmentSewingOutItems;

            //    Parallel.ForEach(itemDto.Items, detailDto =>
            //    {
            //        var garmentSewingOutDetails = garmentSewingOutDetailDto.Where(x => x.SewingOutItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
            //        detailDto.Details = garmentSewingOutDetails;
            //    });
            //});

            //if (order != "{}")
            //{
            //    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //    garmentSewingOutDto = QueryHelper<GarmentSewingOutDto>.Order(garmentSewingOutDto.AsQueryable(), OrderDictionary).ToArray();
            //}

            await Task.Yield();
            return Ok(garmentSewingOutDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("loader-by-ro")]
        public async Task<IActionResult> GetLoaderByRO(string keyword, string filter = "{}")
        {
            VerifyUser();

            var result = await Mediator.Send(new GetGarmentSampleSewingOutsByRONoQuery(keyword, filter));

            return Ok(result.data);
        }

        [HttpGet("dynamic")]
        public async Task<IActionResult> GetDynamic(int page = 1, int size = 25, string order = "{}", string search = "[]", string select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var result = await Mediator.Send(new GetGarmentSampleSewingOutsDynamicQuery(page, size, order, search, select, keyword, filter));

            return Ok(result.data, info: new
            {
                page,
                size,
                count = result.data.Count,
                result.total
            });
        }

        [HttpGet("{id}/{buyer}")]
        public async Task<IActionResult> GetPdf(string id, string buyer)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentSampleSewingOutDto garmentSewingOutDto = _garmentSewingOutRepository.Find(o => o.Identity == guid).Select(sewOut => new GarmentSampleSewingOutDto(sewOut)
            {
                Items = _garmentSewingOutItemRepository.Find(o => o.SampleSewingOutId == sewOut.Identity).Select(sewOutItem => new GarmentSampleSewingOutItemDto(sewOutItem)
                {
                    Details = _garmentSewingOutDetailRepository.Find(o => o.SampleSewingOutItemId == sewOutItem.Identity).Select(sewOutDetail => new GarmentSampleSewingOutDetailDto(sewOutDetail)
                    {
                    }).ToList()

                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentSampleSewingOutPDFTemplate.Generate(garmentSewingOutDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentSewingOutDto.SewingOutNo}.pdf"
            };
        }

        [HttpGet("monitoring")]
        public async Task<IActionResult> GetMonitoring(int unit, DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            VerifyUser();
            GetMonitoringSampleSewingQuery query = new GetMonitoringSampleSewingQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
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
                GetXlsSampleSewingQuery query = new GetXlsSampleSewingQuery(page, size, Order, unit, dateFrom, dateTo, type, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Sewing Sample";

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
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentSampleSewingOutCommand command)
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
