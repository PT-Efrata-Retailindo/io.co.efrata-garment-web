using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Dtos.GarmentSample.SampleAvalProducts;
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
    [Route("garment-sample-aval-products")]
    public class GarmentSampleAvalProductController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentSampleAvalProductRepository _garmentSampleAvalProductRepository;
        private readonly IGarmentSampleAvalProductItemRepository _garmentSampleAvalProductItemRepository;
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;

        public GarmentSampleAvalProductController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSampleAvalProductRepository = Storage.GetRepository<IGarmentSampleAvalProductRepository>();
            _garmentSampleAvalProductItemRepository = Storage.GetRepository<IGarmentSampleAvalProductItemRepository>();
            _garmentSamplePreparingRepository = Storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = Storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var query = _garmentSampleAvalProductRepository.Read(order, select, filter);


            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }
            int totalRows = query.Count();
            double totalQty = query.Sum(a => a.GarmentSampleAvalProductItem.Sum(b => b.Quantity));

            query = query.Skip((page - 1) * size).Take(size);
            var garmentSampleAvalProductDto = _garmentSampleAvalProductRepository.Find(query).Select(o => new GarmentSampleAvalProductDto(o)).OrderByDescending(x => x.LastModifiedDate).ToArray();
            var dtoIds = garmentSampleAvalProductDto.Select(s => s.Id).ToList();
            var query2 = _garmentSampleAvalProductItemRepository.Query.Where(x => dtoIds.Contains(x.APId));
            var garmentSampleAvalProductItemDto = _garmentSampleAvalProductItemRepository.Find(query2).Select(o => new GarmentSampleAvalProductItemDto(o)).ToList();

            //var garmentAvalProductItemDto = _garmentAvalProductItemRepository.Find(_garmentAvalProductItemRepository.Query).Select(o => new GarmentAvalProductItemDto(o)).ToList();

            Parallel.ForEach(garmentSampleAvalProductDto, itemDto =>
            {
                var garmentSamplePreparingItems = garmentSampleAvalProductItemDto.Where(x => x.APId == itemDto.Id).ToList();

                itemDto.Items = garmentSamplePreparingItems;

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
                garmentSampleAvalProductDto = QueryHelper<GarmentSampleAvalProductDto>.Order(garmentSampleAvalProductDto.AsQueryable(), OrderDictionary).ToArray();
            }

            //garmentAvalProductDto = garmentAvalProductDto.Take(size).Skip((page - 1) * size).ToArray();

            await Task.Yield();
            return Ok(garmentSampleAvalProductDto, info: new
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
            var avalProductDto = _garmentSampleAvalProductRepository.Find(o => o.Identity == avalProductId).Select(o => new GarmentSampleAvalProductDto(o)).FirstOrDefault();

            if (avalProductId == null)
                return NotFound();

            var itemConfigs = _garmentSampleAvalProductItemRepository.Find(x => x.APId == avalProductDto.Id).Select(o => new GarmentSampleAvalProductItemDto(o)).ToList();
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
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleAvalProductCommand command)
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

            var garmentSamplePreparing = _garmentSampleAvalProductRepository.Find(x => x.Identity == avalProductId).Select(o => new GarmentSampleAvalProductDto(o)).FirstOrDefault();

            var command = new RemoveGarmentSampleAvalProductCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpPut("update-received")]
        public async Task<IActionResult> UpdateIsReceived([FromBody] UpdateIsReceivedGarmentSampleAvalProductCommand command)
        {
            VerifyUser();
            var order = await Mediator.Send(command);

            return Ok();
        }
    }
}
