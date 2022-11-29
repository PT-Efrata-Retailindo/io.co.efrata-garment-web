using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
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
	[Route("scrap-classifications")]
	public class GarmentScrapClassificationController : Barebone.Controllers.ControllerApiBase
	{
		private readonly IGarmentScrapClassificationRepository _garmentScrapClassificationRepository;
		
		public GarmentScrapClassificationController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentScrapClassificationRepository = Storage.GetRepository<IGarmentScrapClassificationRepository>();
			
		}

		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapClassificationRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapClassificationListDto> listDtos = _garmentScrapClassificationRepository
				.Find(query)
				.Select(data => new GarmentScrapClassificationListDto(data))
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

			GarmentScrapClassificationListDto  listDto = _garmentScrapClassificationRepository.Find(o => o.Identity == guid).Select(data => new GarmentScrapClassificationListDto(data)).FirstOrDefault();

			await Task.Yield();
			return Ok(listDto);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PlaceGarmentScrapClassificationCommand command)
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

			RemoveGarmentScrapClassificationCommand command = new RemoveGarmentScrapClassificationCommand(guid);
			var data = await Mediator.Send(command);

			return Ok(data.Identity);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentScrapClassificationCommand command)
		{
			Guid guid = Guid.Parse(id);

			command.SetIdentity(guid);

			VerifyUser();

			var order = await Mediator.Send(command);

			return Ok(order.Identity);
		}

	}
}
