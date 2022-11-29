using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare;
using Manufactures.Application.GarmentPreparings.Queries.GetPrepareTraceable;
using Manufactures.Application.GarmentPreparings.Queries.GetWIP;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("preparings")]
    public class GarmentPreparingController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;

        public GarmentPreparingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentPreparingRepository = Storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = Storage.GetRepository<IGarmentPreparingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            // var query = _garmentPreparingRepository.Read(order, select, filter);
            // int totalRows = query.Count();
            // var garmentPreparingDto = _garmentPreparingRepository.Find(query).Select(o => new GarmentPreparingDto(o)).ToArray();
            // var garmentPreparingItemDto = _garmentPreparingItemRepository.Find(_garmentPreparingItemRepository.Query).Select(o => new GarmentPreparingItemDto(o)).ToList();
            // var garmentPreparingItemDtoArray = _garmentPreparingItemRepository.Find(_garmentPreparingItemRepository.Query).Select(o => new GarmentPreparingItemDto(o)).ToArray();

            // Parallel.ForEach(garmentPreparingDto, itemDto =>
            // {
            //     var garmentPreparingItems = garmentPreparingItemDto.Where(x => x.GarmentPreparingId == itemDto.Id).ToList();

            //     itemDto.Items = garmentPreparingItems;
            //     itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();
            //     itemDto.TotalQuantity = garmentPreparingItems.Sum(x => x.Quantity);
            // });

            // if (!string.IsNullOrEmpty(keyword))
            // {
            //     garmentPreparingItemDtoArray = garmentPreparingItemDto.Where(x => x.Product.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
            //     List<GarmentPreparingDto> ListTemp = new List<GarmentPreparingDto>();
            //     foreach(var a in garmentPreparingItemDtoArray)
            //     {
            //         var temp = garmentPreparingDto.Where(x => x.Id.Equals(a.GarmentPreparingId)).ToArray();
            //         foreach(var b in temp)
            //         {
            //             ListTemp.Add(b);
            //         }
            //     }
            //     var garmentPreparingDtoList = garmentPreparingDto.Where(x => x.UENNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                         || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                         || x.Unit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                         || (x.Article != null && x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            //                         //|| x.Items.Where(y => y.ProductId.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            //                         ).ToList();

            //     var i = 0;
            //     foreach(var data in ListTemp)
            //     {
            //         i = 0;
            //         foreach(var item in garmentPreparingDtoList)
            //         {
            //             if(data.Id == item.Id)
            //             {
            //                 i++;
            //             }
            //         }
            //         if (i == 0)
            //         {
            //             garmentPreparingDtoList.Add(data);
            //         }
            //     }
            //     var garmentPreparingDtoListArray = garmentPreparingDtoList.ToArray();
            //     if (order != "{}")
            //     {
            //         Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //         garmentPreparingDtoListArray = QueryHelper<GarmentPreparingDto>.Order(garmentPreparingDtoList.AsQueryable(), OrderDictionary).ToArray();
            //     }
            //     else
            //     {
            //         garmentPreparingDtoListArray = garmentPreparingDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
            //     }

            //     garmentPreparingDtoListArray = garmentPreparingDtoListArray.Skip((page - 1) * size).Take(size).ToArray();

            //     await Task.Yield();
            //     return Ok(garmentPreparingDtoListArray, info: new
            //     {
            //         page,
            //         size,
            //         total = totalRows
            //     });
            // } else
            // {
            //     if (order != "{}")
            //     {
            //         Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //         garmentPreparingDto = QueryHelper<GarmentPreparingDto>.Order(garmentPreparingDto.AsQueryable(), OrderDictionary).ToArray();
            //     }
            //     else
            //     {
            //         garmentPreparingDto = garmentPreparingDto.OrderByDescending(x => x.LastModifiedDate).ToArray();
            //     }

            //     garmentPreparingDto = garmentPreparingDto.Skip((page - 1) * size).Take(size).ToArray();

            //     await Task.Yield();
            //     return Ok(garmentPreparingDto, info: new
            //     {
            //         page,
            //         size,
            //         total = totalRows
            //     });
            // }
            
            var query = _garmentPreparingRepository.ReadOptimized(order, filter, keyword);
            var newQuery = _garmentPreparingRepository.ReadExecute(query, keyword).ToList();
            int totalRows = query.Count();
            var data = newQuery.Skip((page - 1) * size).Take(size);
            await Task.Yield();
            return Ok(data, info: new
            {
                page,
                size,
                total = totalRows
            });
        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var query = _garmentPreparingRepository.Read(order, select, filter);
            int totalRows = query.Count();
            var garmentPreparingDto = _garmentPreparingRepository.Find(query).Select(o => new GarmentPreparingDto(o)).ToArray();


            if (!string.IsNullOrEmpty(keyword))
            {
                var garmentPreparingDtoList = garmentPreparingDto.Where(x => x.UENNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Unit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || (x.Article != null && x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    //|| x.Items.Where(y => y.ProductId.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    ).ToList();

                var garmentPreparingDtoListArray = garmentPreparingDtoList.ToArray();
                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentPreparingDtoListArray = QueryHelper<GarmentPreparingDto>.Order(garmentPreparingDtoList.AsQueryable(), OrderDictionary).ToArray();
                }
                else
                {
                    garmentPreparingDtoListArray = garmentPreparingDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
                }

                garmentPreparingDtoListArray = garmentPreparingDtoListArray.Skip((page - 1) * size).Take(size).ToArray();

                await Task.Yield();
                return Ok(garmentPreparingDtoListArray, info: new
                {
                    page,
                    size,
                    total = totalRows
                });
            }
            else
            {
                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentPreparingDto = QueryHelper<GarmentPreparingDto>.Order(garmentPreparingDto.AsQueryable(), OrderDictionary).ToArray();
                }
                else
                {
                    garmentPreparingDto = garmentPreparingDto.OrderByDescending(x => x.LastModifiedDate).ToArray();
                }

                garmentPreparingDto = garmentPreparingDto.Skip((page - 1) * size).Take(size).ToArray();

                await Task.Yield();
                return Ok(garmentPreparingDto, info: new
                {
                    page,
                    size,
                    total = totalRows
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var preparingId = Guid.Parse(id);
            VerifyUser();
            var preparingDto = _garmentPreparingRepository.Find(o => o.Identity == preparingId).Select(o => new GarmentPreparingDto(o)).FirstOrDefault();

            if (preparingDto == null)
                return NotFound();

            //var selectedUnit = GetUnit(preparingDto.Unit.Id, WorkContext.Token).data;
            
            //if (selectedUnit != null)
            //{
            //    preparingDto.Unit.Name = selectedUnit.Name;
            //    preparingDto.Unit.Code = selectedUnit.Code;
            //}

            var itemConfigs = _garmentPreparingItemRepository.Find(x => x.GarmentPreparingId == preparingDto.Id).Select(o => new GarmentPreparingItemDto(o)).ToList();
            preparingDto.Items = itemConfigs;

            //Parallel.ForEach(preparingDto.Items, orderItem =>
            //{
            //    var selectedUOM = GetUom(orderItem.Uom.Id, WorkContext.Token).data;
            //    var selectedProduct = GetGarmentProduct(orderItem.Product.Id, WorkContext.Token).data;

            //    if (selectedUOM != null)
            //    {
            //        orderItem.Uom.Unit = selectedUOM.Unit;
            //    }

            //    if (selectedProduct != null)
            //    {
            //        orderItem.Product.Code = selectedProduct.Code;
            //        orderItem.Product.Name = selectedProduct.Name;
            //    }

            //});
            preparingDto.Items = preparingDto.Items.OrderBy(x => x.Id).ToList();
            await Task.Yield();

            return Ok(preparingDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceGarmentPreparingCommand command)
        {
            try
            {
                VerifyUser();

                var garmentPreparingValidation = _garmentPreparingRepository.Find(o => o.UENId == command.UENId && o.UENNo == command.UENNo && o.UnitId == command.Unit.Id
                                && o.ProcessDate == command.ProcessDate && o.RONo == command.RONo && o.Article == command.Article && o.IsCuttingIn == command.IsCuttingIn).Select(o => new GarmentPreparingDto(o)).FirstOrDefault();
                if (garmentPreparingValidation != null)
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        error = "Data sudah ada"
                    });

                var order = await Mediator.Send(command);

                await PutGarmentUnitExpenditureNoteCreate(command.UENId);

                return Ok(order.Identity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, [FromBody]UpdateGarmentPreparingCommand command)
        //{
        //    Guid orderId;
        //    try
        //    {
        //        VerifyUser();
        //        if (!Guid.TryParse(id, out orderId))
        //            return NotFound();

        //        command.SetId(orderId);


        //        var order = await Mediator.Send(command);

        //        return Ok(order.Identity);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }

        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            VerifyUser();
            var preparingId = Guid.Parse(id);

            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            var garmentPreparing = _garmentPreparingRepository.Find(x => x.Identity == preparingId).Select(o => new GarmentPreparingDto(o)).FirstOrDefault();

            var command = new RemoveGarmentPreparingCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);
            await PutGarmentUnitExpenditureNoteDelete (garmentPreparing.UENId);

            return Ok(order.Identity);
        }
		[HttpGet("loader/ro")]
        public async Task<IActionResult> GetLoaderByRO(string keyword, string filter = "{}")
        {
            var query = _garmentPreparingRepository.Read(null, null, filter);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(o => o.RONo.Contains(keyword) && o.GarmentPreparingItem.Any(a=>a.RemainingQuantity > 0));
            }

            var rOs = query.Select(o => new { o.RONo, o.Article }).Distinct().ToList();

            await Task.Yield();

            return Ok(rOs);
        }
		[HttpGet("monitoring")]
		public async Task<IActionResult> GetMonitoring(int unit, DateTime dateFrom, DateTime dateTo,int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringPrepareQuery query = new GetMonitoringPrepareQuery(page,size, Order, unit,dateFrom,dateTo,WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.garmentMonitorings , info: new
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
				GetXlsPrepareQuery query = new GetXlsPrepareQuery(page, size, Order, unit, dateFrom, dateTo,type, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Prepare";

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

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentPreparingCommand command)
        {
            VerifyUser();

            if (command.Date==null || command.Date == DateTimeOffset.MinValue)
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

        [HttpGet("wip")]
        public async Task<IActionResult> GetWIP(DateTime date, int page = 1, int size = 25, string Order = "{}")
        {
            VerifyUser();
            GetWIPQuery query = new GetWIPQuery(page, size, Order,date, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentWIP, info: new
            {
                page,
                size,
                viewModel.count
            });
        }

        [HttpGet("wip/download")]
        public async Task<IActionResult> GetXlsWIP(DateTime date, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsWIPQuery query = new GetXlsWIPQuery(page, size, Order, date, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan WIP";

                if (date != null) filename += " " + ((DateTime)date).ToString("dd-MM-yyyy");
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

        [HttpGet("byRONO")]
        public async Task<IActionResult> GetpreparingbyRONO([FromBody]string RO)
        {

            VerifyUser();

            GetPrepareTraceableQuery query = new GetPrepareTraceableQuery(RO, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.getPrepareTraceableDtos);
        }
    }
}
