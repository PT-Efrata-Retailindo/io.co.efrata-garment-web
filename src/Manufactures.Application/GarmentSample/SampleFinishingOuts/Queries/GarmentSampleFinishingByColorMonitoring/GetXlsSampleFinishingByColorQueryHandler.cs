using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries.GarmentSampleFinishingByColorMonitoring
{
    public class GetXlsSampleFinishingByColorQueryHandler : IQueryHandler<GetXlsSampleFinishingByColorQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingOutRepository garmentSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository garmentSewingOutItemRepository;
        private readonly IGarmentSampleFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentSamplePreparingRepository garmentPreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentSampleCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository garmentCuttingInDetailRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository GarmentSampleRequestProductRepository;
        public GetXlsSampleFinishingByColorQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
        }
        
        class monitoringView
        {
            public string roJob { get; internal set; }
            public string article { get; internal set; }
            public string buyerCode { get; internal set; }
            public double qtyOrder { get; internal set; }
            public double stock { get; internal set; }
            public string style { get; internal set; }
            public double sewingQtyPcs { get; internal set; }
            public double finishingQtyPcs { get; internal set; }
            public string uomUnit { get; internal set; }
            public double remainQty { get; internal set; }
            public decimal price { get; internal set; }
			public string color { get; internal set; }
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
			public double AvgFC { get; set; }
		}
        public async Task<MemoryStream> Handle(GetXlsSampleFinishingByColorQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            dateTo = dateTo.AddHours(7);
            

            var _unitName = (from a in garmentFinishingOutRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();
			var sumbasicPrice = (from a in (from prep in garmentPreparingRepository.Query
											select new { prep.RONo, prep.Identity, prep.UnitId })
								 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId

								 select new { a.RONo, b.BasicPrice })
					   .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
					   {
						   RO = key.RONo,
						   BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
						   Count = group.Count()

					   });

			var sumFCs = (from a in garmentCuttingInRepository.Query
                          where a.CuttingType == "Main Fabric" &&
                          a.CuttingInDate <= dateTo
                          join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new { a.FC, a.RONo, FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC), c.CuttingInQuantity })
                       .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                       {
                           RO = key.RONo,
                           FC = group.Sum(s => (s.FCs)),
                           Count = group.Sum(s => s.CuttingInQuantity),
						   AvgFC = group.Sum(s => (s.FCs)) / group.Sum(s => s.CuttingInQuantity)
					   });
			GarmentSampleFinishingByColorMonitoringListViewModel listViewModel = new GarmentSampleFinishingByColorMonitoringListViewModel();
            List<GarmentSampleFinishingByColorMonitoringDto> monitoringDtos = new List<GarmentSampleFinishingByColorMonitoringDto>();

            var QueryFinishing = from a in (from aa in garmentFinishingOutRepository.Query
                                            where aa.UnitId == request.unit && aa.FinishingOutDate <= dateTo 
                                            select new { aa.Identity, aa.FinishingOutDate, aa.RONo, aa.Article })
                                 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                 select new monitoringView { color = b.Color, price = 0, finishingQtyPcs = a.FinishingOutDate >= dateFrom ? b.Quantity : 0, sewingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.FinishingOutDate < dateFrom ? -b.Quantity : 0, roJob = a.RONo, article = a.Article };
            var QuerySewingOut = from a in (from aa in garmentSewingOutRepository.Query
                                            where aa.UnitId == request.unit && aa.SewingOutDate <= dateTo && aa.SewingTo == "FINISHING"

                                            select new { aa.Identity, aa.SewingOutDate, aa.RONo, aa.Article })
                                 join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SampleSewingOutId
                                 select new monitoringView { color = b.Color, price = 0, finishingQtyPcs = 0, sewingQtyPcs = a.SewingOutDate >= dateFrom ? b.Quantity : 0, uomUnit = "PCS", remainQty = 0, stock = a.SewingOutDate < dateFrom ? b.Quantity : 0, roJob = a.RONo, article = a.Article };
            var queryNow = QuerySewingOut.Union(QueryFinishing);
            var querySum = queryNow.ToList().GroupBy(x => new { x.roJob, x.article, x.uomUnit ,x.color}, (key, group) => new
            {

                RoJob = key.roJob,
				color = key.color,
                Stock = group.Sum(s => s.stock),
                UomUnit = key.uomUnit,
                Article = key.article,
                SewingQtyPcs = group.Sum(s => s.sewingQtyPcs),
                Finishing = group.Sum(s => s.finishingQtyPcs)
            }).OrderBy(s => s.RoJob);
			var querySumTotal = from p in querySum
								group p by 1 into g
								select new GarmentSampleFinishingByColorMonitoringDto
								{
									color = "",
									roJob = "TOTAL",
									uomUnit = "",
									article = "",
									stock = g.Sum(s => s.Stock),
									sewingOutQtyPcs = g.Sum(s => s.SewingQtyPcs),
									finishingOutQtyPcs = g.Sum(s => s.Finishing)
								};
			foreach (var item in querySum)
            {
				GarmentSampleFinishingByColorMonitoringDto dto = new GarmentSampleFinishingByColorMonitoringDto
				{
                    roJob = item.RoJob,
                    article = item.Article,
                    uomUnit = item.UomUnit,
					color= item.color,
                    sewingOutQtyPcs = item.SewingQtyPcs,
                    finishingOutQtyPcs = item.Finishing,
                    stock = item.Stock,
					remainQty = item.Stock + item.SewingQtyPcs - item.Finishing
                };
                monitoringDtos.Add(dto);
            }
			foreach (var item in querySumTotal)
			{
				GarmentSampleFinishingByColorMonitoringDto dto = new GarmentSampleFinishingByColorMonitoringDto
				{
					roJob = item.roJob,
					article = item.article,
					uomUnit = item.uomUnit,
					color = item.color,
					sewingOutQtyPcs = item.sewingOutQtyPcs,
					finishingOutQtyPcs = item.finishingOutQtyPcs,
					stock = item.stock,
					remainQty = item.stock + item.sewingOutQtyPcs - item.finishingOutQtyPcs
				};
				monitoringDtos.Add(dto);
			}

			listViewModel.garmentMonitorings = monitoringDtos;
            var data = from a in monitoringDtos
                       where a.stock > 0 || a.sewingOutQtyPcs > 0 || a.finishingOutQtyPcs > 0 || a.remainQty > 0
                       select a;
            var roList = (from a in data
                          select a.roJob).Distinct().ToList();

            var sample = from s in GarmentSampleRequestRepository.Query
                         select new
                         {
                             s.RONoSample,
                             s.ComodityName,
                             s.BuyerCode,
                             Quantity = GarmentSampleRequestProductRepository.Query.Where(p => s.Identity == p.SampleRequestId).Sum(a => a.Quantity)
                         };
 

            foreach (var garment in data)
            {
                garment.buyerCode = garment.buyerCode == null ? (from sr in sample where sr.RONoSample == garment.roJob select sr.BuyerCode).FirstOrDefault() : garment.buyerCode;
                garment.style = garment.style == null ? (from sr in sample where sr.RONoSample == garment.roJob select sr.ComodityName).FirstOrDefault() : garment.style;
				garment.price =  Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDouble((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault());
				garment.nominal = Math.Round((Convert.ToDouble(garment.stock + garment.sewingOutQtyPcs - garment.finishingOutQtyPcs)) * garment.price, 2);
				garment.qtyOrder = (from sr in sample where sr.RONoSample == garment.roJob select sr.Quantity).FirstOrDefault();

			}
			monitoringDtos = data.ToList();
             
            listViewModel.garmentMonitorings = monitoringDtos;
            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
           
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
			reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Color", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Masuk", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Keluar", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) }); 
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });
            int counter = 5;
			int idx = 1;
			var rCount = 0;
			Dictionary<string, string> Rowcount = new Dictionary<string, string>();

			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings )
				{
					idx++;
					if (!Rowcount.ContainsKey(report.roJob))
					{
						rCount = 0;
						var index = idx;
						Rowcount.Add(report.roJob, index.ToString());
					}
					else
					{
						rCount += 1;
						Rowcount[report.roJob] = Rowcount[report.roJob] + "-" + rCount.ToString();
						var val = Rowcount[report.roJob].Split("-");
						if ((val).Length > 0)
						{
							Rowcount[report.roJob] = val[0] + "-" + rCount.ToString();
						}
					}

					reportDataTable.Rows.Add(report.roJob, report.article, report.qtyOrder, report.style, report.color, report.stock, report.sewingOutQtyPcs, report.finishingOutQtyPcs, report.remainQty, report.uomUnit);
					counter++;
				}
			}
			using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A" + 5 + ":J" + 5 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "Report Finishing By Color "; worksheet.Cells["A" + 1 + ":J" + 1 + ""].Merge = true;
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
                worksheet.Cells["A" + 1 + ":J" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":J" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":J" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":J" + 3 + ""].Style.Font.Size = 15;
                worksheet.Cells["A" + 1 + ":J" + 5 + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 1 + ":J" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 1 + ":J" + 5 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells["D" + 2 + ":D" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["D" + 2 + ":D" + counter + ""].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells["F" + 6 + ":k" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["F" + 6 + ":k" + counter + ""].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells["A" + 5 + ":J" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":J" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":J" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":J" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["F" + (counter) + ":J" + (counter) + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 1 + ":J" + 1 + ""].Style.Font.Bold = true;

				if (worksheet.Cells["A" + (counter)].Value.ToString() == "TOTAL")
				{
					worksheet.Cells["A" + (counter) + ":J" + (counter) + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + (counter) + ":E" + (counter) + ""].Merge = true;
				}
				worksheet.Cells["A" + 5 + ":J" + (counter) + ""].AutoFitColumns();

				var stream = new MemoryStream();

				foreach (var rowMerge in Rowcount)
				{
					var UnitrowNum = rowMerge.Value.Split("-");
					int rowNum2 = 1;
					int rowNum1 = Convert.ToInt32(UnitrowNum[0]);
					if (UnitrowNum.Length > 1 && UnitrowNum.Length < Rowcount.Count() )
					{
						rowNum2 = Convert.ToInt32(rowNum1) + Convert.ToInt32(UnitrowNum[1]);
					

					worksheet.Cells[$"A{(rowNum1 + 4)}:A{(rowNum2 + 4)}"].Merge = true;
					worksheet.Cells[$"A{(rowNum1 + 4)}:A{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
					worksheet.Cells[$"A{(rowNum1 + 4)}:A{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

					worksheet.Cells[$"B{(rowNum1 + 4)}:B{(rowNum2 + 4)}"].Merge = true;
					worksheet.Cells[$"B{(rowNum1 + 4)}:B{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
					worksheet.Cells[$"B{(rowNum1 + 4)}:B{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

					worksheet.Cells[$"C{(rowNum1 + 4)}:C{(rowNum2 + 4)}"].Merge = true;
					worksheet.Cells[$"C{(rowNum1 + 4)}:C{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
					worksheet.Cells[$"C{(rowNum1 + 4)}:C{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

					worksheet.Cells[$"D{(rowNum1 + 4)}:D{(rowNum2 + 4)}"].Merge = true;
					worksheet.Cells[$"D{(rowNum1 + 4)}:D{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
					worksheet.Cells[$"D{(rowNum1 + 4)}:D{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

					

					}

				}

				package.SaveAs(stream);

                return stream;
            }
        }
    }
}