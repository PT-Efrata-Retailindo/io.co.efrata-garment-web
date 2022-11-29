using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentSubconFinishingIns.Commands;
using Manufactures.Dtos;
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
    [Route("finishing-ins")]
    public class GarmentFinishingInController : ControllerApiBase
    {
        private readonly IMemoryCacheManager _cacheManager;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;

        public GarmentFinishingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentFinishingInRepository = Storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = Storage.GetRepository<IGarmentFinishingInItemRepository>();
            _garmentSewingOutItemRepository = Storage.GetRepository<IGarmentSewingOutItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishingInRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentFinishingInListDto> garmentCuttingInListDtos = _garmentFinishingInRepository.Find(query).Select(loading =>
            {
                var items = _garmentFinishingInItemRepository.Query.Where(o => o.FinishingInId == loading.Identity).Select(loadingItem => new
                {
                    loadingItem.ProductCode,
                    loadingItem.ProductName,
                    loadingItem.Quantity,
                    loadingItem.RemainingQuantity
                }).ToList();

                return new GarmentFinishingInListDto(loading)
                {
                    Products = items.Select(i => i.ProductName).Distinct().ToList(),
                    TotalFinishingInQuantity = Math.Round(items.Sum(i => i.Quantity), 2),
                    TotalRemainingQuantity = Math.Round(items.Sum(i => i.RemainingQuantity), 2),

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

        //[HttpGet("get-by-ro")]
        //public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        //{
        //    VerifyUser();

        //    var query = _garmentFinishingInRepository.ReadComplete(page, size, order, keyword, filter);
        //    var count = query.Count();

        //    var garmentFinishingInDto = _garmentFinishingInRepository.Find(query).Select(o => new GarmentFinishingInDto(o)).ToArray();

        //    if (order != "{}")
        //    {
        //        Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
        //        garmentFinishingInDto = QueryHelper<GarmentFinishingInDto>.Order(garmentFinishingInDto.AsQueryable(), OrderDictionary).ToArray();
        //    }

        //    await Task.Yield();
        //    return Ok(garmentFinishingInDto, info: new
        //    {
        //        page,
        //        size,
        //        count
        //    });
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentFinishingInDto garmentFinishingInDto = _garmentFinishingInRepository.Find(o => o.Identity == guid).Select(loading => new GarmentFinishingInDto(loading)
            {
                Items = _garmentFinishingInItemRepository.Find(o => o.FinishingInId == loading.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(loadingItem => new GarmentFinishingInItemDto(loadingItem)
                ).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentFinishingInDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentFinishingInCommand command)
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

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentFinishingInCommand command)
        //{
        //    Guid guid = Guid.Parse(id);

        //    command.SetIdentity(guid);

        //    VerifyUser();

        //    var order = await Mediator.Send(command);

        //    return Ok(order.Identity);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentFinishingInCommand command = new RemoveGarmentFinishingInCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishingInRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentFinishingInDto = _garmentFinishingInRepository.Find(query).Select(o => new GarmentFinishingInDto(o)).ToList();
            
            //Enhance Jason Aug 2021
            var finishingInIds = garmentFinishingInDto.Select(s => s.Id).ToList();

            //var garmentFinishingInItemDto = _garmentFinishingInItemRepository.Find(_garmentFinishingInItemRepository.Query).Select(o => new GarmentFinishingInItemDto(o)).ToList();
            var garmentFinishingInItemDto = _garmentFinishingInItemRepository.Query
                .Where(w => finishingInIds.Contains(w.FinishingInId))
                .Select(o => new GarmentFinishingInItemDto(o))
                .OrderBy(o => o.Id).ToList();
            
            Parallel.ForEach(garmentFinishingInDto, itemDto =>
            {
                var garmentFinishingInItems = garmentFinishingInItemDto.Where(x => x.FinishingInId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentFinishingInItems;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentFinishingInDto = QueryHelper<GarmentFinishingInDto>.Order(garmentFinishingInDto.AsQueryable(), OrderDictionary).ToList();
            }

            await Task.Yield();
            return Ok(garmentFinishingInDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentFinishingInCommand command)
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

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetLoaderByRO(string keyword, string filter = "{}")
        {
            var query = _garmentFinishingInRepository.Read(1, int.MaxValue, "{}", "", filter);
            query = query.Where(o => o.RONo.Contains(keyword));

            var rOs = _garmentFinishingInRepository.Find(query)
                .Select(o => new { o.RONo, o.Article, o.ComodityCode, o.ComodityId, o.ComodityName }).Distinct().ToList();

            await Task.Yield();

            return Ok(rOs);
        }
    }
}
