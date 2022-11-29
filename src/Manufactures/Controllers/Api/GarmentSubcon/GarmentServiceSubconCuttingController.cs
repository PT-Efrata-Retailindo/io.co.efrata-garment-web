using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.Queries;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Dtos.GarmentSubcon;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("service-subcon-cuttings")]
    public class GarmentServiceSubconCuttingController : ControllerApiBase
    {
        private readonly IGarmentServiceSubconCuttingRepository _garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository _garmentServiceSubconCuttingItemRepository;
        private readonly IGarmentServiceSubconCuttingDetailRepository _garmentServiceSubconCuttingDetailRepository;
        private readonly IGarmentServiceSubconCuttingSizeRepository _garmentServiceSubconCuttingSizeRepository;

        public GarmentServiceSubconCuttingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentServiceSubconCuttingRepository = Storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = Storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
            _garmentServiceSubconCuttingDetailRepository = Storage.GetRepository<IGarmentServiceSubconCuttingDetailRepository>();
            _garmentServiceSubconCuttingSizeRepository = Storage.GetRepository<IGarmentServiceSubconCuttingSizeRepository>();

        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconCuttingRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentServiceSubconCuttingItem.Sum(b => b.GarmentServiceSubconCuttingDetail.Sum(c=>c.Quantity)));

            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentServiceSubconCuttingListDto> garmentServiceSubconCuttingListDtos = _garmentServiceSubconCuttingRepository
                .Find(query)
                .Select(subcon => new GarmentServiceSubconCuttingListDto(subcon))
                .ToList();

            var dtoIds = garmentServiceSubconCuttingListDtos.Select(s => s.Id).ToList();
            //var items = _garmentServiceSubconCuttingItemRepository.Query
            //    .Where(o => dtoIds.Contains(o.ServiceSubconCuttingId))
            //    .Select(s => new { s.Identity, s.ServiceSubconCuttingId, s.ProductCode, s.Quantity})
            //    .ToList();

            //Parallel.ForEach(garmentServiceSubconCuttingListDtos, dto =>
            //{
            //    var currentItems = items.Where(w => w.ServiceSubconCuttingId == dto.Id);
            //    dto.Products = currentItems.Select(d => d.ProductCode).Distinct().ToList();
            //    dto.TotalQuantity = currentItems.Sum(d => d.Quantity);
            //});

            await Task.Yield();
            return Ok(garmentServiceSubconCuttingListDtos, info: new
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

            GarmentServiceSubconCuttingDto garmentServiceSubconCuttingDto = _garmentServiceSubconCuttingRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentServiceSubconCuttingDto(subcon)
            {
                Items = _garmentServiceSubconCuttingItemRepository.Find(o => o.ServiceSubconCuttingId == subcon.Identity).Select(subconItem => new GarmentServiceSubconCuttingItemDto(subconItem)
                {
                    Details = _garmentServiceSubconCuttingDetailRepository.Find(o => o.ServiceSubconCuttingItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconCuttingDetailDto(subconDetail)
                    {
                        Sizes= _garmentServiceSubconCuttingSizeRepository.Find(o => o.ServiceSubconCuttingDetailId == subconDetail.Identity).Select(subconSize => new GarmentServiceSubconCuttingSizeDto(subconSize)
                        {

                        }).ToList()
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentServiceSubconCuttingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentServiceSubconCuttingCommand command)
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

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconCuttingRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentServiceSubconCuttingDto = _garmentServiceSubconCuttingRepository.Find(query).Select(o => new GarmentServiceSubconCuttingDto(o)).ToArray();
            var garmentServiceSubconCuttingItemDto = _garmentServiceSubconCuttingItemRepository.Find(_garmentServiceSubconCuttingItemRepository.Query).Select(o => new GarmentServiceSubconCuttingItemDto(o)).ToList();
            var garmentServiceSubconCuttingDetailDto = _garmentServiceSubconCuttingDetailRepository.Find(_garmentServiceSubconCuttingDetailRepository.Query).Select(o => new GarmentServiceSubconCuttingDetailDto(o)).ToList();
            var garmentServiceSubconCuttingSizeDto = _garmentServiceSubconCuttingSizeRepository.Find(_garmentServiceSubconCuttingSizeRepository.Query).Select(o => new GarmentServiceSubconCuttingSizeDto(o)).ToList();

            Parallel.ForEach(garmentServiceSubconCuttingDto, itemDto =>
            {
                var garmentServiceSubconCuttingItems = garmentServiceSubconCuttingItemDto.Where(x => x.ServiceSubconCuttingId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentServiceSubconCuttingItems;
                Parallel.ForEach(itemDto.Items, detailDto =>
                {
                    var garmentCuttingInDetails = garmentServiceSubconCuttingDetailDto.Where(x => x.ServiceSubconCuttingItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Details = garmentCuttingInDetails;
                    Parallel.ForEach(detailDto.Details, detDto =>
                    {
                        var garmentCuttingSizes = garmentServiceSubconCuttingSizeDto.Where(x => x.ServiceSubconCuttingDetailId == detDto.Id).OrderBy(x => x.Id).ToList();
                        detDto.Sizes = garmentCuttingSizes;
                    });
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSubconCuttingDto = QueryHelper<GarmentServiceSubconCuttingDto>.Order(garmentServiceSubconCuttingDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSubconCuttingDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentServiceSubconCuttingCommand command)
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

            RemoveGarmentServiceSubconCuttingCommand command = new RemoveGarmentServiceSubconCuttingCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
            
        }

        [HttpGet("item")]
        public async Task<IActionResult> GetItems(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentServiceSubconCuttingItemRepository.ReadItem(page, size, order, keyword, filter);
            var count = query.Count();

            //var garmentServiceSubconCuttingDto = _garmentServiceSubconCuttingRepository.Find(query).Select(o => new GarmentServiceSubconCuttingDto(o)).ToArray();
            var garmentServiceSubconCuttingItemDto = _garmentServiceSubconCuttingItemRepository.Find(_garmentServiceSubconCuttingItemRepository.Query).Select(o => new GarmentServiceSubconCuttingItemDto(o)).ToArray();
            var garmentServiceSubconCuttingDetailDto = _garmentServiceSubconCuttingDetailRepository.Find(_garmentServiceSubconCuttingDetailRepository.Query).Select(o => new GarmentServiceSubconCuttingDetailDto(o)).ToList();
            var garmentServiceSubconCuttingSizeDto = _garmentServiceSubconCuttingSizeRepository.Find(_garmentServiceSubconCuttingSizeRepository.Query).Select(o => new GarmentServiceSubconCuttingSizeDto(o)).ToList();

            Parallel.ForEach(garmentServiceSubconCuttingItemDto, itemDto =>
            {
                var garmentServiceSubconCuttingDetails = garmentServiceSubconCuttingDetailDto.Where(x => x.ServiceSubconCuttingItemId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Details = garmentServiceSubconCuttingDetails;
                Parallel.ForEach(itemDto.Details, detailDto =>
                {
                    var garmentCuttingSizes = garmentServiceSubconCuttingSizeDto.Where(x => x.ServiceSubconCuttingDetailId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Sizes = garmentCuttingSizes;
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentServiceSubconCuttingItemDto = QueryHelper<GarmentServiceSubconCuttingItemDto>.Order(garmentServiceSubconCuttingItemDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentServiceSubconCuttingItemDto, info: new
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
            GarmentServiceSubconCuttingDto garmentServiceSubconCuttingDto = _garmentServiceSubconCuttingRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentServiceSubconCuttingDto(subcon)
            {
                Items = _garmentServiceSubconCuttingItemRepository.Find(o => o.ServiceSubconCuttingId == subcon.Identity).Select(subconItem => new GarmentServiceSubconCuttingItemDto(subconItem)
                {
                    Details = _garmentServiceSubconCuttingDetailRepository.Find(o => o.ServiceSubconCuttingItemId == subconItem.Identity).Select(subconDetail => new GarmentServiceSubconCuttingDetailDto(subconDetail)
                    {
                        Sizes = _garmentServiceSubconCuttingSizeRepository.Find(o => o.ServiceSubconCuttingDetailId == subconDetail.Identity).Select(subconSize => new GarmentServiceSubconCuttingSizeDto(subconSize)
                        {

                        }).ToList()
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            var stream = GarmentServiceSubconCuttingPDFTemplate.Generate(garmentServiceSubconCuttingDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentServiceSubconCuttingDto.SubconNo}.pdf"
            };
        }


        [HttpGet("download")]
        public async Task<IActionResult> GetXlsMutation(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsServiceSubconCuttingQuery query = new GetXlsServiceSubconCuttingQuery(page, size, Order, dateFrom, dateTo, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Monitoring SUbcon Jasa Komponen ";

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

    }
}
