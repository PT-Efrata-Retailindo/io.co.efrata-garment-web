using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Manufactures.Application.GarmentLoadings.Queries;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Dtos;
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

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("loadings")]
    public class GarmentLoadingController : ControllerApiBase
    {
        private readonly IMemoryCacheManager _cacheManager;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public GarmentLoadingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentLoadingRepository = Storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = Storage.GetRepository<IGarmentLoadingItemRepository>();
            _garmentSewingDOItemRepository = Storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentLoadingRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);
            var ids = query.Select(x => x.Identity).ToList();
            var loadingItems = _garmentLoadingItemRepository.Query.Where(x => ids.Contains(x.LoadingId)).Select(loadingItem => new
            {
                loadingItem.ProductCode,
                loadingItem.ProductName,
                loadingItem.Quantity,
                loadingItem.RemainingQuantity,
                loadingItem.Color,
                loadingItem.LoadingId
            }).ToList();
            List<GarmentLoadingListDto> garmentCuttingInListDtos = _garmentLoadingRepository.Find(query).Select(loading =>
            {
                //var items = _garmentLoadingItemRepository.Query.Where(o => o.LoadingId == loading.Identity).Select(loadingItem => new
                //{
                //    loadingItem.ProductCode,
                //    loadingItem.ProductName,
                //    loadingItem.Quantity,
                //    loadingItem.RemainingQuantity,
                //    loadingItem.Color
                //}).ToList();

                var items = loadingItems.Where(o => o.LoadingId == loading.Identity);

                return new GarmentLoadingListDto(loading)
                {
                    Products = items.Select(i => i.ProductName).Distinct().ToList(),
                    TotalLoadingQuantity =Math.Round( items.Sum(i => i.Quantity),2),
                    TotalRemainingQuantity= Math.Round(items.Sum(i => i.RemainingQuantity),2),
                    Colors=items.Where(i=>i.Color!=null).Select(i=>i.Color).Distinct().ToList()
                };
            }).ToList();

            await Task.Yield();
            return Ok(garmentCuttingInListDtos, info: new
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

            GarmentLoadingDto garmentLoadingDto = _garmentLoadingRepository.Find(o => o.Identity == guid).Select(loading => new GarmentLoadingDto(loading)
            {
                Items = _garmentLoadingItemRepository.Find(o => o.LoadingId == loading.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(loadingItem => new GarmentLoadingItemDto(loadingItem)
                ).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentLoadingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentLoadingCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentLoadingCommand command)
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

            RemoveGarmentLoadingCommand command = new RemoveGarmentLoadingCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentLoadingRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            // var garmentLoadingDto = _garmentLoadingRepository.Find(query).Select(o => new GarmentLoadingDto(o)).ToArray();
            // var garmentLoadingItemDto = _garmentLoadingItemRepository.Find(_garmentLoadingItemRepository.Query).Select(o => new GarmentLoadingItemDto(o)).ToList();

            // Parallel.ForEach(garmentLoadingDto, itemDto =>
            // {
            //     var garmentLoadingItems = garmentLoadingItemDto.Where(x => x.LoadingId == itemDto.Id).OrderBy(x => x.Id).ToList();

            //     itemDto.Items = garmentLoadingItems;
            // });

            // if (order != "{}")
            // {
            //     Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //     garmentLoadingDto = QueryHelper<GarmentLoadingDto>.Order(garmentLoadingDto.AsQueryable(), OrderDictionary).ToArray();
            // }

            // await Task.Yield();
            // return Ok(garmentLoadingDto, info: new
            // {
            //     page,
            //     size,
            //     count
            // });

            var newQuery = _garmentLoadingRepository.ReadExecute(query);
            await Task.Yield();
            return Ok(newQuery, info: new
            {
                page,
                size,
                count
            });

        }

		[HttpGet("monitoring")]
		public async Task<IActionResult> GetMonitoring(int unit, DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringLoadingQuery query = new GetMonitoringLoadingQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.garmentMonitorings, info: new
			{
				page,
				size,
				viewModel.count
			});
		}
		[HttpGet("download")]
		public async Task<IActionResult> GetXls(int unit, DateTime dateFrom, DateTime dateTo, string type,int page = 1, int size = 25, string Order = "{}")
		{
			try
			{
				VerifyUser();
				GetXlsLoadingQuery query = new GetXlsLoadingQuery(page, size, Order, unit, dateFrom, dateTo,type, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Loading";

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

        [HttpGet("{id}/{buyer}")]
        public async Task<IActionResult> GetPdf(string id, string buyer)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentLoadingDto garmentLoadingDto = _garmentLoadingRepository.Find(o => o.Identity == guid).Select(loading => new GarmentLoadingDto(loading)
            {
                Items = _garmentLoadingItemRepository.Find(o => o.LoadingId == loading.Identity).Select(loadingItem => new GarmentLoadingItemDto(loadingItem)
                ).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentLoadingPDFTemplate.Generate(garmentLoadingDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentLoadingDto.LoadingNo}.pdf"
            };
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentLoadingCommand command)
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
