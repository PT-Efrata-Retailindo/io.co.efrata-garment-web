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

namespace Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries
{
    public class GetXlsSampleFinishingQueryHandler : IQueryHandler<GetXlsSampleFinishingQuery, MemoryStream>
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
        private readonly IGarmentSampleFinishingMonitoringReportRepository garmentMonitoringFinishingReportRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository GarmentSampleRequestProductRepository;
        public GetXlsSampleFinishingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
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
            garmentMonitoringFinishingReportRepository = storage.GetRepository<IGarmentSampleFinishingMonitoringReportRepository>();
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
        public async Task<MemoryStream> Handle(GetXlsSampleFinishingQuery request, CancellationToken cancellationToken)
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
            GarmentSampleFinishingMonitoringListViewModel listViewModel = new GarmentSampleFinishingMonitoringListViewModel();
            List<GarmentSampleFinishingMonitoringDto> monitoringDtos = new List<GarmentSampleFinishingMonitoringDto>();

            var QueryFinishing = from a in (from aa in garmentFinishingOutRepository.Query
                                            where aa.UnitId == request.unit && aa.FinishingOutDate <= dateTo 
                                            select new { aa.Identity, aa.FinishingOutDate, aa.RONo, aa.Article })
                                 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                 select new monitoringView { price = 0, finishingQtyPcs = a.FinishingOutDate >= dateFrom ? b.Quantity : 0, sewingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.FinishingOutDate < dateFrom ? -b.Quantity : 0, roJob = a.RONo, article = a.Article };
            var QuerySewingOut = from a in (from aa in garmentSewingOutRepository.Query
                                            where aa.UnitId == request.unit && aa.SewingOutDate <= dateTo && aa.SewingTo == "FINISHING"

                                            select new { aa.Identity, aa.SewingOutDate, aa.RONo, aa.Article })
                                 join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SampleSewingOutId
                                 select new monitoringView { price = 0, finishingQtyPcs = 0, sewingQtyPcs = a.SewingOutDate >= dateFrom ? b.Quantity : 0, uomUnit = "PCS", remainQty = 0, stock = a.SewingOutDate < dateFrom ? b.Quantity : 0, roJob = a.RONo, article = a.Article };
            var queryNow = QuerySewingOut.Union(QueryFinishing);
            var querySum = queryNow.ToList().GroupBy(x => new { x.roJob, x.article, x.uomUnit }, (key, group) => new
            {

                RoJob = key.roJob,
                Stock = group.Sum(s => s.stock),
                UomUnit = key.uomUnit,
                Article = key.article,
                SewingQtyPcs = group.Sum(s => s.sewingQtyPcs),
                Finishing = group.Sum(s => s.finishingQtyPcs)
            }).OrderBy(s => s.RoJob);
            foreach (var item in querySum)
            {
                GarmentSampleFinishingMonitoringDto dto = new GarmentSampleFinishingMonitoringDto
                {
                    roJob = item.RoJob,
                    article = item.Article,
                    uomUnit = item.UomUnit,
                    sewingOutQtyPcs = item.SewingQtyPcs,
                    finishingOutQtyPcs = item.Finishing,
                    stock = item.Stock,
					remainQty = item.Stock + item.SewingQtyPcs - item.Finishing
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

            double stocks = 0;
            double finishing = 0;
            double sewingOutQtyPcs = 0;
            decimal nominals = 0;

            foreach (var garment in data)
            {
                garment.buyerCode = garment.buyerCode == null ? (from sr in sample where sr.RONoSample == garment.roJob select sr.BuyerCode).FirstOrDefault() : garment.buyerCode;
                garment.style = garment.style == null ? (from sr in sample where sr.RONoSample == garment.roJob select sr.ComodityName).FirstOrDefault() : garment.style;
				garment.price =  Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDouble((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault());
				garment.nominal = Math.Round((Convert.ToDouble(garment.stock + garment.sewingOutQtyPcs - garment.finishingOutQtyPcs)) * garment.price, 2);
				garment.qtyOrder = (from sr in sample where sr.RONoSample == garment.roJob select sr.Quantity).FirstOrDefault();
                stocks += garment.stock;
                finishing += garment.finishingOutQtyPcs;
                sewingOutQtyPcs += garment.sewingOutQtyPcs;
				nominals += Math.Round((Convert.ToDecimal(garment.stock + garment.sewingOutQtyPcs - garment.finishingOutQtyPcs)) * Convert.ToDecimal(garment.price), 2);
			}
			monitoringDtos = data.ToList();
            GarmentSampleFinishingMonitoringDto dtos = new GarmentSampleFinishingMonitoringDto
            {
                roJob = "",
                article = "",
                buyerCode = "",
                uomUnit = "",
                qtyOrder = 0,
                sewingOutQtyPcs = sewingOutQtyPcs,
                finishingOutQtyPcs = finishing,
                stock = stocks,
                style = "",
                price = 0,
                remainQty = stocks + sewingOutQtyPcs - finishing,
                nominal = Convert.ToDouble( nominals)

            };
            monitoringDtos.Add(dtos);
            listViewModel.garmentMonitorings = monitoringDtos;
            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Buyer", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Harga(M)", DataType = typeof(decimal) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Masuk", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Keluar", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nominal Sisa", DataType = typeof(decimal) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });
            int counter = 5;
            if (listViewModel.garmentMonitorings.Count > 0)
            {
                foreach (var report in listViewModel.garmentMonitorings)
                {
                    reportDataTable.Rows.Add(report.roJob, report.article, report.buyerCode, report.qtyOrder, report.style,report.price, report.stock, report.sewingOutQtyPcs, report.finishingOutQtyPcs, report.remainQty,report.nominal, report.uomUnit);
                    counter++;
                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A" + 5 + ":L" + 5 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "Report Finishing "; worksheet.Cells["A" + 1 + ":L" + 1 + ""].Merge = true;
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
                worksheet.Cells["A" + 1 + ":L" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":L" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":L" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":L" + 3 + ""].Style.Font.Size = 15;
                worksheet.Cells["A" + 1 + ":L" + 5 + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 1 + ":L" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 1 + ":L" + 5 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells["D" + 2 + ":D" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["D" + 2 + ":D" + counter + ""].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells["F" + 6 + ":k" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["F" + 6 + ":k" + counter + ""].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["F" + (counter) + ":L" + (counter) + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 1 + ":L" + 1 + ""].Style.Font.Bold = true;

                var stream = new MemoryStream();
                if (request.type != "bookkeeping")
                {
                    worksheet.Cells["A" + (counter) + ":E" + (counter) + ""].Merge = true;
                    worksheet.Column(3).Hidden = true;
                    worksheet.Column(6).Hidden = true;
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