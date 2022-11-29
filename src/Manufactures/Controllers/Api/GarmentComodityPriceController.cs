using Barebone.Controllers;
using Manufactures.Domain.GarmentComodityPrices.Commands;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("comodity-prices")]
    public class GarmentComodityPriceController : ControllerApiBase
    {
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public GarmentComodityPriceController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentComodityPriceRepository = Storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentComodityPriceRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentComodityPriceDto> garmentComodityPriceListDtos = _garmentComodityPriceRepository
                .Find(query)
                .Select(price => new GarmentComodityPriceDto(price))
                .ToList();

            var dtoIds = garmentComodityPriceListDtos.Select(s => s.Id).ToList();

            await Task.Yield();
            return Ok(garmentComodityPriceListDtos, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentComodityPriceCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok(order);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] UpdateGarmentComodityPriceCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok(order);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
