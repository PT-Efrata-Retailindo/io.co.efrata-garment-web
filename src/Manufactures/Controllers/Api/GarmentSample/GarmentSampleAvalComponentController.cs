using Barebone.Controllers;
using Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetAllGarmentSampleAvalComponents;
using Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetGarmentSampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-aval-components")]
    public class GarmentSampleAvalComponentController : ControllerApiBase
    {
        public GarmentSampleAvalComponentController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            GetAllGarmentSampleAvalComponentsQuery query = new GetAllGarmentSampleAvalComponentsQuery(page, size, order, keyword, filter);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.GarmentSampleAvalComponents, info: new
            {
                page,
                size,
                count = viewModel.GarmentSampleAvalComponents.Count,
                viewModel.total
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GetGarmentSampleAvalComponentQuery query = new GetGarmentSampleAvalComponentQuery(guid);
            var data = await Mediator.Send(query);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleAvalComponentCommand command)
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

            RemoveGarmentSampleAvalComponentCommand command = new RemoveGarmentSampleAvalComponentCommand(guid);
            var data = await Mediator.Send(command);

            return Ok(data.Identity);
        }

        [HttpPut("update-received")]
        public async Task<IActionResult> UpdateIsReceived([FromBody] UpdateIsReceivedGarmentSampleAvalComponentCommand command)
        {
            VerifyUser();
            var order = await Mediator.Send(command);

            return Ok();
        }
    }
}
