using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("adjustments")]
    public class GarmentAdjustmentController : ControllerApiBase
    {
        private readonly IMemoryCacheManager _cacheManager;
        private readonly IGarmentAdjustmentRepository _garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository _garmentAdjustmentItemRepository;

        public GarmentAdjustmentController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentAdjustmentRepository = Storage.GetRepository<IGarmentAdjustmentRepository>();
            _garmentAdjustmentItemRepository = Storage.GetRepository<IGarmentAdjustmentItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentAdjustmentRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b=>b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentAdjustmentListDto> garmentCuttingInListDtos = _garmentAdjustmentRepository.Find(query).Select(adjustment =>
            {
                var items = _garmentAdjustmentItemRepository.Query.Where(o => o.AdjustmentId == adjustment.Identity).Select(adjustmentItem => new
                {
                    adjustmentItem.Quantity
                }).ToList();

                return new GarmentAdjustmentListDto(adjustment)
                {
                    TotalAdjustmentQuantity = Math.Round(items.Sum(i => i.Quantity), 2)
                };
            }).ToList();

            await Task.Yield();
            return Ok(garmentCuttingInListDtos, info: new
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

            GarmentAdjustmentDto garmentAdjustmentDto = _garmentAdjustmentRepository.Find(o => o.Identity == guid).Select(adjustment => new GarmentAdjustmentDto(adjustment)
            {
                Items = _garmentAdjustmentItemRepository.Find(o => o.AdjustmentId == adjustment.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(adjustmentItem => new GarmentAdjustmentItemDto(adjustmentItem)
                ).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentAdjustmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentAdjustmentCommand command)
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentAdjustmentCommand command = new RemoveGarmentAdjustmentCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentAdjustmentRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentAdjustmentDto = _garmentAdjustmentRepository.Find(query).Select(o => new GarmentAdjustmentDto(o)).ToArray();
            var garmentAdjustmentItemDto = _garmentAdjustmentItemRepository.Find(_garmentAdjustmentItemRepository.Query).Select(o => new GarmentAdjustmentItemDto(o)).ToList();

            Parallel.ForEach(garmentAdjustmentDto, itemDto =>
            {
                var garmentAdjustmentItems = garmentAdjustmentItemDto.Where(x => x.AdjustmentId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentAdjustmentItems;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentAdjustmentDto = QueryHelper<GarmentAdjustmentDto>.Order(garmentAdjustmentDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentAdjustmentDto, info: new
            {
                page,
                size,
                count
            });
        }
    }
}
