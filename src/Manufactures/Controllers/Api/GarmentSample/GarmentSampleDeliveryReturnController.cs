using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Dtos.GarmentSample.SampleDeliveryReturns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("garment-sample-delivery-returns")]
    public class GarmentSampleDeliveryReturnController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentSampleDeliveryReturnRepository _garmentSampleDeliveryReturnRepository;
        private readonly IGarmentSampleDeliveryReturnItemRepository _garmentSampleDeliveryReturnItemRepository;

        public GarmentSampleDeliveryReturnController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSampleDeliveryReturnRepository = Storage.GetRepository<IGarmentSampleDeliveryReturnRepository>();
            _garmentSampleDeliveryReturnItemRepository = Storage.GetRepository<IGarmentSampleDeliveryReturnItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var query = _garmentSampleDeliveryReturnRepository.Read(order, select, filter);
            int totalRows = query.Count();
            double totalQty = query.Sum(a => a.GarmentSampleDeliveryReturnItem.Sum(b => b.Quantity));
            var garmentSampleDeliveryReturnDto = _garmentSampleDeliveryReturnRepository.Find(query).Select(o => new GarmentSampleDeliveryReturnDto(o)).OrderByDescending(x => x.LastModifiedDate).ToArray();
            var garmentSampleDeliveryReturnItemDto = _garmentSampleDeliveryReturnItemRepository.Find(_garmentSampleDeliveryReturnItemRepository.Query).Select(o => new GarmentSampleDeliveryReturnItemDto(o)).ToList();

            Parallel.ForEach(garmentSampleDeliveryReturnDto, itemDto =>
            {
                if (itemDto.Storage.Name == null)
                {
                    itemDto.Storage.Name = "-";
                }
                var garmentSampleDeliveryReturnItems = garmentSampleDeliveryReturnItemDto.Where(x => x.DRId == itemDto.Id).ToList();

                itemDto.Items = garmentSampleDeliveryReturnItems;

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
                garmentSampleDeliveryReturnDto = garmentSampleDeliveryReturnDto.Where(x => x.DRNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.UnitDONo.Contains(keyword, StringComparison.OrdinalIgnoreCase) || x.Unit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Storage.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
            }

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSampleDeliveryReturnDto = QueryHelper<GarmentSampleDeliveryReturnDto>.Order(garmentSampleDeliveryReturnDto.AsQueryable(), OrderDictionary).ToArray();
            }

            garmentSampleDeliveryReturnDto = garmentSampleDeliveryReturnDto.Take(size).Skip((page - 1) * size).ToArray();

            await Task.Yield();
            return Ok(garmentSampleDeliveryReturnDto, info: new
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
            var deliveryReturnDto = _garmentSampleDeliveryReturnRepository.Find(o => o.Identity == deliveryReturnId).Select(o => new GarmentSampleDeliveryReturnDto(o)).FirstOrDefault();

            if (deliveryReturnId == null)
                return NotFound();

            var itemConfigs = _garmentSampleDeliveryReturnItemRepository.Find(x => x.DRId == deliveryReturnDto.Id).Select(o => new GarmentSampleDeliveryReturnItemDto(o)).ToList();
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
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleDeliveryReturnCommand command)
        {
            try
            {
                VerifyUser();

                var garmentSampleDeliveryReturnValidation = _garmentSampleDeliveryReturnRepository.Find(o => o.DRNo == command.DRNo && o.RONo == command.RONo && o.Article == command.Article && o.UnitDOId == command.UnitDOId
                                && o.UnitDONo == command.UnitDONo && o.UENId == command.UENId && o.PreparingId == command.PreparingId && o.ReturnDate == command.ReturnDate && o.ReturnType == command.ReturnType
                                && o.UnitId == command.Unit.Id && o.UnitCode == command.Unit.Code && o.UnitName == command.Unit.Name && o.StorageId == command.Storage.Id && o.StorageCode == command.Storage.Code
                                && o.StorageName == command.Storage.Name && o.IsUsed == command.IsUsed).Select(o => new GarmentSampleDeliveryReturnDto(o)).FirstOrDefault();
                if (garmentSampleDeliveryReturnValidation != null)
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        error = "Data sudah ada"
                    });

                var order = await Mediator.Send(command);

                foreach (var item in command.Items)
                {
                    if (item.Product.Name != "FABRIC")
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleDeliveryReturnCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetId(guid);

            VerifyUser();

            foreach (var item in command.Items)
            {
                var garmentDeliveryReturnItems = _garmentSampleDeliveryReturnItemRepository.Find(x => x.Identity == item.Id).Single();
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

            var garmentSampleDeliveryReturnItems = _garmentSampleDeliveryReturnItemRepository.Find(x => x.DRId == deliveryReturnId);
            foreach (var item in garmentSampleDeliveryReturnItems)
            {
                if (item.ProductName != "FABRIC")
                {
                    await PutGarmentUnitExpenditureNoteCreateForDeliveryReturn(item.UENItemId, 0, item.Quantity);
                }
            };

            var command = new RemoveGarmentSampleDeliveryReturnCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);


            return Ok(order.Identity);
        }
    }
}
