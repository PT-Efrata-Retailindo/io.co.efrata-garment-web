using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Dtos.GarmentSubcon;
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
    [Route("subcon-customs-outs")]
    public class GarmentSubconCustomsOutController : ControllerApiBase
    {
        private readonly IGarmentSubconCustomsOutRepository _garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository _garmentSubconCustomsOutItemRepository;

        public GarmentSubconCustomsOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSubconCustomsOutRepository = Storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            _garmentSubconCustomsOutItemRepository = Storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconCustomsOutRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSubconCustomsOutItem.Sum(b => b.Quantity));

            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSubconCustomsOutListDto> garmentSubconCustomsOutListDtos = _garmentSubconCustomsOutRepository
                .Find(query)
                .Select(subcon => new GarmentSubconCustomsOutListDto(subcon))
                .ToList();

            await Task.Yield();
            return Ok(garmentSubconCustomsOutListDtos, info: new
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

            GarmentSubconCustomsOutDto garmentSubconCustomsOutDto = _garmentSubconCustomsOutRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconCustomsOutDto(subcon)
            {
                Items = _garmentSubconCustomsOutItemRepository.Find(o => o.SubconCustomsOutId == subcon.Identity).Select(subconItem => new GarmentSubconCustomsOutItemDto(subconItem)
                {

                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSubconCustomsOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconCustomsOutCommand command)
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

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconCustomsOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSubconCustomsOutDto = _garmentSubconCustomsOutRepository.Find(query).Select(o => new GarmentSubconCustomsOutDto(o)).ToArray();
            var garmentSubconCustomsOutItemDto = _garmentSubconCustomsOutItemRepository.Find(_garmentSubconCustomsOutItemRepository.Query).Select(o => new GarmentSubconCustomsOutItemDto(o)).ToList();

            Parallel.ForEach(garmentSubconCustomsOutDto, itemDto =>
            {
                var garmentSubconCustomsOutItems = garmentSubconCustomsOutItemDto.Where(x => x.SubconCustomsOutId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentSubconCustomsOutItems;

            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSubconCustomsOutDto = QueryHelper<GarmentSubconCustomsOutDto>.Order(garmentSubconCustomsOutDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSubconCustomsOutDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconCustomsOutCommand command)
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
            var garmentSubconCustomsOut = _garmentSubconCustomsOutRepository.Find(x => x.Identity == guid).Select(o => new GarmentSubconCustomsOutDto(o)).FirstOrDefault();

            RemoveGarmentSubconCustomsOutCommand command = new RemoveGarmentSubconCustomsOutCommand(guid);
            var order = await Mediator.Send(command);
            return Ok(order.Identity);
        }
    }
}