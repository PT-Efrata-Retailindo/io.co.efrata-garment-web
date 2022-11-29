using Manufactures.Domain.GarmentScrapSources.Commands;
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
	[Route("scrap-sources")]
	public class GarmentScrapSourceController : Barebone.Controllers.ControllerApiBase
	{
		private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;

		public GarmentScrapSourceController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentScrapSourceRepository = Storage.GetRepository<IGarmentScrapSourceRepository>();
		}

		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapSourceRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapSourceDto> listDtos = _garmentScrapSourceRepository
				.Find(query)
				.Select(data => new GarmentScrapSourceDto(data))
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

            GarmentScrapSourceDto listDto = _garmentScrapSourceRepository.Find(o => o.Identity == guid).Select(data => new GarmentScrapSourceDto(data)).FirstOrDefault();

            await Task.Yield();
            return Ok(listDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentScrapSourceCommand command)
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

            RemoveGarmentScrapSourceCommand command = new RemoveGarmentScrapSourceCommand(guid);
            var data = await Mediator.Send(command);

            return Ok(data.Identity);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentScrapSourceCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

    }
}
