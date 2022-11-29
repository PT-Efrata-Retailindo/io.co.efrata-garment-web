using Manufactures.Domain.GarmentScrapDestinations.Commands;
using Manufactures.Domain.GarmentScrapDestinations.Repositories;
using Manufactures.Domain.GarmentScrapSources.Repositories;
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
    [Route("scrap-destinations")]
    public class GarmentScrapDestinationController : Barebone.Controllers.ControllerApiBase
    {
        private readonly IGarmentScrapDestinationRepository _garmentScrapDestinationRepository;
        public GarmentScrapDestinationController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentScrapDestinationRepository = Storage.GetRepository<IGarmentScrapDestinationRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();
            var query = _garmentScrapDestinationRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            List<GarmentScrapDestinationDto> listDtos = _garmentScrapDestinationRepository
                .Find(query)
                .Select(data => new GarmentScrapDestinationDto(data))
                .ToList();

            await Task.Yield();
            return Ok(listDtos, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);
            VerifyUser();

            GarmentScrapDestinationDto listDto = _garmentScrapDestinationRepository.Find(o => o.Identity == guid).Select(data => new GarmentScrapDestinationDto(data)).FirstOrDefault();

            await Task.Yield();
            return Ok(listDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentScrapDestinationCommand command)
        {
            VerifyUser();

            var data = await Mediator.Send(command);

            return Ok(data.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentScrapDestinationCommand command = new RemoveGarmentScrapDestinationCommand(guid);
            var data = await Mediator.Send(command);

            return Ok(data.Identity);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentScrapDestinationCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }
    }
}
