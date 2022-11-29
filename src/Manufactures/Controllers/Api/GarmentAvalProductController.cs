using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
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
    [Route("aval-products")]
    public class GarmentAvalProductController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentAvalProductRepository _garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository _garmentAvalProductItemRepository;
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;

        public GarmentAvalProductController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentAvalProductRepository = Storage.GetRepository<IGarmentAvalProductRepository>();
            _garmentAvalProductItemRepository = Storage.GetRepository<IGarmentAvalProductItemRepository>();
            _garmentPreparingRepository = Storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = Storage.GetRepository<IGarmentPreparingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var query = _garmentAvalProductRepository.Read(order, select, filter);
            

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }
            int totalRows = query.Count();
            double totalQty = query.Sum(a => a.GarmentAvalProductItem.Sum(b => b.Quantity));

            query = query.Skip((page - 1) * size).Take(size);
            var garmentAvalProductDto = _garmentAvalProductRepository.Find(query).Select(o => new GarmentAvalProductDto(o)).OrderByDescending(x => x.LastModifiedDate).ToArray();
            var dtoIds = garmentAvalProductDto.Select(s => s.Id).ToList();
            var query2 = _garmentAvalProductItemRepository.Query.Where(x => dtoIds.Contains(x.APId));
            var garmentAvalProductItemDto = _garmentAvalProductItemRepository.Find(query2).Select(o => new GarmentAvalProductItemDto(o)).ToList();

            //var garmentAvalProductItemDto = _garmentAvalProductItemRepository.Find(_garmentAvalProductItemRepository.Query).Select(o => new GarmentAvalProductItemDto(o)).ToList();

            Parallel.ForEach(garmentAvalProductDto, itemDto =>
            {
                var garmentPreparingItems = garmentAvalProductItemDto.Where(x => x.APId == itemDto.Id).ToList();

                itemDto.Items = garmentPreparingItems;

                //Parallel.ForEach(itemDto.Items, orderItem =>
                //{
                //    var selectedProduct = GetGarmentProduct(orderItem.Product.Id, WorkContext.Token);
                //    var selectedUom = GetUom(orderItem.Uom.Id, WorkContext.Token);

                //    if (selectedProduct != null && selectedProduct.data != null)
                //    {
                //        orderItem.Product.Name = selectedProduct.data.Name;
                //        orderItem.Product.Code = selectedProduct.data.Code;
                //    }

                //    if (selectedUom != null && selectedUom.data != null)
                //    {
                //        orderItem.Uom.Unit = selectedUom.data.Unit;
                //    }
                //});

                itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();
            });

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    garmentAvalProductDto = garmentAvalProductDto.Where(x => x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                        || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
            //}

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentAvalProductDto = QueryHelper<GarmentAvalProductDto>.Order(garmentAvalProductDto.AsQueryable(), OrderDictionary).ToArray();
            }

            //garmentAvalProductDto = garmentAvalProductDto.Take(size).Skip((page - 1) * size).ToArray();

            await Task.Yield();
            return Ok(garmentAvalProductDto, info: new
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
            var avalProductId = Guid.Parse(id);
            VerifyUser();
            var avalProductDto = _garmentAvalProductRepository.Find(o => o.Identity == avalProductId).Select(o => new GarmentAvalProductDto(o)).FirstOrDefault();

            if (avalProductId == null)
                return NotFound();

            var itemConfigs = _garmentAvalProductItemRepository.Find(x => x.APId == avalProductDto.Id).Select(o => new GarmentAvalProductItemDto(o)).ToList();
            avalProductDto.Items = itemConfigs;

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
            avalProductDto.Items = avalProductDto.Items.OrderBy(x => x.Id).ToList();
            await Task.Yield();

            return Ok(avalProductDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceGarmentAvalProductCommand command)
        {
            try
            {
                VerifyUser();

                //var garmentAvalProductValidation = _garmentAvalProductRepository.Find(o => o.RONo == command.RONo && o.Article == command.Article && o.AvalDate == command.AvalDate).Select(o => new GarmentAvalProductDto(o)).FirstOrDefault();
                //if (garmentAvalProductValidation != null)
                //    return BadRequest(new
                //    {
                //        code = HttpStatusCode.BadRequest,
                //        error = "Data sudah ada"
                //    });

                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            VerifyUser();
            var avalProductId = Guid.Parse(id);

            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            var garmentPreparing = _garmentAvalProductRepository.Find(x => x.Identity == avalProductId).Select(o => new GarmentAvalProductDto(o)).FirstOrDefault();

            var command = new RemoveGarmentAvalProductCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpPut("update-received")]
        public async Task<IActionResult> UpdateIsReceived([FromBody]UpdateIsReceivedGarmentAvalProductCommand command)
        {
            VerifyUser();
            var order = await Mediator.Send(command);

            return Ok();
        }
    }
}