using Barebone.Controllers;
using Manufactures.Domain.GarmentTradingFinishingIns.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("trading-finishing-ins")]
    public class GarmentTradingFinishingInController : ControllerApiBase
    {
        public GarmentTradingFinishingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentTradingFinishingInCommand command)
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

            RemoveGarmentTradingFinishingInCommand command = new RemoveGarmentTradingFinishingInCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

    }
}
