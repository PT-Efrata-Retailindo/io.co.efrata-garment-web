using Manufactures.Application.GarmentMonitoringProductionFlows.Queries;
using Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries;
using Manufactures.Domain.MonitoringProductionFlow;
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
	[Route("monitoringFlowProduction")]
	public class GarmentMonitoringProductionFlowController : Barebone.Controllers.ControllerApiBase
	{
		private readonly IGarmentMonitoringProductionFlowRepository repository; 

		public GarmentMonitoringProductionFlowController(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			repository = Storage.GetRepository<IGarmentMonitoringProductionFlowRepository>();
			 
		}

		[HttpGet("bySize")]
		public async Task<IActionResult> GetMonitoring(int unit, DateTime date,string ro, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringProductionFlowQuery query = new GetMonitoringProductionFlowQuery(page, size, Order, unit, date,ro, WorkContext.Token);
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
				GetXlsMonitoringProductionFlowQuery query = new GetXlsMonitoringProductionFlowQuery(page, size, Order, unit, date, ro, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Flow Produksi per Size";

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
		public async Task<IActionResult> GetMonitoringProductionStockFlow(int unit, DateTime dateFrom,DateTime dateTo, string ro, int page = 1, int size = 25, string Order = "{}")
		{
			VerifyUser();
			GetMonitoringProductionStockFlowQuery query = new GetMonitoringProductionStockFlowQuery(page, size, Order, unit,ro, dateFrom,dateTo,WorkContext.Token);
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel.garmentMonitorings, info: new
			{
				page,
				size,
				viewModel.count
			});
		}

		[HttpGet("stocksdownload")]
		public async Task<IActionResult> GetXlsMonitoringProductionStockFlow(string type,int unit, DateTime dateFrom, DateTime dateTo, string ro, int page = 1, int size = 25, string Order = "{}")
		{
			try
			{
				VerifyUser();
				GetXlsMonitoringProductionStockFlowQuery query = new GetXlsMonitoringProductionStockFlowQuery(page, size, Order, unit, ro, dateFrom, dateTo,type, WorkContext.Token);
				byte[] xlsInBytes;

				var xls = await Mediator.Send(query);

				string filename = "Laporan Flow Persediaan";

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
