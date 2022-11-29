using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("delivery-returns")]
    public class GarmentDeliveryReturnController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentDeliveryReturnRepository _garmentDeliveryReturnRepository;
        private readonly IGarmentDeliveryReturnItemRepository _garmentDeliveryReturnItemRepository;

        public GarmentDeliveryReturnController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentDeliveryReturnRepository = Storage.GetRepository<IGarmentDeliveryReturnRepository>();
            _garmentDeliveryReturnItemRepository = Storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var query = _garmentDeliveryReturnRepository.Read(order, select, filter);
            int totalRows = query.Count();
            double totalQty = query.Sum(a => a.GarmentDeliveryReturnItem.Sum(b => b.Quantity));
            var garmentDeliveryReturnDto = _garmentDeliveryReturnRepository.Find(query).Select(o => new GarmentDeliveryReturnDto(o)).OrderByDescending(x => x.LastModifiedDate).ToArray();
            var garmentDeliveryReturnItemDto = _garmentDeliveryReturnItemRepository.Find(_garmentDeliveryReturnItemRepository.Query).Select(o => new GarmentDeliveryReturnItemDto(o)).ToList();

            Parallel.ForEach(garmentDeliveryReturnDto, itemDto =>
            {
                if(itemDto.Storage.Name == null)
                {
                    itemDto.Storage.Name = "-";
                }
                var garmentDeliveryReturnItems = garmentDeliveryReturnItemDto.Where(x => x.DRId == itemDto.Id).ToList();

                itemDto.Items = garmentDeliveryReturnItems;

                Parallel.ForEach(itemDto.Items, orderItem =>
                {
                    //var selectedProduct = GetGarmentProduct(orderItem.Product.Id, WorkContext.Token);
                    //var selectedUom = GetUom(orderItem.Uom.Id, WorkContext.Token);

                    //if (selectedProduct != null && selectedProduct.data != null)
                    //{
                    //    orderItem.Product.Name = selectedProduct.data.Name;
                    //    orderItem.Product.Code = selectedProduct.data.Code;
                    //}

                    //if (selectedUom != null && selectedUom.data != null)
                    //{
                    //    orderItem.Uom.Unit = selectedUom.data.Unit;
                    //}
                });

                itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();
            });

            if (!string.IsNullOrEmpty(keyword))
            {
                garmentDeliveryReturnDto = garmentDeliveryReturnDto.Where(x => x.DRNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.UnitDONo.Contains(keyword, StringComparison.OrdinalIgnoreCase) || x.Unit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) 
                                    || x.Storage.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
            }

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentDeliveryReturnDto = QueryHelper<GarmentDeliveryReturnDto>.Order(garmentDeliveryReturnDto.AsQueryable(), OrderDictionary).ToArray();
            }

            garmentDeliveryReturnDto = garmentDeliveryReturnDto.Take(size).Skip((page - 1) * size).ToArray();

            await Task.Yield();
            return Ok(garmentDeliveryReturnDto, info: new
            {
                page,
                size,
                count = totalRows,
                totalQty
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var deliveryReturnId = Guid.Parse(id);
            VerifyUser();
            var deliveryReturnDto = _garmentDeliveryReturnRepository.Find(o => o.Identity == deliveryReturnId).Select(o => new GarmentDeliveryReturnDto(o)).FirstOrDefault();

            if (deliveryReturnId == null)
                return NotFound();

            var itemConfigs = _garmentDeliveryReturnItemRepository.Find(x => x.DRId == deliveryReturnDto.Id).Select(o => new GarmentDeliveryReturnItemDto(o)).ToList();
            deliveryReturnDto.Items = itemConfigs;

            //Parallel.ForEach(avalProductDto.Items, orderItem =>
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
            deliveryReturnDto.Items = deliveryReturnDto.Items.OrderBy(x => x.Id).ToList();
            await Task.Yield();

            return Ok(deliveryReturnDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceGarmentDeliveryReturnCommand command)
        {
            try
            {
                VerifyUser();

                var garmentDeliveryReturnValidation = _garmentDeliveryReturnRepository.Find(o => o.DRNo == command.DRNo && o.RONo == command.RONo && o.Article == command.Article && o.UnitDOId == command.UnitDOId
                                && o.UnitDONo == command.UnitDONo && o.UENId == command.UENId && o.PreparingId == command.PreparingId && o.ReturnDate == command.ReturnDate && o.ReturnType == command.ReturnType
                                && o.UnitId == command.Unit.Id && o.UnitCode == command.Unit.Code && o.UnitName == command.Unit.Name && o.StorageId == command.Storage.Id && o.StorageCode == command.Storage.Code
                                && o.StorageName == command.Storage.Name && o.IsUsed == command.IsUsed).Select(o => new GarmentDeliveryReturnDto(o)).FirstOrDefault();
                if (garmentDeliveryReturnValidation != null)
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        error = "Data sudah ada"
                    });

                var order = await Mediator.Send(command);

                foreach (var item in command.Items)
                {
                    if(item.Product.Name != "FABRIC")
                    {
                        await PutGarmentUnitExpenditureNoteCreateForDeliveryReturn(item.UENItemId, item.Quantity, 0);
                    }
                };
                

                return Ok(order.Identity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentDeliveryReturnCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetId(guid);

            VerifyUser();

            foreach (var item in command.Items)
            {
                var garmentDeliveryReturnItems = _garmentDeliveryReturnItemRepository.Find(x => x.Identity == item.Id).Single();
                if (item.IsSave == true && item.Product.Name != "FABRIC")
                {
                    await PutGarmentUnitExpenditureNoteCreateForDeliveryReturn(item.UENItemId, item.Quantity, garmentDeliveryReturnItems.Quantity);
                }
                else if (item.IsSave == false && item.Product.Name != "FABRIC")
                {
                    await PutGarmentUnitExpenditureNoteCreateForDeliveryReturn(item.UENItemId, 0, garmentDeliveryReturnItems.Quantity);
                }

            };

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            VerifyUser();
            var deliveryReturnId = Guid.Parse(id);

            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            var garmentDeliveryReturnItems = _garmentDeliveryReturnItemRepository.Find(x => x.DRId == deliveryReturnId);
            foreach (var item in garmentDeliveryReturnItems)
            {
                if (item.ProductName != "FABRIC")
                {
                    await PutGarmentUnitExpenditureNoteCreateForDeliveryReturn(item.UENItemId, 0, item.Quantity);
                }
            };

            var command = new RemoveGarmentDeliveryReturnCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);
            

            return Ok(order.Identity);
        }
    }
}