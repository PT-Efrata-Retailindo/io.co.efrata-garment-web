using Barebone.Controllers;
using Manufactures.Application.GarmentSubconCuttings.Queries.GetAllGarmentSubconCuttings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("subcon-cuttings")]
    public class GarmentSubconCuttingController : ControllerApiBase
    {
        public GarmentSubconCuttingController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            VerifyUser();

            GetAllGarmentSubconCuttingsQuery query = new GetAllGarmentSubconCuttingsQuery(page, size, order, keyword, filter);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.data, info: new
            {
                page,
                size,
                count = viewModel.data.Count,
                viewModel.total
            });
        }
    }
}
