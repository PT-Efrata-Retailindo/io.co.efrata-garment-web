using Barebone.Controllers;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Newtonsoft.Json;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentCuttingOuts.Queries;
using Manufactures.Helpers.PDFTemplates;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Application.GarmentCuttingOuts.Queries.GetCuttingOutForTraceable;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("cutting-outs")]
    public class GarmentCuttingOutController : ControllerApiBase
    {
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public GarmentCuttingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentCuttingOutRepository = Storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = Storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = Storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            _garmentCuttingInRepository = Storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = Storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = Storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSewingDORepository = Storage.GetRepository<IGarmentSewingDORepository>();
            _garmentSewingDOItemRepository = Storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var cuttingOutQuery = new Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts.GetAllCuttingOutQuery(page, size, order, keyword, filter);
            var viewModel = await Mediator.Send(cuttingOutQuery);
            return Ok(viewModel.data, info: new
            {
                page,
                size,
                viewModel.total,
                count = viewModel.data.Count,
                viewModel.totalQty
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentCuttingOutDto garmentCuttingOutDto = _garmentCuttingOutRepository.Find(o => o.Identity == guid).Select(cutOut => new GarmentCuttingOutDto(cutOut)
            {
                Items = _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).Select(cutOutItem => new GarmentCuttingOutItemDto(cutOutItem)
                {
                    Details = _garmentCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).OrderBy(s=>s.Color).ThenBy(s=>s.SizeName).Select(cutOutDetail => new GarmentCuttingOutDetailDto(cutOutDetail)
                    {
                        //PreparingRemainingQuantity = _garmentPreparingItemRepository.Query.Where(o => o.Identity == cutInDetail.PreparingItemId).Select(o => o.RemainingQuantity).FirstOrDefault() + cutInDetail.PreparingQuantity,
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentCuttingOutDto);

            
        }

        [HttpGet("{id}/{buyer}")]
        public async Task<IActionResult> GetPdf(string id, string buyer)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentCuttingOutDto garmentCuttingOutDto = _garmentCuttingOutRepository.Find(o => o.Identity == guid).Select(cutOut => new GarmentCuttingOutDto(cutOut)
            {
                Items = _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).Select(cutOutItem => new GarmentCuttingOutItemDto(cutOutItem)
                {
                    Details = _garmentCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).Select(cutOutDetail => new GarmentCuttingOutDetailDto(cutOutDetail)
                    {
                        
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentCuttingOutPDFTemplate.Generate(garmentCuttingOutDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentCuttingOutDto.CutOutNo}.pdf"
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentCuttingOutCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentCuttingOutCommand command)
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
            var usedData = false;
            var garmentSewingDO = _garmentSewingDORepository.Query.Where(o => o.CuttingOutId == guid).Select(o => new GarmentSewingDO(o)).Single();

            _garmentSewingDOItemRepository.Find(x => x.SewingDOId == garmentSewingDO.Identity).ForEach(async sewingDOItem =>
            {
                if (sewingDOItem.RemainingQuantity < sewingDOItem.Quantity)
                {
                    usedData = true;
                }
            });

            if(usedData == true)
            {
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Data Sudah Digunakan di Sewing In"
                });
            } else
            {
                RemoveGarmentCuttingOutCommand command = new RemoveGarmentCuttingOutCommand(guid);
                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
        }

		[HttpGet("monitoring")]
		public async Task<IActionResult> GetMonitoring(int unit, DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringCuttingQuery query = new GetMonitoringCuttingQuery(page, size, Order, unit, dateFrom, dateTo, WorkContext.Token);
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
				GetXlsCuttingQuery query = new GetXlsCuttingQuery(page, size, Order, unit, dateFrom, dateTo, type,WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Cutting";

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

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentCuttingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var newQuery = _garmentCuttingOutRepository.ReadExecute(query);
            //var garmentCuttingOutDto = _garmentCuttingOutRepository.Find(query).Select(o => new GarmentCuttingOutDto(o)).ToArray();
            ////var CuttingOutsIdentities = garmentCuttingOutDto.Select(x => x.Id).ToArray();
            //var garmentCuttingOutItemDto = _garmentCuttingOutItemRepository.Find(_garmentCuttingOutItemRepository.Query).Where(x=> CuttingOutsIdentities.Contains(x.CutOutId)).Select(o => new GarmentCuttingOutItemDto(o)).ToList();
            ////var CuttingOutItemsIdentities = garmentCuttingOutDto.Select(x => x.Id).ToArray();
            //var garmentCuttingOutDetailDto = _garmentCuttingOutDetailRepository.Find(_garmentCuttingOutDetailRepository.Query).Where(x=> CuttingOutItemsIdentities.Contains(x.CutOutItemId)).Select(o => new GarmentCuttingOutDetailDto(o)).ToList();

            //Parallel.ForEach(garmentCuttingOutDto, itemDto =>
            //{
            //    var garmentCuttingOutItems = garmentCuttingOutItemDto.Where(x => x.CutOutId == itemDto.Id).OrderBy(x => x.Id).ToList();

            //    itemDto.Items = garmentCuttingOutItems;

            //    Parallel.ForEach(itemDto.Items, detailDto =>
            //    {
            //        var garmentCuttingInDetails = garmentCuttingOutDetailDto.Where(x => x.CutOutItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
            //        detailDto.Details = garmentCuttingInDetails;
            //    });
            //});

            //if (order != "{}")
            //{
            //    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //    garmentCuttingOutDto = QueryHelper<GarmentCuttingOutDto>.Order(garmentCuttingOutDto.AsQueryable(), OrderDictionary).ToArray();
            //}

            await Task.Yield();
            return Ok(newQuery, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentCuttingOutCommand command)
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
        public async Task<IActionResult> GetForTraceable ([FromBody]string RONo)
        {
            VerifyUser();

            var ros = RONo.Contains(",") ? RONo.Split(",").ToList() : new List<string> { RONo };

            GetCuttingOutForTraceableQuery query = new GetCuttingOutForTraceableQuery(ros, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.data);
        }
    }
}