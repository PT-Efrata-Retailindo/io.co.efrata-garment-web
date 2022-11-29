using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.Monitoring
{
    public class GetXlsSampleCuttingQueryHandler : IQueryHandler<GetXlsSampleCuttingQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingOutRepository GarmentSampleCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository GarmentSampleCuttingOutItemRepository;
        private readonly IGarmentSampleCuttingInRepository GarmentSampleCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository GarmentSampleCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository GarmentSampleCuttingInDetailRepository;
        private readonly IGarmentSampleAvalComponentRepository GarmentSampleAvalComponentRepository;
        private readonly IGarmentSampleAvalComponentItemRepository GarmentSampleAvalComponentItemRepository;
        private readonly IGarmentSamplePreparingRepository GarmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository GarmentSamplePreparingItemRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository GarmentSampleRequestProductRepository;

        public GetXlsSampleCuttingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            GarmentSampleCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            GarmentSampleCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            GarmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            GarmentSampleCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            GarmentSampleCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            GarmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            GarmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            GarmentSampleAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
            GarmentSampleAvalComponentItemRepository = storage.GetRepository<IGarmentSampleAvalComponentItemRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();

            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
        }
        class ViewFC
        {
            public string RO { get; internal set; }
            public double Total { get; internal set; }
        }

        class ViewBasicPrices
        {
            public string RO { get; internal set; }
            public decimal Total { get; internal set; }
        }
        public async Task<MemoryStream> Handle(GetXlsSampleCuttingQuery request, CancellationToken cancellationToken)
        {
            var _unitName = (from a in GarmentSampleCuttingOutRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();

			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
			dateFrom.AddHours(7);
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
			dateTo = dateTo.AddHours(7);


			var sumbasicPrice = (from a in (from cut in GarmentSamplePreparingRepository.Query
                                            where cut.UnitId == request.unit
                                            select new
                                            {
                                                cut.Identity,
                                                cut.UnitId,
                                                cut.RONo
                                            })
                                 join b in GarmentSamplePreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
                                 select new
                                 {
                                     a.RONo,
                                     b.BasicPrice
                                 })
                        .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                        {
                            RO = key.RONo,
                            Total = Convert.ToDecimal(group.Sum(s => s.BasicPrice) / group.Count())
                            //BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
                            //Count = group.Count()
                        });

           
            var sumFCs = (from a in (from cut in GarmentSampleCuttingInRepository.Query
                                     where cut.CuttingType == "Main Fabric" &&
                                    cut.UnitId == request.unit && cut.CuttingInDate <= dateTo
                                     select new
                                     {
                                         cut.Identity,
                                         cut.FC,
                                         cut.RONo
                                     })
                          join b in GarmentSampleCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in GarmentSampleCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new
                          {
                              a.FC,
                              a.RONo,
                              FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC),
                              c.CuttingInQuantity
                          })
                          .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                          {
                              RO = key.RONo,
                              Total = group.Sum(s => (s.FCs)) / group.Sum(s => s.CuttingInQuantity)
                              //FC = group.Sum(s => (s.FCs)),
                              //Count = group.Sum(s => s.CuttingInQuantity)
                          });

           
            //NEW QUERY
            var QueryCuttingIn = from a in (from aa in GarmentSampleCuttingInRepository.Query
                                            where aa.UnitId == request.unit && aa.CuttingInDate <= dateTo && aa.CuttingType == "Main Fabric"
                                            select new
                                            {
                                                aa.Identity,
                                                aa.RONo,
                                                aa.CuttingInDate,
                                                aa.Article
                                            })
                                 join b in GarmentSampleCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                 join c in GarmentSampleCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                 select new monitoringView
                                 {
                                     buyerCode = (from buyer in GarmentSamplePreparingRepository.Query where buyer.RONo == a.RONo select buyer.BuyerCode).FirstOrDefault(),
                                     price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault()),
                                     fc = (from cost in sumFCs where cost.RO == a.RONo select cost.Total).FirstOrDefault(),
                                     cuttingQtyMeter = 0,
                                     remainQty = 0,
                                     stock = a.CuttingInDate < dateFrom  ? c.CuttingInQuantity : 0,
                                     cuttingQtyPcs = a.CuttingInDate >= dateFrom ? c.CuttingInQuantity : 0,
                                     roJob = a.RONo,
                                     article = a.Article,
                                     style = (from buyer in GarmentSampleCuttingOutRepository.Query where buyer.RONo == a.RONo select buyer.ComodityName).FirstOrDefault(),
                                     expenditure = 0
                                 };


            
            //NEW QUERY
            var QueryCuttingOut = from a in (from aa in GarmentSampleCuttingOutRepository.Query
                                             where aa.UnitFromId == request.unit && aa.CuttingOutDate <= dateTo
                                             select new
                                             {
                                                 aa.Identity,
                                                 aa.RONo.Length,
                                                 aa.ComodityName,
                                                 aa.CuttingOutDate,
                                                 aa.RONo,
                                                 aa.Article
                                             })
                                  join b in GarmentSampleCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
                                  select new monitoringView
                                  {
                                      buyerCode = (from buyer in GarmentSamplePreparingRepository.Query where buyer.RONo == a.RONo select buyer.BuyerCode).FirstOrDefault(),
                                      price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault()),
                                      fc = (from cost in sumFCs where cost.RO == a.RONo select cost.Total).FirstOrDefault(),
                                      cuttingQtyMeter = 0,
                                      remainQty = 0,
                                      stock = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0,
                                      cuttingQtyPcs = 0,
                                      roJob = a.RONo,
                                      article = a.Article,
                                      style = a.ComodityName,
                                      expenditure = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0
                                  };

            var QueryAvalComp = from a in (from aa in GarmentSampleAvalComponentRepository.Query
                                           where aa.UnitId == request.unit && aa.Date <= dateTo
                                           select new
                                           {
                                               aa.Identity,
                                               aa.RONo,
                                               aa.Date,
                                               aa.Article,
                                               aa.ComodityName
                                           })
                                join b in GarmentSampleAvalComponentItemRepository.Query on a.Identity equals b.SampleAvalComponentId
                                select new monitoringView
                                {
                                    buyerCode = (from buyer in GarmentSamplePreparingRepository.Query where buyer.RONo == a.RONo select buyer.BuyerCode).FirstOrDefault(),
                                    price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault()),
                                    fc = (from cost in sumFCs where cost.RO == a.RONo select cost.Total).FirstOrDefault(),
                                    cuttingQtyMeter = 0,
                                    remainQty = 0,
                                    stock = a.Date < dateFrom ? -b.Quantity : 0,
                                    cuttingQtyPcs = 0,
                                    roJob = a.RONo,
                                    article = a.Article,
                                    style = a.ComodityName,
                                    expenditure = a.Date >= dateFrom ? b.Quantity : 0
                                };

            var queryNow = QueryCuttingIn.Union(QueryCuttingOut).Union(QueryAvalComp);

            var roList = (from a in queryNow
                          select a.roJob).Distinct().ToList();

            var sample = from s in GarmentSampleRequestRepository.Query
                             //join p in GarmentSampleRequestProductRepository.Query on s.Identity equals p.SampleRequestId
                         select new
                         {
                             s.RONoSample,
                             s.ComodityName,
                             s.BuyerCode,
                             Quantity = GarmentSampleRequestProductRepository.Query.Where(p => s.Identity == p.SampleRequestId).Sum(a => a.Quantity)
                         };


            var queryReport = from a in queryNow
                              select new monitoringView
                              {
                                  buyerCode = (from s in sample where s.RONoSample == a.roJob select s.BuyerCode).FirstOrDefault(),
                                  price = a.price,
                                  fc = a.fc,
                                  cuttingQtyMeter = a.cuttingQtyMeter,
                                  remainQty = a.remainQty,
                                  stock = a.stock,
                                  cuttingQtyPcs = a.cuttingQtyPcs,
                                  roJob = a.roJob,
                                  article = a.article,
                                  style = (from s in sample where s.RONoSample == a.roJob select s.ComodityName).FirstOrDefault(),
                                  expenditure = a.expenditure,
                                  //hours = (from sample in GarmentSampleRequestRepository.Query where sample.RONoSample == a.roJob select sample.Hours).FirstOrDefault(),
                                  qtyOrder = (from s in sample where s.RONoSample == a.roJob select s.Quantity).FirstOrDefault()
                              };
            var ccc = queryReport.ToList();
            foreach (var item in ccc)
            {
                item.fc = Math.Round(item.fc, 2);
            }

            var querySum = ccc.GroupBy(x => new { x.price, x.fc, x.buyerCode, x.qtyOrder, x.roJob, x.article, x.style, x.hours }, (key, group) => new
            {
                QtyOrder = key.qtyOrder,
                RoJob = key.roJob,
                Fc = key.fc,
                Stock = group.Sum(s => s.stock),
                buyer = key.buyerCode,
                bPrice = key.price,
                Article = key.article,
                Style = key.style,
                CuttingQtyPcs = group.Sum(s => s.cuttingQtyPcs),
                CuttingQtyMeter = group.Sum(s => s.cuttingQtyMeter),
                Expenditure = group.Sum(s => s.expenditure),
                Hours = key.hours
            }).OrderBy(s => s.RoJob);
            GarmentSampleCuttingMonitoringViewModel listViewModel = new GarmentSampleCuttingMonitoringViewModel();
            List<GarmentSampleCuttingMonitoringDto> monitoringCuttingDtos = new List<GarmentSampleCuttingMonitoringDto>();
            foreach (var item in querySum)
            {
                GarmentSampleCuttingMonitoringDto cuttingDto = new GarmentSampleCuttingMonitoringDto
                {
                    roJob = item.RoJob,
                    article = item.Article,
                    style = item.Style,
                    hours = item.Hours,
                    qtyOrder = item.QtyOrder,
                    cuttingQtyPcs = item.CuttingQtyPcs,
                    expenditure = item.Expenditure,
                    stock = item.Stock,
                    remainQty = item.Stock + item.CuttingQtyPcs - item.Expenditure,
                    fc = Math.Round(item.Fc, 2),
                    cuttingQtyMeter = Math.Round(item.Fc * item.CuttingQtyPcs, 2),
                    price = Math.Round(Convert.ToDecimal(item.bPrice), 2) * Convert.ToDecimal(Math.Round(item.Fc, 2)),
                    buyerCode = item.buyer,
					nominal = Convert.ToDecimal(item.Stock + item.CuttingQtyPcs - item.Expenditure ) * Math.Round(Convert.ToDecimal(item.bPrice), 2) * Convert.ToDecimal(Math.Round(item.Fc, 2))

				};
                monitoringCuttingDtos.Add(cuttingDto);
            }
            var data = from a in monitoringCuttingDtos
                       where a.stock > 0 || a.expenditure > 0 || a.cuttingQtyPcs > 0 || a.remainQty > 0
                       select a;
            double stocks = 0;
            double cuttingQtyPcs = 0;
            double expenditure = 0;
            decimal nominals = 0;
            foreach (var item in data)
            {
                stocks += item.stock;
                cuttingQtyPcs += item.cuttingQtyPcs;
                expenditure += item.expenditure;
                nominals += item.nominal;
            }
            monitoringCuttingDtos = data.ToList();
            GarmentSampleCuttingMonitoringDto cuttingDtos = new GarmentSampleCuttingMonitoringDto
            {
                roJob = "",
                article = "",
                productCode = "",
                style = "",
                hours = 0,
                qtyOrder = 0,
                cuttingQtyPcs = cuttingQtyPcs,
                expenditure = expenditure,
                stock = stocks,
                remainQty = stocks + cuttingQtyPcs - expenditure,
                fc = 0,
                cuttingQtyMeter = 0,
                price = 0,
                buyerCode = "",
                nominal = nominals

            };
            monitoringCuttingDtos.Add(cuttingDtos);
            listViewModel.garmentMonitorings = monitoringCuttingDtos;


            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO JOB", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Buyer", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Order", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Style", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FC", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Potong (M)", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Harga (M)", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Awal", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Hasil Potong", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Barang Keluar", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sisa Nominal", DataType = typeof(double) });
            int counter = 5;

			if (listViewModel.garmentMonitorings.Count > 0)
			{
				foreach (var report in listViewModel.garmentMonitorings)
				{
					reportDataTable.Rows.Add(report.roJob, report.article, report.buyerCode, report.qtyOrder, report.style, Math.Round(report.fc),report.cuttingQtyMeter, Math.Round(report.price, 2), report.stock, report.cuttingQtyPcs, report.expenditure, report.remainQty, report.nominal);
					counter++;

				}
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

				worksheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["E" + 2 + ":E" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Cells["G" + 6 + ":M" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Cells["G" + 6 + ":M" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["A" + 5 + ":M" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":M" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":M" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":M" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["I" + (counter) + ":M" + (counter) + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 1 + ":M" + 1 + ""].Style.Font.Bold = true;
				worksheet.Cells["A1"].Value = "Report Cutting";
				worksheet.Cells["A" + 1 + ":M" + 1 + ""].Merge = true;
				worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
				worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
				worksheet.Cells["A" + 1 + ":M" + 1 + ""].Merge = true;
				worksheet.Cells["A" + 2 + ":M" + 2 + ""].Merge = true;
				worksheet.Cells["A" + 3 + ":M" + 3 + ""].Merge = true;
				worksheet.Cells["A" + 1 + ":M" + 3 + ""].Style.Font.Size = 15;
				worksheet.Cells["A" + 1 + ":M" + 5 + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 1 + ":M" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				worksheet.Cells["A" + 1 + ":M" + 5 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
				var stream = new MemoryStream();
				if (request.type != "bookkeeping")
				{
					worksheet.Column(8).Hidden = true;
					worksheet.Column(13).Hidden = true;
					worksheet.Cells["A" + (counter) + ":g" + (counter) + ""].Merge = true;
				}
				else
				{
					worksheet.Cells["A" + (counter) + ":h" + (counter) + ""].Merge = true;
				}
				package.SaveAs(stream);

                return stream;
            }
        }
        class monitoringView
        {
            public string roJob { get; set; }
            public string article { get; set; }
            public string productCode { get; set; }
            public string buyerCode { get; set; }
            public double qtyOrder { get; set; }
            public string style { get; set; }
            public double fc { get; set; }
            public double hours { get; set; }
            public double cuttingQtyMeter { get; set; }
            public double stock { get; set; }
            public double cuttingQtyPcs { get; set; }
            public double expenditure { get; set; }
            public double remainQty { get; set; }
            public decimal price { get; set; }
        }
        
    }
}
