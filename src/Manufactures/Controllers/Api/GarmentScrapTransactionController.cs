using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.TCKecil;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.SampahSapuan;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
	[ApiController]
	[Authorize]
	[Route("scrap-transactions")]
	public class GarmentScrapTransactionController : Barebone.Controllers.ControllerApiBase
	{
		private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;
		private readonly IGarmentScrapTransactionItemRepository _garmentScrapTransactionItemRepository;

		public GarmentScrapTransactionController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			_garmentScrapTransactionRepository = Storage.GetRepository<IGarmentScrapTransactionRepository>();
			_garmentScrapTransactionItemRepository = Storage.GetRepository<IGarmentScrapTransactionItemRepository>();

		}

		[HttpGet]
		public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapTransactionRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapTransactionDto> listDtos = _garmentScrapTransactionRepository
				.Find(query)
				.Select(data => new GarmentScrapTransactionDto(data)).Where(s => s.TransactionType == "IN")
				.ToList();

			await Task.Yield();
			return Ok(listDtos, info: new
			{
				page,
				size,
				count
			});
		}
		[HttpGet("out")]
		public async Task<IActionResult> GetOut(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			VerifyUser();

			var query = _garmentScrapTransactionRepository.Read(page, size, order, keyword, filter);
			var count = query.Count();

			List<GarmentScrapTransactionDto> listDtos = _garmentScrapTransactionRepository
				.Find(query)
				.Select(data => new GarmentScrapTransactionDto(data)).Where(s=>s.TransactionType== "OUT")
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

			GarmentScrapTransactionDto garmentScrapTransactionDto = _garmentScrapTransactionRepository.Find(o => o.Identity == guid).Select(ScrapTransaction => new GarmentScrapTransactionDto(ScrapTransaction)
			{
				Items = _garmentScrapTransactionItemRepository.Find(o => o.ScrapTransactionId == ScrapTransaction.Identity).OrderBy(i=>i.ScrapClassificationName).Select(ScrapTransactionItem => new GarmentScrapTransactionItemDto(ScrapTransactionItem)
				).ToList()
			}
			).FirstOrDefault();

			await Task.Yield();
			return Ok(garmentScrapTransactionDto);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PlaceGarmentScrapTransactionCommand command)
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

			RemoveGarmentScrapTransactionCommand command = new RemoveGarmentScrapTransactionCommand(guid);
			var data = await Mediator.Send(command);

			return Ok(data.Identity);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentScrapTransactionCommand command)
		{
			Guid guid = Guid.Parse(id);

			command.SetIdentity(guid);

			VerifyUser();

			var order = await Mediator.Send(command);

			return Ok(order.Identity);
		}

        [HttpGet("mutation")]
        public async Task<IActionResult> GetMutation(DateTime dateFrom, DateTime dateTo)
        {
            VerifyUser();
            GetMutationScrapQuery query = new GetMutationScrapQuery(dateFrom, dateTo, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentMonitorings);
        }

		[HttpGet("tc_kecil_IN")]
		public async Task<IActionResult> GetTCKecilIN(DateTime dateFrom, DateTime dateTo)
		{
			VerifyUser();
			 TCKecil_In_Query query = new TCKecil_In_Query(dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.scrapDtos);
		}

		[HttpGet("tc_kecil_IN/download")]
		public async Task<IActionResult> GetXls(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				VerifyUser();
				GetXlsTCKecil_in_Query query = new GetXlsTCKecil_in_Query(dateFrom, dateTo, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Monitoring Pemasukan Aval TC Kecil ";

				if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

				if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
				filename += ".xlsx";

				xlsInBytes = xls.ToArray();
				var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
				return file;
			}
			catch (Exception e)
			{

				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpGet("tc_kecil_out")]
		public async Task<IActionResult> GetTCKecilout(DateTime dateFrom, DateTime dateTo)
		{
			VerifyUser();
			TCKecil_Out_Query query = new TCKecil_Out_Query(dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.scrapDtos);
		}

		[HttpGet("tc_kecil_out/download")]
		public async Task<IActionResult> GetXlsTCKecilOut(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				VerifyUser();
				GetXlsTCKecil_Out_Query query = new GetXlsTCKecil_Out_Query(dateFrom, dateTo, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Monitoring Pengeluaran Aval TC Kecil ";

				if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

				if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
				filename += ".xlsx";

				xlsInBytes = xls.ToArray();
				var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
				return file;
			}
			catch (Exception e)
			{

				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpGet("sapuan_IN")]
		public async Task<IActionResult> GetSapuanIN(DateTime dateFrom, DateTime dateTo)
		{
			VerifyUser();
			Sapuan_In_Query query = new Sapuan_In_Query(dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.scrapDtos);
		}

		[HttpGet("sapuan_IN/download")]
		public async Task<IActionResult> GetXlsSapuanIn(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				VerifyUser();
				GetXlsSapuan_in_Query query = new GetXlsSapuan_in_Query(dateFrom, dateTo, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Monitoring Pemasukan Aval Sampah Sapuan ";

				if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

				if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
				filename += ".xlsx";

				xlsInBytes = xls.ToArray();
				var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
				return file;
			}
			catch (Exception e)
			{

				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpGet("sapuan_out")]
		public async Task<IActionResult> GetSapuanOut(DateTime dateFrom, DateTime dateTo)
		{
			VerifyUser();
			Sapuan_Out_Query query = new Sapuan_Out_Query(dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.scrapDtos);
		}

		[HttpGet("sapuan_out/download")]
		public async Task<IActionResult> GetXlsSapuanOut(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				VerifyUser();
				GetXlsSapuan_Out_Query query = new GetXlsSapuan_Out_Query(dateFrom, dateTo, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Monitoring Pengeluaran Aval Sampah Sapuan ";

				if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");

				if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
				filename += ".xlsx";

				xlsInBytes = xls.ToArray();
				var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
				return file;
			}
			catch (Exception e)
			{

				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}
	}
}
