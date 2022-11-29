using Barebone.Controllers;
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
	[Route("scrap-stocks")]
	public class GarmentScrapStockController : ControllerApiBase
	{
		private readonly IGarmentScrapStockRepository _garmentScrapStockRepository;
	
		public GarmentScrapStockController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentScrapStockRepository = Storage.GetRepository<IGarmentScrapStockRepository>();
		}

		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapStockRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapStockDto> listDtos = _garmentScrapStockRepository
				.Find(query)
				.Where(s=>s.Quantity >0)
				.Select(data => new GarmentScrapStockDto(data))
				.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
			{
				page,
				size,
				count
			});
		}
		[HttpGet("remainingQty")]
		public async Task<IActionResult> GetRemaining(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapStockRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapStockDto> listDtos = _garmentScrapStockRepository
				.Find(query)
				.Select(data => new GarmentScrapStockDto(data))
				.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
			{
				page,
				size,
				count
			});
		}
	}
}
