using Manufactures.Application.GarmentSample.GarmentMonitoringSampleFlows.Queries;
using Manufactures.Application.GarmentSample.GarmentMonitoringSampleStockFlows.Queries;
using Manufactures.Domain.GarmentSample.MonitoringSampleStockFlow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
	[ApiController]
	[Authorize]
	[Route("monitoringFlowSample")]
	public class GarmentMonitoringSampleFlowController : Barebone.Controllers.ControllerApiBase
	{
		private readonly IGarmentMonitoringSampleStockFlowRepository repository; 

		public GarmentMonitoringSampleFlowController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			repository = Storage.GetRepository<IGarmentMonitoringSampleStockFlowRepository>();
			 
		}

		[HttpGet("bySize")]
		public async Task<IActionResult> GetMonitoring(int unit, DateTime date,string ro, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringSampleFlowQuery query = new GetMonitoringSampleFlowQuery(page, size, Order, unit, date,ro, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.garmentMonitorings, info: new
			{
				page,
				size,
				viewModel.count
			});
		}
		[HttpGet("download")]
		public async Task<IActionResult> GetXls(int unit, DateTime date, string ro, int page = 1, int size = 25, string Order = "{}")
		{
			try
			{
				VerifyUser();
				GetXlsMonitoringSampleFlowQuery query = new GetXlsMonitoringSampleFlowQuery(page, size, Order, unit, date, ro, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Flow Sample per Size";

				if (date != null) filename += " " + ((DateTime)date).ToString("dd-MM-yyyy");

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
		[HttpGet("stocks")]
		public async Task<IActionResult> GetMonitoringSampleStockFlow(int unit, DateTime dateFrom, DateTime dateTo, string ro, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringSampleStockFlowQuery query = new GetMonitoringSampleStockFlowQuery(page, size, Order, unit, ro, dateFrom, dateTo, WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.garmentMonitorings, info: new
			{
				page,
				size,
				viewModel.count
			});
		}

		[HttpGet("stocksdownload")]
		public async Task<IActionResult> GetXlsMonitoringSampleStockFlow(string type, int unit, DateTime dateFrom, DateTime dateTo, string ro, int page = 1, int size = 25, string Order = "{}")
		{
			try
			{
				VerifyUser();
				GetXlsMonitoringSampleStockFlowQuery query = new GetXlsMonitoringSampleStockFlowQuery(page, size, Order, unit, ro, dateFrom, dateTo, type, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Flow Persediaan Sample";

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
