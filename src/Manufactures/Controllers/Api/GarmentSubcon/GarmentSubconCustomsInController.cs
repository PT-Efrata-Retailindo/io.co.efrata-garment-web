using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Manufactures.Dtos.GarmentSubcon.GarmentSubconCustomsIns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-customs-ins")]
    public class GarmentSubconCustomsInController : ControllerApiBase
    {
        private readonly IGarmentSubconCustomsInRepository _garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository _garmentSubconCustomsInItemRepository;

        public GarmentSubconCustomsInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSubconCustomsInRepository = Storage.GetRepository<IGarmentSubconCustomsInRepository>();
            _garmentSubconCustomsInItemRepository = Storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconCustomsInRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            decimal totalQty = query.Sum(a => a.GarmentSubconCustomsInItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSubconCustomsInListDto> garmentSubconCustomsInListDtos = _garmentSubconCustomsInRepository
                .Find(query)
                .Select(SubconCustomsIn => new GarmentSubconCustomsInListDto(SubconCustomsIn))
                .ToList();

            var dtoIds = garmentSubconCustomsInListDtos.Select(s => s.Id).ToList();
            var items = _garmentSubconCustomsInItemRepository.Query
                .Where(o => dtoIds.Contains(o.SubconCustomsInId))
                .Select(s => new { s.Identity, s.SubconCustomsInId })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();

            await Task.Yield();
            return Ok(garmentSubconCustomsInListDtos, info: new
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

            GarmentSubconCustomsInDto garmentSubconCustomsInDto = _garmentSubconCustomsInRepository.Find(o => o.Identity == guid).Select(subconCustomsIn => new GarmentSubconCustomsInDto(subconCustomsIn)
            {
                Items = _garmentSubconCustomsInItemRepository.Find(o => o.SubconCustomsInId == subconCustomsIn.Identity).Select(subconCustomsInItem => new GarmentSubconCustomsInItemDto(subconCustomsInItem)
                { }).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSubconCustomsInDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconCustomsInCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconCustomsInCommand command)
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

            RemoveGarmentSubconCustomsInCommand command = new RemoveGarmentSubconCustomsInCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconCustomsInRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSubconCustomsInDto = _garmentSubconCustomsInRepository.Find(query).Select(o => new GarmentSubconCustomsInDto(o)).ToArray();
            var garmentSubconCustomsInItemDto = _garmentSubconCustomsInItemRepository.Find(_garmentSubconCustomsInItemRepository.Query).Select(o => new GarmentSubconCustomsInItemDto(o)).ToList();

            Parallel.ForEach(garmentSubconCustomsInDto, itemDto =>
            {
                var garmentSubconCustomsInItems = garmentSubconCustomsInItemDto.Where(x => x.SubconCustomsInId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentSubconCustomsInItems;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSubconCustomsInDto = QueryHelper<GarmentSubconCustomsInDto>.Order(garmentSubconCustomsInDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSubconCustomsInDto, info: new
            {
                page,
                size,
                count
            });
        }
    }
}
