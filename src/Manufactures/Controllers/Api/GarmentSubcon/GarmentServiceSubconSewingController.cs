using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconGarmentWashReport;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Dtos.GarmentSubcon;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("service-subcon-sewings")]
    public class GarmentServiceSubconSewingController : ControllerApiBase
    {
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;
        private readonly IGarmentServiceSubconSewingItemRepository _garmentServiceSubconSewingItemRepository;
        private readonly IGarmentServiceSubconSewingDetailRepository _garmentServiceSubconSewingDetailRepository;

        public GarmentServiceSubconSewingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSubconSewingRepository = Storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconSewingDetailRepository = Storage.GetRepository<IGarmentServiceSubconSewingDetailRepository>();
            _garmentServiceSubconSewingItemRepository = Storage.GetRepository<IGarmentServiceSubconSewingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconSewingRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentServiceSubconSewingItem.Sum(b => b.GarmentServiceSubconSewingDetail.Sum(c => c.Quantity)));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSubconSewingListDto> garmentServiceSubconSewingListDtos = _garmentServiceSubconSewingRepository
                .Find(query)
                .Select(ServiceSubconSewing => new GarmentServiceSubconSewingListDto(ServiceSubconSewing))
                .ToList();

            var dtoIds = garmentServiceSubconSewingListDtos.Select(s => s.Id).ToList();
            var items = _garmentServiceSubconSewingItemRepository.Query
                .Where(o => dtoIds.Contains(o.ServiceSubconSewingId))
                .Select(s => new { s.Identity, s.ServiceSubconSewingId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            //Parallel.ForEach(garmentServiceSubconSewingListDtos, dto =>
            //{
            //    var currentItems = items.Where(w => w.ServiceSubconSewingId == dto.Id);
            //    //dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
            //    //dto.Products = currentItems.Select(i => i.ProductCode).Distinct().ToList();
            //    //dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            //});

            await Task.Yield();
            return Ok(garmentServiceSubconSewingListDtos, info: new
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

            GarmentServiceSubconSewingDto garmentServiceSubconSewingDto = _garmentServiceSubconSewingRepository.Find(o => o.Identity == guid).Select(serviceSubconSewing => new GarmentServiceSubconSewingDto(serviceSubconSewing)
            {
                Items = _garmentServiceSubconSewingItemRepository.Find(o => o.ServiceSubconSewingId == serviceSubconSewing.Identity).Select(subconItem => new GarmentServiceSubconSewingItemDto(subconItem)
                {
                    Details = _garmentServiceSubconSewingDetailRepository.Find(o => o.ServiceSubconSewingItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconSewingDetailDto(subconDetail)
                    {

                    }).ToList()
                }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSubconSewingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSubconSewingCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSubconSewingCommand command)
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

            RemoveGarmentServiceSubconSewingCommand command = new RemoveGarmentServiceSubconSewingCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconSewingRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentServiceSubconSewingDto = _garmentServiceSubconSewingRepository.Find(query).Select(o => new GarmentServiceSubconSewingDto(o)).ToArray();
            var garmentServiceSubconSewingItemDto = _garmentServiceSubconSewingItemRepository.Find(_garmentServiceSubconSewingItemRepository.Query).Select(o => new GarmentServiceSubconSewingItemDto(o)).ToList();
            var garmentServiceSubconSewingDetailDto = _garmentServiceSubconSewingDetailRepository.Find(_garmentServiceSubconSewingDetailRepository.Query).Select(o => new GarmentServiceSubconSewingDetailDto(o)).ToList();

            Parallel.ForEach(garmentServiceSubconSewingDto, itemDto =>
            {
                var garmentServiceSubconSewingItems = garmentServiceSubconSewingItemDto.Where(x => x.ServiceSubconSewingId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentServiceSubconSewingItems;
                Parallel.ForEach(itemDto.Items, detailDto =>
                {
                    var garmentCuttingInDetails = garmentServiceSubconSewingDetailDto.Where(x => x.ServiceSubconSewingItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Details = garmentCuttingInDetails;
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSubconSewingDto = QueryHelper<GarmentServiceSubconSewingDto>.Order(garmentServiceSubconSewingDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSubconSewingDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("item")]
        public async Task<IActionResult> GetItems(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconSewingItemRepository.ReadItem(page, size, order, keyword, filter);
            var count = query.Count();

            //var garmentServiceSubconSewingDto = _garmentServiceSubconSewingRepository.Find(query).Select(o => new GarmentServiceSubconSewingDto(o)).ToArray();
            var garmentServiceSubconSewingItemDto = _garmentServiceSubconSewingItemRepository.Find(_garmentServiceSubconSewingItemRepository.Query).Select(o => new GarmentServiceSubconSewingItemDto(o)).ToArray();
            var garmentServiceSubconSewingDetailDto = _garmentServiceSubconSewingDetailRepository.Find(_garmentServiceSubconSewingDetailRepository.Query).Select(o => new GarmentServiceSubconSewingDetailDto(o)).ToList();


            Parallel.ForEach(garmentServiceSubconSewingItemDto, itemDto =>
            {
                var garmentServiceSubconSewingDetails = garmentServiceSubconSewingDetailDto.Where(x => x.ServiceSubconSewingItemId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Details = garmentServiceSubconSewingDetails;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSubconSewingItemDto = QueryHelper<GarmentServiceSubconSewingItemDto>.Order(garmentServiceSubconSewingItemDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSubconSewingItemDto, info: new
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

            GarmentServiceSubconSewingDto garmentServiceSubconSewingDto = _garmentServiceSubconSewingRepository.Find(o => o.Identity == guid).Select(serviceSubconSewing => new GarmentServiceSubconSewingDto(serviceSubconSewing)
            {
                Items = _garmentServiceSubconSewingItemRepository.Find(o => o.ServiceSubconSewingId == serviceSubconSewing.Identity).Select(subconItem => new GarmentServiceSubconSewingItemDto(subconItem)
                {
                    Details = _garmentServiceSubconSewingDetailRepository.Find(o => o.ServiceSubconSewingItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconSewingDetailDto(subconDetail)
                    {

                    }).ToList()
                }).ToList()

            }
            ).FirstOrDefault();
            var stream = GarmentServiceSubconSewingPDFTemplate.Generate(garmentServiceSubconSewingDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentServiceSubconSewingDto.ServiceSubconSewingNo}.pdf"
            };
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetXlsSubconGarmentWashReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            try
            {
                GetXlsGarmentSubconGarmentWashReporQuery query = new GetXlsGarmentSubconGarmentWashReporQuery(page, size, order, dateFrom, dateTo);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = String.Format("Laporan SubCon Jasa Garment Wash.xlsx");

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
