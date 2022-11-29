using Barebone.Controllers;
using Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable;
using Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries;
using Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries.GarmentSampleFinishingByColorMonitoring;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Dtos.GarmentSample.SampleFinishingOuts;
using Manufactures.Helpers.PDFTemplates.GarmentSample;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantitySampleTraceable;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-finishing-outs")]
    public class GarmentSampleFinishingOutController : ControllerApiBase
    {
        private readonly IGarmentSampleFinishingOutRepository _GarmentSampleFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository _GarmentSampleFinishingOutItemRepository;
        private readonly IGarmentSampleFinishingOutDetailRepository _GarmentSampleFinishingOutDetailRepository;
        private readonly IGarmentSampleFinishingInItemRepository _GarmentSampleFinishingInItemRepository;

        public GarmentSampleFinishingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _GarmentSampleFinishingOutRepository = Storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            _GarmentSampleFinishingOutItemRepository = Storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            _GarmentSampleFinishingOutDetailRepository = Storage.GetRepository<IGarmentSampleFinishingOutDetailRepository>();
            _GarmentSampleFinishingInItemRepository = Storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _GarmentSampleFinishingOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSampleFinishingOutItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSampleFinishingOutListDto> GarmentSampleFinishingOutListDtos = _GarmentSampleFinishingOutRepository
                .Find(query)
                .Select(SewOut => new GarmentSampleFinishingOutListDto(SewOut))
                .ToList();

            var dtoIds = GarmentSampleFinishingOutListDtos.Select(s => s.Id).ToList();
            var items = _GarmentSampleFinishingOutItemRepository.Query
                .Where(o => dtoIds.Contains(o.FinishingOutId))
                .Select(s => new { s.Identity, s.FinishingOutId, s.ProductCode, s.Color, s.Quantity, s.RemainingQuantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            var details = _GarmentSampleFinishingOutDetailRepository.Query
                .Where(o => itemIds.Contains(o.FinishingOutItemId))
                .Select(s => new { s.Identity, s.FinishingOutItemId })
                .ToList();

            Parallel.ForEach(GarmentSampleFinishingOutListDtos, dto =>
            {
                var currentItems = items.Where(w => w.FinishingOutId == dto.Id);
                dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
                dto.Products = currentItems.Select(i => i.ProductCode).Distinct().ToList();
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
                dto.TotalRemainingQuantity = currentItems.Sum(i => i.RemainingQuantity);
            });

            await Task.Yield();
            return Ok(GarmentSampleFinishingOutListDtos, info: new
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

            GarmentSampleFinishingOutDto GarmentSampleFinishingOutDto = _GarmentSampleFinishingOutRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentSampleFinishingOutDto(finishOut)
            {
                Items = _GarmentSampleFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(finishOutItem => new GarmentSampleFinishingOutItemDto(finishOutItem)
                {
                    Details = _GarmentSampleFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).OrderBy(i => i.SizeName).Select(finishOutDetail => new GarmentSampleFinishingOutDetailDto(finishOutDetail)
                    {
                    }).ToList()

                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(GarmentSampleFinishingOutDto);
        }

        [HttpGet("traceable-by-ro")]
        public async Task<IActionResult> GetTraceablebyRONo([FromBody] string RONo)
        {
            VerifyUser();

            GetTotalQuantitySampleTraceableQuery query = new GetTotalQuantitySampleTraceableQuery(RONo, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentTotalQtyTraceables);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleFinishingOutCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleFinishingOutCommand command)
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

            RemoveGarmentSampleFinishingOutCommand command = new RemoveGarmentSampleFinishingOutCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _GarmentSampleFinishingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var GarmentSampleFinishingOutDto = _GarmentSampleFinishingOutRepository.ReadExecute(query);
            

            await Task.Yield();
            return Ok(GarmentSampleFinishingOutDto, info: new
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
            GetSampleFinishingMonitoringQuery query = new GetSampleFinishingMonitoringQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
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
                GetXlsSampleFinishingQuery query = new GetXlsSampleFinishingQuery(page, size, Order, unit, dateFrom, dateTo, type, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Finishing Sample";

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

		//[HttpGet("color")]
		//public async Task<IActionResult> GetColor(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		//{
		//    VerifyUser();

		//    var query = _GarmentSampleFinishingOutRepository.ReadColor(page, size, order, keyword, filter);
		//    var total = query.Count();
		//    query = query.Skip((page - 1) * size).Take(size);

		//    List<GarmentSampleFinishingOutListDto> GarmentSampleFinishingOutListDtos = _GarmentSampleFinishingOutRepository
		//        .Find(query)
		//        .Select(SewOut => new GarmentSampleFinishingOutListDto(SewOut))
		//        .ToList();

		//    var dtoIds = GarmentSampleFinishingOutListDtos.Select(s => s.Id).ToList();
		//    var items = _GarmentSampleFinishingOutItemRepository.Query
		//        .Where(o => dtoIds.Contains(o.FinishingOutId))
		//        .Select(s => new { s.Identity, s.FinishingOutId, s.ProductCode, s.Color, s.Quantity, s.RemainingQuantity })
		//        .ToList();

		//    var itemIds = items.Select(s => s.Identity).ToList();

		//    List<object> color = new List<object>();
		//    foreach (var item in items)
		//    {
		//        color.Add(new { item.Color });
		//    }
		//    await Task.Yield();
		//    return Ok(color.Distinct(), info: new
		//    {
		//        page,
		//        size,
		//        color.Count
		//    });
		//}
		[HttpGet("monitoringByColor")]
		public async Task<IActionResult> GetMonitoringByColor(int unit, DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetSampleFinishingByColorMonitoringQuery query = new GetSampleFinishingByColorMonitoringQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.garmentMonitorings, info: new
			{
				page,
				size,
				viewModel.count
			});
		}

		[HttpGet("downloadByColor")]
		public async Task<IActionResult> GetXlsByColor(int unit, DateTime dateFrom, DateTime dateTo, string type, int page = 1, int size = 25, string Order = "{}")
		{
			try
			{
				VerifyUser();
				GetXlsSampleFinishingByColorQuery query = new GetXlsSampleFinishingByColorQuery(page, size, Order, unit, dateFrom, dateTo, type, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Finishing Sample By Color";

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
            GarmentSampleFinishingOutDto GarmentSampleFinishingOutDto = _GarmentSampleFinishingOutRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentSampleFinishingOutDto(finishOut)
            {
                Items = _GarmentSampleFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).Select(finishOutItem => new GarmentSampleFinishingOutItemDto(finishOutItem)
                {
                    Details = _GarmentSampleFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).Select(finishOutDetail => new GarmentSampleFinishingOutDetailDto(finishOutDetail)
                    {
                    }).ToList()

                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentSampleFinishingOutPDFTemplate.Generate(GarmentSampleFinishingOutDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{GarmentSampleFinishingOutDto.FinishingOutNo}.pdf"
            };
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentSampleFinishingOutCommand command)
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
