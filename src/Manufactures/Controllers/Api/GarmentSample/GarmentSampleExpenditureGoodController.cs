using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries;
using Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries.ArchiveMonitoring;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Dtos.GarmentSample.SampleExpenditureGoods;
using Manufactures.Helpers.PDFTemplates.GarmentSample;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("garment-sample-expenditure-goods")]
    public class GarmentSampleExpenditureGoodController : ControllerApiBase
    {
        private readonly IGarmentSampleExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentSampleExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;

        public GarmentSampleExpenditureGoodController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentExpenditureGoodRepository = Storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = Storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSampleExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentSampleExpenditureGoodListDto(ExGood))
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

            List<GarmentSampleExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentSampleExpenditureGoodListDto(ExGood))
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

            List<GarmentSampleExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentSampleExpenditureGoodListDto(ExGood))
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

            GarmentSampleExpenditureGoodDto garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentSampleExpenditureGoodDto(finishOut)
            {
                Items = _garmentExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == finishOut.Identity).OrderBy(a => a.Description).ThenBy(i => i.SizeName).Select(finishOutItem => new GarmentSampleExpenditureGoodItemDto(finishOutItem)
                {
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleExpenditureGoodCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);
                if(order.PackingListId>0)
                {
                    await SetIsSampleExpenditureGood(order.Invoice, true);
                }

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleExpenditureGoodCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var packing = _garmentExpenditureGoodRepository.Find(o => guid == o.Identity).Select(s => new { s.Invoice, s.PackingListId }).Single();

            var order = await Mediator.Send(command);

            if (packing.Invoice != command.Invoice)
            {
                var isExist = _garmentExpenditureGoodRepository.Find(o => o.Invoice == packing.Invoice).FirstOrDefault();
                if (isExist == null)
                {
                    if (packing.PackingListId > 0)
                    {
                        await SetIsSampleExpenditureGood(packing.Invoice, false);
                    }
                    if (command.PackingListId > 0)
                    {
                        await SetIsSampleExpenditureGood(command.Invoice, true);
                    }
                }
                else
                {
                    if (command.PackingListId > 0)
                    {
                        await SetIsSampleExpenditureGood(command.Invoice, true);
                    }
                }
            }
            else
            {
                if (command.PackingListId > 0)
                {
                    await SetIsSampleExpenditureGood(command.Invoice, true);
                }
            }
            

            return Ok(order.Identity);
        }

        //[HttpPut("update-received/{id}")]
        //public async Task<IActionResult> Patch(string id, [FromBody]bool isReceived)
        //{
        //    Guid guid = Guid.Parse(id);


        //    VerifyUser();

        //    UpdateIsReceivedGarmentSampleExpenditureGoodCommand command = new UpdateIsReceivedGarmentSampleExpenditureGoodCommand(guid, isReceived);
        //    var order = await Mediator.Send(command);

        //    return Ok(order.Identity);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();
            var packing = _garmentExpenditureGoodRepository.Find(o => guid == o.Identity).Select(s => new { s.Invoice, s.PackingListId }).Single();

            RemoveGarmentSampleExpenditureGoodCommand command = new RemoveGarmentSampleExpenditureGoodCommand(guid);

            var order = await Mediator.Send(command);

            if (packing.PackingListId>0)
            {
                var isExist = _garmentExpenditureGoodRepository.Find(o => o.Invoice == packing.Invoice).FirstOrDefault();
                if (isExist == null)
                {
                    await SetIsSampleExpenditureGood(packing.Invoice, false);
                }
            }
                

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.ReadExecute(query);


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

            var garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(query).Select(o => new GarmentSampleExpenditureGoodDto(o)).ToArray();

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentExpenditureGoodDto = QueryHelper<GarmentSampleExpenditureGoodDto>.Order(garmentExpenditureGoodDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto, info: new
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
            GetMonitoringSampleExpenditureGoodQuery query = new GetMonitoringSampleExpenditureGoodQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
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
                GetXlsSampleExpenditureGoodQuery query = new GetXlsSampleExpenditureGoodQuery(page, size, Order, unit, dateFrom, dateTo, type, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Pengiriman Barang Jadi Sample ";

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
            GarmentSampleExpenditureGoodDto garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentSampleExpenditureGoodDto(finishOut)
            {
                Items = _garmentExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == finishOut.Identity).Select(finishOutItem => new GarmentSampleExpenditureGoodItemDto(finishOutItem)
                {
                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentSampleExpenditureGoodPDFTemplate.Generate(garmentExpenditureGoodDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentExpenditureGoodDto.ExpenditureGoodNo}.pdf"
            };
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentSampleExpenditureGoodCommand command)
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

        [HttpGet("archive-monitoring")]
        public async Task<IActionResult> GetArchiveMonitoring(string type, string roNo, string comodity, int page = 1, int size = 25, string Order = "{}")
        {
            VerifyUser();
            GarmentArchiveMonitoringQuery query = new GarmentArchiveMonitoringQuery(page, size, Order, type,roNo,comodity, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentMonitorings, info: new
            {
                page,
                size,
                viewModel.count
            });
        }

        [HttpGet("archive-download")]
        public async Task<IActionResult> GetArchiveXls(string type, string roNo, string comodity,  int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GarmentArchiveMonitoringXlsQuery query = new GarmentArchiveMonitoringXlsQuery(page, size, Order, type, roNo, comodity, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Monitoring Arsip Sampel/MD";

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
        //
        [HttpGet("forOmzet")]
        public async Task<IActionResult> GetExpenditureForOmzet(DateTime dateFrom, DateTime dateTo, string unitcode, int offset)
        {
            VerifyUser();
            var query = _garmentExpenditureGoodRepository.Read(1, int.MaxValue, "{}", null, "{}");
            query = query.Where(x => x.ExpenditureDate.AddHours(offset).Date >= dateFrom && x.ExpenditureDate.AddHours(offset).Date <= dateTo && x.UnitCode == (string.IsNullOrWhiteSpace(unitcode) ? x.UnitCode : unitcode) && x.ExpenditureType == "EXPORT").Select(x => x);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));

            List<GarmentSampleExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentSampleExpenditureGoodListDto(ExGood))
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

            List<GarmentSampleExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentSampleExpenditureGoodListDto(ExGood))
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
        //
    }
}
