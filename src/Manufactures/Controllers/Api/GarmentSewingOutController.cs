using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsByRONo;
using Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsDynamic;
using Manufactures.Application.GarmentSewingOuts.Queries.MonitoringSewing;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
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
    [Route("sewing-outs")]
    public class GarmentSewingOutController : ControllerApiBase
    {
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public GarmentSewingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSewingOutRepository = Storage.GetRepository<IGarmentSewingOutRepository>();
            _garmentSewingOutItemRepository = Storage.GetRepository<IGarmentSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = Storage.GetRepository<IGarmentSewingOutDetailRepository>();
            _garmentSewingInItemRepository = Storage.GetRepository<IGarmentSewingInItemRepository>();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSewingOutItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSewingOutListDto> garmentSewingOutListDtos = _garmentSewingOutRepository
                .Find(query)
                .Select(SewOut => new GarmentSewingOutListDto(SewOut))
                .ToList();

            var dtoIds = garmentSewingOutListDtos.Select(s => s.Id).ToList();
            var items = _garmentSewingOutItemRepository.Query
                .Where(o => dtoIds.Contains(o.SewingOutId))
                .Select(s => new { s.Identity, s.SewingOutId, s.ProductCode, s.Color , s.Quantity, s.RemainingQuantity})
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            var details = _garmentSewingOutDetailRepository.Query
                .Where(o => itemIds.Contains(o.SewingOutItemId))
                .Select(s => new { s.Identity, s.SewingOutItemId })
                .ToList();

            Parallel.ForEach(garmentSewingOutListDtos, dto =>
            {
                var currentItems = items.Where(w => w.SewingOutId == dto.Id);
                dto.Colors = currentItems.Where(i => i.Color != null).Select(i => i.Color).Distinct().ToList();
                dto.Products = currentItems.Select(i =>  i.ProductCode).Distinct().ToList();
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

            GarmentSewingOutDto garmentSewingOutDto = _garmentSewingOutRepository.Find(o => o.Identity == guid).Select(sewOut => new GarmentSewingOutDto(sewOut)
            {
                Items = _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(sewOutItem => new GarmentSewingOutItemDto(sewOutItem)
                {
                    Details = _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).OrderBy(i => i.SizeName).Select(sewOutDetail => new GarmentSewingOutDetailDto(sewOutDetail)
                    {
                    }).ToList()
                    
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSewingOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSewingOutCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSewingOutCommand command)
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

            RemoveGarmentSewingOutCommand command = new RemoveGarmentSewingOutCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 10, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingOutRepository.ReadComplete(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSewingOutDto = _garmentSewingOutRepository.Find(query).Select(o => new GarmentSewingOutDto(o)).ToArray();

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSewingOutDto = QueryHelper<GarmentSewingOutDto>.Order(garmentSewingOutDto.AsQueryable(), OrderDictionary).ToArray();
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

            var result = await Mediator.Send(new GetGarmentSewingOutsByRONoQuery(keyword, filter));

            return Ok(result.data);
        }

        [HttpGet("dynamic")]
        public async Task<IActionResult> GetDynamic(int page = 1, int size = 25, string order = "{}", string search = "[]", string select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var result = await Mediator.Send(new GetGarmentSewingOutsDynamicQuery(page, size, order, search, select, keyword, filter));

            return Ok(result.data, info: new
            {
                page,
                size,
                count = result.data.Count,
                result.total
            });
        }

		[HttpGet("monitoring")]
		public async Task<IActionResult> GetMonitoring(int unit, DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringSewingQuery query = new GetMonitoringSewingQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
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
				GetXlsSewingQuery query = new GetXlsSewingQuery(page, size, Order, unit, dateFrom, dateTo,type, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Sewing";

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
            GarmentSewingOutDto garmentSewingOutDto = _garmentSewingOutRepository.Find(o => o.Identity == guid).Select(sewOut => new GarmentSewingOutDto(sewOut)
            {
                Items = _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).Select(sewOutItem => new GarmentSewingOutItemDto(sewOutItem)
                {
                    Details = _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).Select(sewOutDetail => new GarmentSewingOutDetailDto(sewOutDetail)
                    {
                    }).ToList()

                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentSewingOutPDFTemplate.Generate(garmentSewingOutDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentSewingOutDto.SewingOutNo}.pdf"
            };
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentSewingOutCommand command)
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
