using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.Queries;
using Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconFabricWashes;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("service-subcon-fabric-washes")]
    public class GarmentServiceSubconFabricWashController : ControllerApiBase
    {
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconFabricWashItemRepository _garmentServiceSubconFabricWashItemRepository;
        private readonly IGarmentServiceSubconFabricWashDetailRepository _garmentServiceSubconFabricWashDetailRepository;

        public GarmentServiceSubconFabricWashController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSubconFabricWashRepository = Storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentServiceSubconFabricWashItemRepository = Storage.GetRepository<IGarmentServiceSubconFabricWashItemRepository>();
            _garmentServiceSubconFabricWashDetailRepository = Storage.GetRepository<IGarmentServiceSubconFabricWashDetailRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconFabricWashRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSubconFabricWashListDto> garmentServiceSubconFabricWashListDtos = _garmentServiceSubconFabricWashRepository
                .Find(query)
                .Select(ServiceSubconFabricWash => new GarmentServiceSubconFabricWashListDto(ServiceSubconFabricWash))
                .ToList();

            var dtoIds = garmentServiceSubconFabricWashListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSubconFabricWashItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSubconFabricWashId))
                .Select(s => new { s.Identity, s.ServiceSubconFabricWashId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            await Task.Yield();
            return Ok(garmentServiceSubconFabricWashListDtos, info: new
            {
                page,
                size,
                total,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentServiceSubconFabricWashDto garmentServiceSubconFabricWashDto = _garmentServiceSubconFabricWashRepository.Find(o => o.Identity == guid).Select(serviceSubconFabricWash => new GarmentServiceSubconFabricWashDto(serviceSubconFabricWash)
            {
                Items = _garmentServiceSubconFabricWashItemRepository.Find(o => o.ServiceSubconFabricWashId == serviceSubconFabricWash.Identity).Select(subconItem => new GarmentServiceSubconFabricWashItemDto(subconItem)
                {
                    Details = _garmentServiceSubconFabricWashDetailRepository.Find(o => o.ServiceSubconFabricWashItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconFabricWashDetailDto(subconDetail)).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSubconFabricWashDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSubconFabricWashCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSubconFabricWashCommand command)
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

            RemoveGarmentServiceSubconFabricWashCommand command = new RemoveGarmentServiceSubconFabricWashCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconFabricWashRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentServiceSubconFabricWashDto = _garmentServiceSubconFabricWashRepository.Find(query).Select(o => new GarmentServiceSubconFabricWashDto(o)).ToArray();
            var garmentServiceSubconFabricWashItemDto = _garmentServiceSubconFabricWashItemRepository.Find(_garmentServiceSubconFabricWashItemRepository.Query).Select(o => new GarmentServiceSubconFabricWashItemDto(o)).ToList();
            var garmentServiceSubconFabricWashDetailDto = _garmentServiceSubconFabricWashDetailRepository.Find(_garmentServiceSubconFabricWashDetailRepository.Query).Select(o => new GarmentServiceSubconFabricWashDetailDto(o)).ToList();

            Parallel.ForEach(garmentServiceSubconFabricWashDto, itemDto =>
            {
                var garmentServiceSubconFabricWashItems = garmentServiceSubconFabricWashItemDto.Where(x => x.ServiceSubconFabricWashId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentServiceSubconFabricWashItems;
                Parallel.ForEach(itemDto.Items, detailDto =>
                {
                    var garmentServiceSubconFabricWashDetails = garmentServiceSubconFabricWashDetailDto.Where(x => x.ServiceSubconFabricWashItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Details = garmentServiceSubconFabricWashDetails;
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSubconFabricWashDto = QueryHelper<GarmentServiceSubconFabricWashDto>.Order(garmentServiceSubconFabricWashDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSubconFabricWashDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("get-pdf/{id}")]
        public async Task<IActionResult> GetPdf(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            //int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentServiceSubconFabricWashDto garmentServiceSubconFabricWashDto = _garmentServiceSubconFabricWashRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentServiceSubconFabricWashDto(subcon)
            {
                Items = _garmentServiceSubconFabricWashItemRepository.Find(o => o.ServiceSubconFabricWashId == subcon.Identity).Select(subconItem => new GarmentServiceSubconFabricWashItemDto(subconItem)
                {
                    Details = _garmentServiceSubconFabricWashDetailRepository.Find(o => o.ServiceSubconFabricWashItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconFabricWashDetailDto(subconDetail)
                    { }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            var stream = GarmentServiceSubconFabricWashPDFTemplate.Generate(garmentServiceSubconFabricWashDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentServiceSubconFabricWashDto.ServiceSubconFabricWashNo}.pdf"
            };
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetXls(DateTime dateFrom, DateTime dateTo, string token, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsServiceSubconFabricWashQuery query = new GetXlsServiceSubconFabricWashQuery(page, size, Order, dateFrom, dateTo, token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan SUBCON BB - FABRIC WASH / PRINT";

                if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

                if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
                filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                //throw e;
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
