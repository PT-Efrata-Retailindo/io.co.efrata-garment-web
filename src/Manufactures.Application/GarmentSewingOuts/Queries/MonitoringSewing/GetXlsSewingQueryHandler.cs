using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.IO;
using System.Data;
using OfficeOpenXml;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using OfficeOpenXml.Style;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System.Net.Http;
using System.Text;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.MonitoringProductionStockFlow;

namespace Manufactures.Application.GarmentSewingOuts.Queries.MonitoringSewing
{
	public class GetXlsSewingQueryHandler : IQueryHandler<GetXlsSewingQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentBalanceMonitoringProductionStockFlowRepository garmentBalanceSewingRepository;
        private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentSewingInRepository garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository garmentSewingInItemRepository;
        private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;
        private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;

        public GetXlsSewingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentBalanceSewingRepository = storage.GetRepository<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
            garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
            _http = serviceProvider.GetService<IHttpClientService>();
		}

       
        public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
        {
			CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
			var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/";

			var httpContent = new StringContent(JsonConvert.SerializeObject(listRO), Encoding.UTF8, "application/json");

			var httpResponse = await _http.SendAsync(HttpMethod.Get, costCalculationUri, token, httpContent);

			var freeRO = new List<string>();

			if (httpResponse.IsSuccessStatusCode)
			{
				var contentString = await httpResponse.Content.ReadAsStringAsync();
				Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
				var dataString = content.GetValueOrDefault("data").ToString();
				var listData = JsonConvert.DeserializeObject<List<CostCalViewModel>>(dataString);

				foreach (var item in ro)
				{
					var data = listData.SingleOrDefault(s => s.ro == item);
					if (data != null)
					{
						costCalculationGarmentDataProductionReport.data.Add(data);
					}
					else
					{
						freeRO.Add(item);
					}
				}
			}
			else
			{
				var err = await httpResponse.Content.ReadAsStringAsync();

			}

			return costCalculationGarmentDataProductionReport;
		}

        class monitoringView
		{
			public string roJob { get; internal set; }
			public string article { get; internal set; }
			public string buyerCode { get; internal set; }
			public double qtyOrder { get; internal set; }
            public double BeginingBalanceSewingQty { get; internal set; }
            public double QtySewingIn { get; internal set; }
            public double QtySewingOut { get; internal set; }
            public double QtySewingInTransfer { get; internal set; }
            public double WipSewingOut { get; internal set; }
            public double WipFinishingOut { get; internal set; }
            public double QtySewingRetur { get; internal set; }
            public double QtySewingAdj { get; internal set; }
            public double PriceSewingAdj { get; internal set; }
            public double EndBalanceSewingQty { get; internal set; }
            public double EndBalanceSewingPrice { get; internal set; }
            public decimal price { get; internal set; }
        }
		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal BasicPrice { get; internal set; }
			public int Count { get; internal set; }
		}
		class ViewFC
		{
			public string RO { get; internal set; }
			public double FC { get; internal set; }
			public int Count { get; internal set; }
		}
		public async Task<MemoryStream> Handle(GetXlsSewingQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
			dateFrom.AddHours(7);
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
			dateTo = dateTo.AddHours(7);
			DateTimeOffset dateBalance = (from a in garmentBalanceSewingRepository.Query.OrderByDescending(s => s.CreatedDate)
										  select a.CreatedDate).FirstOrDefault();
			var QueryRoSewingOut = (from a in garmentSewingOutRepository.Query
									where a.UnitId == request.unit && a.SewingOutDate <= dateTo
									select a.RONo).Distinct();
		
			var QueryRo = QueryRoSewingOut.Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			var _unitName = (from a in garmentSewingOutRepository.Query
							 where a.UnitId == request.unit
							 select a.UnitName).FirstOrDefault();

            var sumbasicPrice = (from a in (from prep in garmentPreparingRepository.Query
                                            select new { prep.RONo, prep.Identity })
                                 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId

                                 select new { a.RONo, b.BasicPrice })
                    .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                    {
                        RO = key.RONo,
                        BasicPrice = Math.Round(Convert.ToDecimal(group.Sum(s => s.BasicPrice)), 2),
                        Count = group.Count()
                    });
            var sumFCs = (from a in garmentCuttingInRepository.Query
                          where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && */ a.CuttingType == "Main Fabric" //&&
                                                                                                                                                                /*a.UnitId == request.unit && a.CuttingInDate <= dateTo*/
                          join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new { a.FC, a.RONo, FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC), c.CuttingInQuantity })
                       .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                       {
                           RO = key.RONo,
                           FC = group.Sum(s => (s.FCs)),
                           Count = group.Sum(s => s.CuttingInQuantity)
                       });
            var queryBalanceSewing = from a in
                                  (from aa in garmentBalanceSewingRepository.Query
                                   where aa.BeginingBalanceSewingQty > 0 && aa.UnitId == request.unit && aa.UnitId == aa.UnitId
                                   select new { aa.BeginingBalanceSewingQty, aa.CreatedDate, aa.UnitId, aa.Ro, aa.BuyerCode, aa.BasicPrice })

                                     where a.CreatedDate < dateFrom && a.UnitId == request.unit
                                     select new monitoringView
                                     {
                                         price = (decimal)a.BasicPrice,
                                         roJob = a.Ro,
                                         buyerCode = a.BuyerCode,
                                         BeginingBalanceSewingQty = a.BeginingBalanceSewingQty,
                                         QtySewingIn = 0,
                                         QtySewingOut = 0,
                                         QtySewingInTransfer = 0,
                                         WipSewingOut = 0,
                                         WipFinishingOut = 0,
                                         QtySewingRetur = 0,
                                         QtySewingAdj = 0
                                     };

            var QuerySewingOut = from a in (from aa in garmentSewingOutRepository.Query
                                            where aa.SewingOutDate >= dateBalance && aa.SewingOutDate <= dateTo
                                            select new { aa.RONo, aa.Identity, aa.SewingOutDate, aa.SewingTo, aa.UnitToId, aa.UnitId })
                                 join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId

                                 select new monitoringView
                                 {
                                     price = 0,
                                     roJob = a.RONo,
                                     BeginingBalanceSewingQty = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0 - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0) + ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0),
                                     QtySewingIn = 0,
                                     QtySewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                     QtySewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
                                     WipSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                     WipFinishingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                     QtySewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0,
                                     QtySewingAdj = 0
                                 };

            var QuerySewingIn = from a in (from aa in garmentSewingInRepository.Query
                                           where aa.SewingInDate >= dateBalance && aa.UnitId == request.unit && aa.SewingInDate <= dateTo
                                           select new { aa.RONo, aa.Identity, aa.SewingInDate, aa.SewingFrom })
                                join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
                                select new monitoringView
                                {
                                    price = 0,
                                    roJob = a.RONo,
                                    BeginingBalanceSewingQty = (a.SewingInDate < dateFrom && a.SewingInDate > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Quantity : 0,
                                    QtySewingIn = (a.SewingInDate >= dateFrom) && a.SewingFrom != "SEWING" ? b.Quantity : 0,
                                    QtySewingOut = 0,
                                    QtySewingInTransfer = 0,
                                    WipSewingOut = 0,
                                    WipFinishingOut = 0,
                                    QtySewingRetur = 0,
                                    QtySewingAdj = 0
                                };
            var QuerySewingAdj = from a in (from aa in garmentAdjustmentRepository.Query
                                            where aa.AdjustmentDate >= dateBalance && aa.UnitId == request.unit && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "SEWING"
                                            select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                 join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                 select new monitoringView
                                 {
                                     price = 0,
                                     roJob = a.RONo,
                                     BeginingBalanceSewingQty = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                     QtySewingIn = 0,
                                     QtySewingOut = 0,
                                     QtySewingInTransfer = 0,
                                     WipSewingOut = 0,
                                     WipFinishingOut = 0,
                                     QtySewingRetur = 0,
                                     QtySewingAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                 };
            var queryNow = queryBalanceSewing.Union(QuerySewingOut).Union(QuerySewingIn).Union(QuerySewingAdj);
            var querySum = queryNow.ToList().GroupBy(x => new { x.roJob }, (key, group) => new
            {

                RoJob = key.roJob,
                BeginingBalanceSewingQty = group.Sum(s => s.BeginingBalanceSewingQty),
                QtySewingIn = group.Sum(s => s.QtySewingIn),
                QtySewingInTransfer = group.Sum(s => s.QtySewingInTransfer),
                QtySewingOut = group.Sum(s => s.QtySewingOut),
                QtySewingAdj = group.Sum(s => s.QtySewingAdj),
                WipFinishingOut = group.Sum(s => s.WipFinishingOut),
                WipSewingOut = group.Sum(s => s.WipSewingOut),
                QtySewingRetur = group.Sum(s => s.QtySewingRetur),


            }).OrderBy(s => s.RoJob);
            GarmentMonitoringSewingListViewModel listViewModel = new GarmentMonitoringSewingListViewModel();
            List<GarmentMonitoringSewingDto> monitoringDtos = new List<GarmentMonitoringSewingDto>();
            foreach (var item in querySum)
            {
                GarmentMonitoringSewingDto dto = new GarmentMonitoringSewingDto
                {
                    roJob = item.RoJob,
                    beginingBalanceSewingQty = item.BeginingBalanceSewingQty,
                    qtySewingIn = item.QtySewingIn,
                    qtySewingInTransfer = item.QtySewingInTransfer,
                    qtySewingOut = item.QtySewingOut,
                    wipSewingOut = item.WipSewingOut,
                    wipFinishingOut = item.WipFinishingOut,
                    qtySewingRetur = item.QtySewingRetur,
                    qtySewingAdj = item.QtySewingAdj,
                    endBalanceSewingQty = Math.Round(item.BeginingBalanceSewingQty + item.QtySewingIn - item.QtySewingOut + item.QtySewingInTransfer - item.WipSewingOut - item.WipFinishingOut - item.QtySewingRetur - item.QtySewingAdj, 2)
                };
                monitoringDtos.Add(dto);
            }
            listViewModel.garmentMonitorings = monitoringDtos;
            var data = from a in monitoringDtos
                       where a.beginingBalanceSewingQty > 0 || a.qtySewingIn > 0 || a.qtySewingInTransfer > 0 || a.qtySewingOut > 0 || a.qtySewingRetur > 0 || a.qtySewingAdj > 0 || a.wipSewingOut > 0 || a.wipFinishingOut > 0
                       select a;
            var roList = (from a in data
                          select a.roJob).Distinct().ToList();
             
            CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(roList, request.token);

            

            foreach (var garment in data)
            {
                garment.buyerCode = garment.buyerCode == null ? (from cost in costCalculation.data where cost.ro == garment.roJob select cost.buyerCode).FirstOrDefault() : garment.buyerCode;
                garment.price = Math.Round(Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDecimal((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault()) == 0 ? Convert.ToDecimal((from a in queryBalanceSewing.ToList() where a.roJob == garment.roJob select a.price).FirstOrDefault()) : Math.Round(Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDecimal((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault());
                garment.endBalanceSewingPrice = Math.Round(garment.endBalanceSewingQty * (double)garment.price, 2);
                garment.qtyOrder = (from cost in costCalculation.data where cost.ro == garment.roJob select cost.qtyOrder).FirstOrDefault();
                garment.style = garment.style == null ? (from cost in costCalculation.data where cost.ro == garment.roJob select cost.comodityName).FirstOrDefault() : garment.style;
                garment.article = (from a in garmentPreparingRepository.Query where a.RONo == garment.roJob select a.Article).FirstOrDefault();

            }
            monitoringDtos = data.ToList();
            double beginingSewing = 0;
            double sewingIn = 0;
            double sewingIntrasnfer = 0;
            double sewingOut=0;
            double wipSewing=0;
            double wipFInishing=0;
            double sewingRetur=0;
            double sewingAdj=0;
            double endBalance = 0;
            double endBalancePrice = 0;
			 
			foreach (var item in data)
			{
                beginingSewing += item.beginingBalanceSewingQty;
                sewingIn += item.qtySewingIn;
                sewingOut += item.qtySewingOut;
                sewingIntrasnfer += item.qtySewingInTransfer;
                wipSewing += item.wipSewingOut;
                wipFInishing += item.wipFinishingOut;
                sewingRetur += item.qtySewingRetur;
                sewingAdj += item.qtySewingAdj;
                endBalance += item.endBalanceSewingQty;
                endBalancePrice += item.endBalanceSewingPrice;

            }
			GarmentMonitoringSewingDto dtos = new GarmentMonitoringSewingDto
			{
				roJob = "",
				article = "",
				buyerCode = "",
				style="",
                price=0,
                qtyOrder=0,
                beginingBalanceSewingQty=beginingSewing,
                qtySewingIn= sewingIn,
                qtySewingOut= sewingOut,
                qtySewingInTransfer= sewingIntrasnfer,
                wipSewingOut= wipSewing,
                wipFinishingOut= wipFInishing,
                qtySewingRetur= sewingRetur,
                qtySewingAdj= sewingAdj,
                endBalanceSewingQty= endBalance ,
                endBalanceSewingPrice=endBalancePrice
			};
			monitoringDtos.Add(dtos);
			listViewModel.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Buyer", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Harga (M)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Saldo Awal WIP Sewing", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sewing In (WIP Sewing)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sewing Out (WIP Finishing)", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sewing In Transfer", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sewing Out Tranfer WIP Sewing", DataType = typeof(double) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sewing Out Transfer WIP Finishing", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Retur ke Cutting", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Adjs Sewing", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir WIP Sewing", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nominal Saldo Akhir WIP Sewing", DataType = typeof(double) });

            int counter = 5;
			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings)
				{
                    reportDataTable.Rows.Add(report.roJob, report.article, report.buyerCode, report.qtyOrder, report.style, report.price, report.beginingBalanceSewingQty, report.qtySewingIn, report.qtySewingOut, report.qtySewingInTransfer, report.wipSewingOut, report.wipFinishingOut,report.qtySewingRetur, report.qtySewingAdj,report.endBalanceSewingQty,report.endBalanceSewingPrice);
                    counter++;
                }
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
				worksheet.Cells["D" + 6 + ":P" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["D" + 6 + ":P" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Cells["A" + 5 + ":P" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":P" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":P" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":P" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["F" + (counter) + ":P" + (counter) + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 5 + ":P" + 5 + ""].Style.Font.Bold = true;
				worksheet.Cells["A1"].Value = "Report Sewing "; worksheet.Cells["A" + 1 + ":P" + 1 + ""].Merge = true;
				worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
				worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
				worksheet.Cells["A" + 1 + ":P" + 1 + ""].Merge = true;
				worksheet.Cells["A" + 2 + ":P" + 2 + ""].Merge = true;
				worksheet.Cells["A" + 3 + ":P" + 3 + ""].Merge = true;
				worksheet.Cells["A" + 1 + ":P" + 3 + ""].Style.Font.Size = 15;
				worksheet.Cells["A" + 1 + ":P" + 5 + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 1 + ":P" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				worksheet.Cells["A" + 1 + ":P" + 2 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
				var stream = new MemoryStream();
				if (request.type != "bookkeeping")
				{
					worksheet.Cells["A" + (counter) + ":E" + (counter) + ""].Merge = true;
					worksheet.Column(3).Hidden = true;
					worksheet.Column(6).Hidden = true;
                    worksheet.Column(16).Hidden = true;
                }
				else
				{
					worksheet.Cells["A" + (counter) + ":F" + (counter) + ""].Merge = true;
				}
				package.SaveAs(stream);

				return stream;
			}
		}
	}
}
