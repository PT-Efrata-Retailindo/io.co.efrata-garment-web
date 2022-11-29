using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentFinishingOuts.Queries;
using Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable;
using Manufactures.Application.GarmentReport.ForTraceableIn.Queries;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
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
    [Route("finishing-outs")]
    public class GarmentFinishingOutController : ControllerApiBase
    {
        private readonly IGarmentFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;

        public GarmentFinishingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentFinishingOutRepository = Storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = Storage.GetRepository<IGarmentFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = Storage.GetRepository<IGarmentFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = Storage.GetRepository<IGarmentFinishingInItemRepository>();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishingOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentFinishingOutItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentFinishingOutListDto> garmentFinishingOutListDtos = _garmentFinishingOutRepository
                .Find(query)
                .Select(SewOut => new GarmentFinishingOutListDto(SewOut))
                .ToList();

            var dtoIds = garmentFinishingOutListDtos.Select(s => s.Id).ToList();
            var items = _garmentFinishingOutItemRepository.Query
                .Where(o => dtoIds.Contains(o.FinishingOutId))
                .Select(s => new { s.Identity, s.FinishingOutId, s.ProductCode, s.Color, s.Quantity, s.RemainingQuantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            var details = _garmentFinishingOutDetailRepository.Query
                .Where(o => itemIds.Contains(o.FinishingOutItemId))
                .Select(s => new { s.Identity, s.FinishingOutItemId })
                .ToList();

            Parallel.ForEach(garmentFinishingOutListDtos, dto =>
            {
                var currentItems = items.Where(w => w.FinishingOutId == dto.Id);
                dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
                dto.Products = currentItems.Select(i => i.ProductCode).Distinct().ToList();
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
                dto.TotalRemainingQuantity = currentItems.Sum(i => i.RemainingQuantity);
            });

            await Task.Yield();
            return Ok(garmentFinishingOutListDtos, info: new
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

            GarmentFinishingOutDto garmentFinishingOutDto = _garmentFinishingOutRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentFinishingOutDto(finishOut)
            {
                Items = _garmentFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(finishOutItem => new GarmentFinishingOutItemDto(finishOutItem)
                {
                    Details = _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).OrderBy(i => i.SizeName).Select(finishOutDetail => new GarmentFinishingOutDetailDto(finishOutDetail)
                    {
                    }).ToList()

                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentFinishingOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentFinishingOutCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentFinishingOutCommand command)
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

            RemoveGarmentFinishingOutCommand command = new RemoveGarmentFinishingOutCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentFinishingOutDto = _garmentFinishingOutRepository.ReadExecute(query);
            //var garmentFinishingOutDto = _garmentFinishingOutRepository.Find(query).Select(o => new GarmentFinishingOutDto(o)).ToArray();
            //var garmentFinishingOutItemDto = _garmentFinishingOutItemRepository.Find(_garmentFinishingOutItemRepository.Query).Select(o => new GarmentFinishingOutItemDto(o)).ToList();
            //var garmentFinishingOutDetailDto = _garmentFinishingOutDetailRepository.Find(_garmentFinishingOutDetailRepository.Query).Select(o => new GarmentFinishingOutDetailDto(o)).ToList();

            //Parallel.ForEach(garmentFinishingOutDto, itemDto =>
            //{
            //    var garmentFinishingOutItems = garmentFinishingOutItemDto.Where(x => x.FinishingOutId == itemDto.Id).OrderBy(x => x.Id).ToList();

            //    itemDto.Items = garmentFinishingOutItems;

            //    Parallel.ForEach(itemDto.Items, detailDto =>
            //    {
            //        var garmentFinishingOutDetails = garmentFinishingOutDetailDto.Where(x => x.FinishingOutItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
            //        detailDto.Details = garmentFinishingOutDetails;
            //    });
            //});

            //if (order != "{}")
            //{
            //    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //    garmentFinishingOutDto = QueryHelper<GarmentFinishingOutDto>.Order(garmentFinishingOutDto.AsQueryable(), OrderDictionary).ToArray();
            //}

            await Task.Yield();
            return Ok(garmentFinishingOutDto, info: new
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
			GetMonitoringFinishingQuery query = new GetMonitoringFinishingQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.garmentMonitorings, info: new
			{
				page,
				size,
				viewModel.count
			});
		}
		[HttpGet("download")]
		public async Task<IActionResult> GetXls(int unit, DateTime dateFrom, DateTime dateTo,string type, int page = 1, int size = 25, string Order = "{}")
		{
			try
			{
				VerifyUser();
				GetXlsFinishingQuery query = new GetXlsFinishingQuery(page, size, Order, unit, dateFrom, dateTo,type, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Finishing";

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

		[HttpGet("color")]
		public async Task<IActionResult> GetColor(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentFinishingOutRepository.ReadColor(page, size, order, keyword, filter);
			var total = query.Count();
			query = query.Skip((page - 1) * size).Take(size);

			List<GarmentFinishingOutListDto> garmentFinishingOutListDtos = _garmentFinishingOutRepository
				.Find(query)
				.Select(SewOut => new GarmentFinishingOutListDto(SewOut))
				.ToList();

			var dtoIds = garmentFinishingOutListDtos.Select(s => s.Id).ToList();
			var items = _garmentFinishingOutItemRepository.Query
				.Where(o => dtoIds.Contains(o.FinishingOutId))
				.Select(s => new { s.Identity, s.FinishingOutId, s.ProductCode, s.Color, s.Quantity, s.RemainingQuantity })
				.ToList();

			var itemIds = items.Select(s => s.Identity).ToList();
			 
			List<object> color = new List<object>();
			foreach (var item in items)
			{
				color.Add(new { item.Color });
			}
			await Task.Yield();
			return Ok(color.Distinct(), info: new
			{
				page,
				size,
				color.Count
			});
		}

        [HttpGet("{id}/{buyer}")]
        public async Task<IActionResult> GetPdf(string id, string buyer)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentFinishingOutDto garmentFinishingOutDto = _garmentFinishingOutRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentFinishingOutDto(finishOut)
            {
                Items = _garmentFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).Select(finishOutItem => new GarmentFinishingOutItemDto(finishOutItem)
                {
                    Details = _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).Select(finishOutDetail => new GarmentFinishingOutDetailDto(finishOutDetail)
                    {
                    }).ToList()

                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentFinishingOutPDFTemplate.Generate(garmentFinishingOutDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentFinishingOutDto.FinishingOutNo}.pdf"
            };
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentFinishingOutCommand command)
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

        [HttpGet("for-traceable")]
        public async Task<IActionResult> ForTraceable([FromBody] string RO)
        {
            VerifyUser();

            GetTotalQuantityTraceableQuery query = new GetTotalQuantityTraceableQuery(RO, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentTotalQtyTraceables);
        }

        [HttpGet("for-traceable-full-garment")]
        public async Task<IActionResult> ForTraceableFullGarment([FromBody] string UENItemID)
        {
            VerifyUser();

            ForTraceableInQuery query = new ForTraceableInQuery(UENItemID, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.data);
        }

        [HttpGet("for-traceable-full-garment-sample")]
        public async Task<IActionResult> ForTraceableFullGarmentSample([FromBody] string UENItemID)
        {
            VerifyUser();

            ForTraceableInSampleQuery query = new ForTraceableInSampleQuery(UENItemID, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.data);
        }

    }
}
