using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.ExcelTemplates;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconShrinkagePanels;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("service-subcon-shrinkage-panels")]
    public class GarmentServiceSubconShrinkagePanelController : ControllerApiBase
    {
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconShrinkagePanelItemRepository _garmentServiceSubconShrinkagePanelItemRepository;
        private readonly IGarmentServiceSubconShrinkagePanelDetailRepository _garmentServiceSubconShrinkagePanelDetailRepository;

        public GarmentServiceSubconShrinkagePanelController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSubconShrinkagePanelRepository = Storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconShrinkagePanelItemRepository = Storage.GetRepository<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _garmentServiceSubconShrinkagePanelDetailRepository = Storage.GetRepository<IGarmentServiceSubconShrinkagePanelDetailRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconShrinkagePanelRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSubconShrinkagePanelListDto> garmentServiceSubconShrinkagePanelListDtos = _garmentServiceSubconShrinkagePanelRepository
                .Find(query)
                .Select(ServiceSubconShrinkagePanel => new GarmentServiceSubconShrinkagePanelListDto(ServiceSubconShrinkagePanel))
                .ToList();

            var dtoIds = garmentServiceSubconShrinkagePanelListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSubconShrinkagePanelItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSubconShrinkagePanelId))
                .Select(s => new { s.Identity, s.ServiceSubconShrinkagePanelId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            await Task.Yield();
            return Ok(garmentServiceSubconShrinkagePanelListDtos, info: new
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

            GarmentServiceSubconShrinkagePanelDto garmentServiceSubconShrinkagePanelDto = _garmentServiceSubconShrinkagePanelRepository.Find(o => o.Identity == guid).Select(serviceSubconShrinkagePanel => new GarmentServiceSubconShrinkagePanelDto(serviceSubconShrinkagePanel)
            {
                Items = _garmentServiceSubconShrinkagePanelItemRepository.Find(o => o.ServiceSubconShrinkagePanelId == serviceSubconShrinkagePanel.Identity).Select(subconItem => new GarmentServiceSubconShrinkagePanelItemDto(subconItem)
                {
                    Details = _garmentServiceSubconShrinkagePanelDetailRepository.Find(o => o.ServiceSubconShrinkagePanelItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconShrinkagePanelDetailDto(subconDetail)).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSubconShrinkagePanelDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSubconShrinkagePanelCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSubconShrinkagePanelCommand command)
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

            RemoveGarmentServiceSubconShrinkagePanelCommand command = new RemoveGarmentServiceSubconShrinkagePanelCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconShrinkagePanelRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentServiceSubconShrinkagePanelDto = _garmentServiceSubconShrinkagePanelRepository.Find(query).Select(o => new GarmentServiceSubconShrinkagePanelDto(o)).ToArray();
            var garmentServiceSubconShrinkagePanelItemDto = _garmentServiceSubconShrinkagePanelItemRepository.Find(_garmentServiceSubconShrinkagePanelItemRepository.Query).Select(o => new GarmentServiceSubconShrinkagePanelItemDto(o)).ToList();
            var garmentServiceSubconShrinkagePanelDetailDto = _garmentServiceSubconShrinkagePanelDetailRepository.Find(_garmentServiceSubconShrinkagePanelDetailRepository.Query).Select(o => new GarmentServiceSubconShrinkagePanelDetailDto(o)).ToList();

            Parallel.ForEach(garmentServiceSubconShrinkagePanelDto, itemDto =>
            {
                var garmentServiceSubconShrinkagePanelItems = garmentServiceSubconShrinkagePanelItemDto.Where(x => x.ServiceSubconShrinkagePanelId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentServiceSubconShrinkagePanelItems;
                Parallel.ForEach(itemDto.Items, detailDto =>
                {
                    var garmentServiceSubconShrinkagePanelDetails = garmentServiceSubconShrinkagePanelDetailDto.Where(x => x.ServiceSubconShrinkagePanelItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Details = garmentServiceSubconShrinkagePanelDetails;
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSubconShrinkagePanelDto = QueryHelper<GarmentServiceSubconShrinkagePanelDto>.Order(garmentServiceSubconShrinkagePanelDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSubconShrinkagePanelDto, info: new
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
            GarmentServiceSubconShrinkagePanelDto garmentServiceSubconShrinkagePanelDto = _garmentServiceSubconShrinkagePanelRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentServiceSubconShrinkagePanelDto(subcon)
            {
                Items = _garmentServiceSubconShrinkagePanelItemRepository.Find(o => o.ServiceSubconShrinkagePanelId == subcon.Identity).Select(subconItem => new GarmentServiceSubconShrinkagePanelItemDto(subconItem)
                {
                    Details = _garmentServiceSubconShrinkagePanelDetailRepository.Find(o => o.ServiceSubconShrinkagePanelItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconShrinkagePanelDetailDto(subconDetail)
                    {
                        Composition = GetProduct(subconDetail.ProductId.Value, WorkContext.Token).data.Composition
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            var stream = GarmentServiceSubconShrinkagePanelPDFTemplate.Generate(garmentServiceSubconShrinkagePanelDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentServiceSubconShrinkagePanelDto.ServiceSubconShrinkagePanelNo}.pdf"
            };
        }

        [HttpGet("getXls")]
        public async Task<IActionResult> GetXls(DateTime dateFrom, DateTime dateTo, string token)
        {
            try
            {
                VerifyUser();
                GetXlsSubconServiceSubconShrinkagePanelsQuery query = new GetXlsSubconServiceSubconShrinkagePanelsQuery(dateFrom, dateTo, token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Subcon Shrinkage BB";

                if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

                if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");

                filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch(Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
