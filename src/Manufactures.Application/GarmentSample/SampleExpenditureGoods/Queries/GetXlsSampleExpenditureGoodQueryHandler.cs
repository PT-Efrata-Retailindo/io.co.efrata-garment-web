using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Threading;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries
{
    public class GetXlsSampleExpenditureGoodQueryHandler : IQueryHandler<GetXlsSampleExpenditureGoodQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        protected readonly IHttpClientService _http;
        private readonly IGarmentSampleExpenditureGoodRepository garmentExpenditureGoodRepository;
        private readonly IGarmentSampleExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
        private readonly IGarmentSamplePreparingRepository garmentPreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentSampleCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository garmentCuttingInDetailRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository GarmentSampleRequestProductRepository;

        public GetXlsSampleExpenditureGoodQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            _http = serviceProvider.GetService<IHttpClientService>();
            garmentExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
            garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();
            garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
        }

        class monitoringView
        {
            public string expenditureGoodNo { get; internal set; }
            public string expenditureGoodType { get; internal set; }
            public DateTimeOffset expenditureDate { get; internal set; }
            public string buyer { get; internal set; }
            public string roNo { get; internal set; }
            public string buyerArticle { get; internal set; }
            public string colour { get; internal set; }
            public string name { get; internal set; }
            public string unitname { get; internal set; }
            public double qty { get; internal set; }
            public string invoice { get; internal set; }
            public decimal price { get; internal set; }
            public double fc { get; set; }
        }

        public async Task<PEBResult> GetDataPEB(List<string> invoice, string token)
        {
            PEBResult pEB = new PEBResult();

            var listInvoice = string.Join(",", invoice.Distinct());
            var stringcontent = new StringContent(JsonConvert.SerializeObject(listInvoice), Encoding.UTF8, "application/json");

            var garmentProductionUri = CustomsDataSettings.Endpoint + $"customs-reports/getPEB";
            var httpResponse = await _http.SendAsync(HttpMethod.Get, garmentProductionUri, token, stringcontent);



            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();

                var listdata = JsonConvert.DeserializeObject<List<PEBResultViewModel>>(dataString);

                foreach (var i in listdata)
                {
                    pEB.data.Add(i);
                }
            }

            return pEB;
        }
        
        class ViewFC
        {
            public string RO { get; internal set; }
            public double FC { get; internal set; }
            public int Count { get; internal set; }
        }
        class ViewBasicPrices
        {
            public string RO { get; internal set; }
            public decimal BasicPrice { get; internal set; }
            public int Count { get; internal set; }
        }
        public async Task<MemoryStream> Handle(GetXlsSampleExpenditureGoodQuery request, CancellationToken cancellationToken)
        {
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
			dateFrom.AddHours(7);
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
			dateTo = dateTo.AddHours(7);

			var QueryRo = (from a in garmentExpenditureGoodRepository.Query
                           where a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.ExpenditureDate >= dateFrom && a.ExpenditureDate <= dateTo
                           select a.RONo).Distinct().ToList();

            List<string> _ro = new List<string>();
            foreach (var item in QueryRo)
            {
                _ro.Add(item);
            }
            var _unitName = (from a in garmentPreparingRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();
            GarmentMonitoringSampleExpenditureGoodListViewModel listViewModel = new GarmentMonitoringSampleExpenditureGoodListViewModel();
            List<GarmentMonitoringSampleExpenditureGoodDto> monitoringDtos = new List<GarmentMonitoringSampleExpenditureGoodDto>();
            var sumbasicPrice = (from a in garmentPreparingRepository.Query
                                 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
                                 where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) &&*/
                                 a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)
                                 select new { a.RONo, b.BasicPrice })
                        .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                        {
                            RO = key.RONo,
                            BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
                            Count = group.Count()
                        });

            var sumFCs = (from a in garmentCuttingInRepository.Query
                          where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && */ a.CuttingType == "Main Fabric" &&
                         a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.CuttingInDate <= dateTo
                          join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new { a.FC, a.RONo, FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC), c.CuttingInQuantity })
                       .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                       {
                           RO = key.RONo,
                           FC = group.Sum(s => (s.FCs)),
                           Count = group.Sum(s => s.CuttingInQuantity)
                       });
            var Query = from a in (from aa in garmentExpenditureGoodRepository.Query
                                   where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.ExpenditureDate >= dateFrom && aa.ExpenditureDate <= dateTo
                                   select aa)
                        join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                        where a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.ExpenditureDate >= dateFrom && a.ExpenditureDate <= dateTo
                        select new monitoringView
                        {
                            fc = (from aa in sumFCs where aa.RO == a.RONo select aa.FC / aa.Count).FirstOrDefault(),
                            price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()),
                            buyer = (from sample in GarmentSampleRequestRepository.Query where sample.RONoSample == a.RONo select sample.BuyerCode).FirstOrDefault(),
                            buyerArticle = a.BuyerCode + " " + a.Article,
                            roNo = a.RONo,
                            expenditureDate = a.ExpenditureDate,
                            expenditureGoodNo = a.ExpenditureGoodNo,
                            expenditureGoodType = a.ExpenditureType,
                            invoice = a.Invoice,
                            colour = b.Description,
                            qty = b.Quantity,
                            name = (from sample in GarmentSampleRequestRepository.Query where sample.RONoSample == a.RONo select sample.ComodityName).FirstOrDefault(),
                            unitname = a.UnitName
                        };



            var querySum = Query.ToList().GroupBy(x => new { x.fc, x.buyer, x.buyerArticle, x.roNo, x.expenditureDate, x.expenditureGoodNo, x.expenditureGoodType, x.invoice, x.colour, x.name, x.unitname }, (key, group) => new
            {
                ros = key.roNo,
                buyer = key.buyerArticle,
                expenditureDates = key.expenditureDate,
                qty = group.Sum(s => s.qty),
                expendituregoodNo = key.expenditureGoodNo,
                expendituregoodTypes = key.expenditureGoodType,
                color = key.colour,
                price = group.Sum(s => s.price),
                buyerC = key.buyer,
                names = key.name,
                unitname = key.unitname,
                invoices = key.invoice,
                fcs = key.fc

            }).OrderBy(s => s.expendituregoodNo);

            var Pebs = await GetDataPEB(querySum.Select(x => x.invoices).ToList(), request.token);

            foreach (var item in querySum)
            {
                var peb = Pebs.data.FirstOrDefault(x => x.BonNo.Trim() == item.invoices);
                GarmentMonitoringSampleExpenditureGoodDto dto = new GarmentMonitoringSampleExpenditureGoodDto
                {
                    roNo = item.ros,
                    buyerArticle = item.buyer,
                    expenditureGoodType = item.expendituregoodTypes,
                    expenditureGoodNo = item.expendituregoodNo,
                    expenditureDate = item.expenditureDates,
                    pebDate = peb == null ? new DateTime(1970, 1, 1) : peb.BCDate,
                    qty = item.qty,
                    colour = item.color,
                    name = item.names,
                    unitname = item.unitname,
                    invoice = item.invoices,
                    price = Math.Round(Convert.ToDecimal(Convert.ToDouble(Math.Round(item.price, 2)) * Math.Round(item.fcs, 2)), 2),
                    buyerCode = item.buyerC,
                    nominal = Math.Round(Convert.ToDecimal(item.qty) * Convert.ToDecimal(Convert.ToDouble(Math.Round(item.price, 2)) * Math.Round(item.fcs, 2)), 2)

                };
                monitoringDtos.Add(dto);
            }
            var data = from a in monitoringDtos
                       where a.qty > 0
                       select a;
            monitoringDtos = data.ToList();
            double qty = 0;
            decimal nominal = 0;
            foreach (var item in data)
            {
                qty += item.qty;
                nominal += item.nominal;

            }
            GarmentMonitoringSampleExpenditureGoodDto dtos = new GarmentMonitoringSampleExpenditureGoodDto
            {
                roNo = "",
                buyerArticle = "",
                expenditureGoodType = "",
                expenditureGoodNo = "",
                expenditureDate = null,
                pebDate = null,
                qty = qty,
                colour = "",
                name = "",
                unitname = "",
                invoice = "",
                price = 0,
                buyerCode = "",
                nominal = nominal

            };
            monitoringDtos.Add(dtos);
            listViewModel.garmentMonitorings = monitoringDtos;
            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO BON", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TIPE PENGELUARAN", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TGL PEB", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BUYER & ARTICLE", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "COLOUR", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NAMA", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "UNIT", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HARGA (PCS)", DataType = typeof(decimal) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "QTY", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NOMINAL", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "INVOICE", DataType = typeof(string) });
            int counter = 5;
            if (listViewModel.garmentMonitorings.Count > 0)
            {
                foreach (var report in listViewModel.garmentMonitorings)
                {

                    string pebDate = report.pebDate.GetValueOrDefault() == new DateTime(1970, 1, 1) || report.pebDate.GetValueOrDefault().ToString("dd MMM yyyy") == "01 Jan 0001" ? "-" : report.pebDate.GetValueOrDefault().ToString("dd MMM yyy");
                    //Console.WriteLine(pebDate);
                    reportDataTable.Rows.Add(report.expenditureGoodNo, report.expenditureGoodType, report.expenditureDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyy"), pebDate,
                    report.roNo, report.buyerArticle, report.colour, report.name, report.unitname,report.price, report.qty,report.nominal, report.invoice);
                    counter++;
                    //Console.WriteLine(counter);
                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				worksheet.Cells["A" + 5 + ":L" + 5 + ""].Style.Font.Bold = true;
				worksheet.Cells["A1"].Value = "Report Barang Jadi "; worksheet.Cells["A" + 1 + ":L" + 1 + ""].Merge = true;
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
				worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["I" + 2 + ":K" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells["J" + 2 + ":J" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 5 + ":L" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["I" + (counter) + ":J" + (counter) + ""].Style.Font.Bold = true;
				worksheet.Cells["A" + 5 + ":L" + 5 + ""].Style.Font.Bold = true;
				var stream = new MemoryStream();
				if (request.type != "bookkeeping")
				{
					worksheet.Cells["A" + (counter) + ":I" + (counter) + ""].Merge = true;

					worksheet.Column(9).Hidden = true;
				}
				else
				{
					worksheet.Cells["A" + (counter) + ":H" + (counter) + ""].Merge = true;
				}
				package.SaveAs(stream);

                return stream;
            }
        }
    }
}
