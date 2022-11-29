using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Commands;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentReturGoodReturns.Commands;
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
    [Route("expenditure-good-returns")]
    public class GarmentExpenditureGoodReturnController : ControllerApiBase
    {
        private readonly IGarmentExpenditureGoodReturnRepository _garmentExpenditureGoodReturnRepository;
        private readonly IGarmentExpenditureGoodReturnItemRepository _garmentExpenditureGoodReturnItemRepository;

        public GarmentExpenditureGoodReturnController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentExpenditureGoodReturnRepository = Storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
            _garmentExpenditureGoodReturnItemRepository = Storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodReturnRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.Items.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentExpenditureGoodReturnListDto> garmentExpenditureGoodReturnListDtos = _garmentExpenditureGoodReturnRepository
                .Find(query)
                .Select(retur => new GarmentExpenditureGoodReturnListDto(retur))
                .ToList();

            var dtoIds = garmentExpenditureGoodReturnListDtos.Select(s => s.Id).ToList();
            var items = _garmentExpenditureGoodReturnItemRepository.Query
                .Where(o => dtoIds.Contains(o.ReturId))
                .Select(s => new { s.Identity, s.ReturId, s.Quantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(garmentExpenditureGoodReturnListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ReturId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentExpenditureGoodReturnListDtos, info: new
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

            GarmentExpenditureGoodReturnDto garmentExpenditureGoodReturnDto = _garmentExpenditureGoodReturnRepository.Find(o => o.Identity == guid).Select(retur => new GarmentExpenditureGoodReturnDto(retur)
            {
                Items = _garmentExpenditureGoodReturnItemRepository.Find(o => o.ReturId == retur.Identity).OrderBy(i=>i.Description).ThenBy(i => i.SizeName).Select(returItem => new GarmentExpenditureGoodReturnItemDto(returItem)
                {
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentExpenditureGoodReturnDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentExpenditureGoodReturnCommand command)
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
        //public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentExpenditureGoodReturnCommand command)
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

            RemoveGarmentExpenditureGoodReturnCommand command = new RemoveGarmentExpenditureGoodReturnCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodReturnRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentExpenditureGoodDto = _garmentExpenditureGoodReturnRepository.Find(query).Select(o => new GarmentExpenditureGoodReturnDto(o)).ToArray();
            var garmentExpenditureGoodItemDto = _garmentExpenditureGoodReturnItemRepository.Find(_garmentExpenditureGoodReturnItemRepository.Query).Select(o => new GarmentExpenditureGoodReturnItemDto(o)).ToList();

            Parallel.ForEach(garmentExpenditureGoodDto, itemDto =>
            {
                var garmentExpenditureGoodItems = garmentExpenditureGoodItemDto.Where(x => x.ExpenditureGoodId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentExpenditureGoodItems;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentExpenditureGoodDto = QueryHelper<GarmentExpenditureGoodReturnDto>.Order(garmentExpenditureGoodDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto, info: new
            {
                page,
                size,
                count
            });
        }
    }
}