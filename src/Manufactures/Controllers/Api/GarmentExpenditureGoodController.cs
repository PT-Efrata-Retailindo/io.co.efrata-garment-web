using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentExpenditureGoods.Queries;
using Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods;
using Manufactures.Application.GarmentExpenditureGoods.Queries.GetReportExpenditureGoods;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Dtos;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("expenditure-goods")]
    public class GarmentExpenditureGoodController : ControllerApiBase
    {
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;

        public GarmentExpenditureGoodController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentExpenditureGoodRepository = Storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = Storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentExpenditureGoodListDto(ExGood))
                .ToList();

            var dtoIds = garmentExpenditureGoodListDtos.Select(s => s.Id).ToList();
            var items = _garmentExpenditureGoodItemRepository.Query
                .IgnoreQueryFilters()
                .Where(o => dtoIds.Contains(o.ExpenditureGoodId))
                .Select(s => new { s.Identity, s.ExpenditureGoodId, s.Quantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(garmentExpenditureGoodListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ExpenditureGoodId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentExpenditureGoodListDtos, info: new
            {
                page,
                size,
                total,
                totalQty
            });
        }

        [HttpGet("byRO")]
        public async Task<IActionResult> GetRONo(string RONo)
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.ReadignoreFilter(1, 75, "{}", null, "{}");
            query = query.Where(x => x.RONo == RONo && x.ExpenditureType == "EXPORT").Select(x => x);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            //query = query.Skip((page - 1) * size).Take(size);

            List<GarmentExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentExpenditureGoodListDto(ExGood))
                .ToList();

            var dtoIds = garmentExpenditureGoodListDtos.Select(s => s.Id).ToList();
            var items = _garmentExpenditureGoodItemRepository.Query
                .Where(o => dtoIds.Contains(o.ExpenditureGoodId))
                .Select(s => new { s.Identity, s.ExpenditureGoodId, s.Quantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(garmentExpenditureGoodListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ExpenditureGoodId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentExpenditureGoodListDtos);
        }

        [HttpGet("traceable-by-ro")]
        public async Task<IActionResult> GetTraceablebyRONo([FromBody]string RONo)
        {
            VerifyUser();

            var ros = RONo.Contains(",") ? RONo.Split(",").ToList() : new List<string> { RONo };

            var query = _garmentExpenditureGoodRepository.Read(1, 75, "{}", null, "{}");
            query = query.Where(x => ros.Contains(x.RONo)).Select(x => x);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            //query = query.Skip((page - 1) * size).Take(size);

            List<GarmentExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentExpenditureGoodListDto(ExGood))
                .ToList();

            var dtoIds = garmentExpenditureGoodListDtos.Select(s => s.Id).ToList();
            var items = _garmentExpenditureGoodItemRepository.Query
                .Where(o => dtoIds.Contains(o.ExpenditureGoodId))
                .Select(s => new { s.Identity, s.ExpenditureGoodId, s.Quantity })
                .ToList();



            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(garmentExpenditureGoodListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ExpenditureGoodId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentExpenditureGoodListDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentExpenditureGoodDto garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentExpenditureGoodDto(finishOut)
            {
                Items = _garmentExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == finishOut.Identity).OrderBy(a=>a.Description).ThenBy(i => i.SizeName).Select(finishOutItem => new GarmentExpenditureGoodItemDto(finishOutItem)
                {
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentExpenditureGoodCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentExpenditureGoodCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpPut("update-received/{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody]bool isReceived)
        {
            Guid guid = Guid.Parse(id);


            VerifyUser();

            UpdateIsReceivedGarmentExpenditureGoodCommand command = new UpdateIsReceivedGarmentExpenditureGoodCommand(guid, isReceived);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentExpenditureGoodCommand command = new RemoveGarmentExpenditureGoodCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }
		[HttpGet("monitoring")]
		public async Task<IActionResult> GetMonitoring(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringExpenditureGoodQuery query = new GetMonitoringExpenditureGoodQuery(page, size, Order, dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.garmentMonitorings, info: new
			{
				page,
				size,
				viewModel.count
			});
		}
        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.ReadExecute(query);

            //var garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(query).Select(o => new GarmentExpenditureGoodDto(o)).ToArray();
            //var garmentExpenditureGoodItemDto = _garmentExpenditureGoodItemRepository.Find(_garmentExpenditureGoodItemRepository.Query).Select(o => new GarmentExpenditureGoodItemDto(o)).ToList();

            //Parallel.ForEach(garmentExpenditureGoodDto, itemDto =>
            //{
            //    var garmentExpenditureGoodItems = garmentExpenditureGoodItemDto.Where(x => x.ExpenditureGoodId == itemDto.Id).OrderBy(x => x.Id).ToList();

            //    itemDto.Items = garmentExpenditureGoodItems;
            //});

            //if (order != "{}")
            //{
            //    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //    garmentExpenditureGoodDto = QueryHelper<GarmentExpenditureGoodDto>.Order(garmentExpenditureGoodDto.AsQueryable(), OrderDictionary).ToArray();
            //}

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(query).Select(o => new GarmentExpenditureGoodDto(o)).ToArray();

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentExpenditureGoodDto = QueryHelper<GarmentExpenditureGoodDto>.Order(garmentExpenditureGoodDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto, info: new
            {
                page,
                size,
                count
            });
        }
        //
        [HttpGet("get-by-no")]
        public async Task<IActionResult> GetByNo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(query).Select(o => new GarmentExpenditureGoodDto(o)).ToArray();

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentExpenditureGoodDto = QueryHelper<GarmentExpenditureGoodDto>.Order(garmentExpenditureGoodDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto, info: new
            {
                page,
                size,
                count
            });
        }
        //
        [HttpGet("download")]
		public async Task<IActionResult> GetXls(DateTime dateFrom, DateTime dateTo, string type,int page = 1, int size = 25, string Order = "{}")
		{
			try
			{
				VerifyUser();
				GetXlsExpenditureGoodQuery query = new GetXlsExpenditureGoodQuery(page, size, Order, dateFrom, dateTo,type, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Pengeluaran Hasil Produksi " ;

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
            GarmentExpenditureGoodDto garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentExpenditureGoodDto(finishOut)
            {
                Items = _garmentExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == finishOut.Identity).Select(finishOutItem => new GarmentExpenditureGoodItemDto(finishOutItem)
                {
                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentExpenditureGoodPDFTemplate.Generate(garmentExpenditureGoodDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentExpenditureGoodDto.ExpenditureGoodNo}.pdf"
            };
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentExpenditureGoodCommand command)
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

        [HttpGet("mutation")]
        public async Task<IActionResult> GetMutation(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            VerifyUser();
            GetMutationExpenditureGoodsQuery query = new GetMutationExpenditureGoodsQuery(page, size, Order, dateFrom, dateTo, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentMutations, info: new
            {
                page,
                size,
                viewModel.count
            });
        }

        [HttpGet("mutation/download")]
        public async Task<IActionResult> GetXlsMutation(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsMutationExpenditureGoodsQuery query = new GetXlsMutationExpenditureGoodsQuery(page, size, Order, dateFrom, dateTo, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Mutasi Hasil Produksi";

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

        [HttpGet("report-out")]
        public async Task<IActionResult> GetReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            VerifyUser();
            GetReportExpenditureGoodsQuery query = new GetReportExpenditureGoodsQuery(page, size, Order, dateFrom, dateTo, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentReports, info: new
            {
                page,
                size,
                viewModel.count
            });
        }

        [HttpGet("report-out/download")]
        public async Task<IActionResult> GetXlsReport(DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsReportExpenditureGoodsQuery query = new GetXlsReportExpenditureGoodsQuery(page, size, Order, dateFrom, dateTo, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Pengeluaran Barang Jadi ";

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

        [HttpGet("basic-price")]
        public async Task<IActionResult> GetBasicPriceByRONo(string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.BasicPriceByRO(keyword, filter);
            
            return Ok(query);
        }
        
        [HttpGet("byInvoice")]
        public async Task<IActionResult> GetTraceablebyInvoice([FromBody]string invoice)
        {
            VerifyUser();

            var invoices = invoice.Contains(",") ? invoice.Split(",").ToList() : new List<string> { invoice };

            var query = _garmentExpenditureGoodRepository.Read(1, 75, "{}", null, "{}");
            query = query.Where(x => invoices.Contains(x.Invoice)).Select(x => x);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            //query = query.Skip((page - 1) * size).Take(size);

            List<GarmentExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentExpenditureGoodListDto(ExGood))
                .ToList();

            var dtoIds = garmentExpenditureGoodListDtos.Select(s => s.Id).ToList();
            var items = _garmentExpenditureGoodItemRepository.Query
                .Where(o => dtoIds.Contains(o.ExpenditureGoodId))
                .Select(s => new { s.Identity, s.ExpenditureGoodId, s.Quantity })
                .ToList();



            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(garmentExpenditureGoodListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ExpenditureGoodId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentExpenditureGoodListDtos);
        }

        [HttpGet("forOmzet")]
        public async Task<IActionResult> GetExpenditureForOmzet(DateTime dateFrom, DateTime dateTo, string unitcode, int offset)
        {
            VerifyUser();
            var query = _garmentExpenditureGoodRepository.Read(1, int.MaxValue, "{}", null, "{}");
            query = query.Where(x => x.ExpenditureDate.AddHours(offset).Date >= dateFrom && x.ExpenditureDate.AddHours(offset).Date <= dateTo && x.UnitCode == (string.IsNullOrWhiteSpace(unitcode) ? x.UnitCode : unitcode) && x.ExpenditureType == "EXPORT").Select(x => x);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            //query = query.Skip((page - 1) * size).Take(size);

            List<GarmentExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentExpenditureGoodListDto(ExGood))
                .ToList();

            var dtoIds = garmentExpenditureGoodListDtos.Select(s => s.Id).ToList();
            var items = _garmentExpenditureGoodItemRepository.Query
                .Where(o => dtoIds.Contains(o.ExpenditureGoodId))
                .Select(s => new { s.Identity, s.ExpenditureGoodId, s.Quantity })
                .ToList();



            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(garmentExpenditureGoodListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ExpenditureGoodId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentExpenditureGoodListDtos);
        }

        [HttpGet("forOmzetAnnual")]
        public async Task<IActionResult> GetExpenditureForAnnualOmzet(DateTime dateFrom, DateTime dateTo, int offset)
        {
            VerifyUser();
            var query = _garmentExpenditureGoodRepository.Read(1, int.MaxValue, "{}", null, "{}");
            query = query.Where(x => x.ExpenditureDate.AddHours(offset).Date >= dateFrom && x.ExpenditureDate.AddHours(offset).Date <= dateTo && x.ExpenditureType == "EXPORT").Select(x => x);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            //query = query.Skip((page - 1) * size).Take(size);

            List<GarmentExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentExpenditureGoodListDto(ExGood))
                .ToList();

            var dtoIds = garmentExpenditureGoodListDtos.Select(s => s.Id).ToList();
            var items = _garmentExpenditureGoodItemRepository.Query
                .Where(o => dtoIds.Contains(o.ExpenditureGoodId))
                .Select(s => new { s.Identity, s.ExpenditureGoodId, s.Quantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(garmentExpenditureGoodListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ExpenditureGoodId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentExpenditureGoodListDtos);
        }
    }
}
