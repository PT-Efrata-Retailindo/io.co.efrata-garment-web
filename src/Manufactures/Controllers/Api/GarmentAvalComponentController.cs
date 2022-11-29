using Barebone.Controllers;
using Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.Queries.GetGarmentAvalComponents;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("aval-components")]
    public class GarmentAvalComponentController : ControllerApiBase
    {
        public GarmentAvalComponentController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            GetAllGarmentAvalComponentsQuery query = new GetAllGarmentAvalComponentsQuery(page, size, order, keyword, filter);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.GarmentAvalComponents, info : new
            {
                page,
                size,
                count = viewModel.GarmentAvalComponents.Count,
                viewModel.total
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GetGarmentAvalComponentQuery query = new GetGarmentAvalComponentQuery(guid);
            var data = await Mediator.Send(query);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentAvalComponentCommand command)
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

            RemoveGarmentAvalComponentCommand command = new RemoveGarmentAvalComponentCommand(guid);
            var data = await Mediator.Send(command);

            return Ok(data.Identity);
        }

        [HttpPut("update-received")]
        public async Task<IActionResult> UpdateIsReceived([FromBody] UpdateIsReceivedGarmentAvalComponentCommand command)
        {
            VerifyUser();
            var order = await Mediator.Send(command);

            return Ok();
        }
    }
}
