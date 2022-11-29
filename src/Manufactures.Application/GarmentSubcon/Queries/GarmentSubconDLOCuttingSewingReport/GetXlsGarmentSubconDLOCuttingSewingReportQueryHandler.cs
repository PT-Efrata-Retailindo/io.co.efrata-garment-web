using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Infrastructure.External.DanLirisClient.Microservice;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.GarmentExpenditureNoteReport;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Data;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Http;
using Newtonsoft.Json;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOCuttingSewingReport
{
    public class GetXlsGarmentSubconDLOSewingReportQueryHandler : IQueryHandler<GetXlsGarmentSubconDLOCuttingSewingReportQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        public readonly IServiceProvider serviceProvider;
        private readonly IGarmentSubconDeliveryLetterOutRepository garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository garmentSubconDeliveryLetterOutItemRepository;

        public GetXlsGarmentSubconDLOSewingReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;

            garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            garmentSubconDeliveryLetterOutItemRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();

            _http = serviceProvider.GetService<IHttpClientService>();
        }

        public async Task<GarmentExpenditureNoteReport> GetDataUEN(List<int> id, string token)
        {

            GarmentExpenditureNoteReport garmentExpenditureNoteReport = new GarmentExpenditureNoteReport();

            var UENUri = PurchasingDataSettings.Endpoint + $"garment-unit-expenditure-notes/data/";

            var httpContent = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            var httpResponse = await _http.SendAsync(HttpMethod.Get, UENUri, token, httpContent);

            var freeUEN = new List<int>();

            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();
                var listData = JsonConvert.DeserializeObject<List<UENViewModel>>(dataString);

                foreach (var item in id)
                {
                    var data = listData.SingleOrDefault(s => s.UENId == item);
                    if (data != null)
                    {
                        garmentExpenditureNoteReport.data.Add(data);
                    }
                    else
                    {
                        freeUEN.Add(item);
                    }
                }
            }
            else
            {
                var err = await httpResponse.Content.ReadAsStringAsync();

            }
            return garmentExpenditureNoteReport;
        }

        class monitoringViewTemp
        {
            public string DLType { get; internal set; }
            public string DLNo { get; internal set; }
            public DateTimeOffset DLDate { get; internal set; }
            public string ContractNo { get; internal set; }
            public string ContractType { get; internal set; }
            public string SubConCategory { get; internal set; }
            public string UENNo { get; internal set; }
            public DateTimeOffset UENDate { get; internal set; }
            public string RONo { get; internal set; }
            public string PONo { get; internal set; }
            public string UnitRequestName { get; internal set; }
            public string UnitSenderName { get; internal set; }
            public string ProductCode { get; internal set; }
            public string ProductName { get; internal set; }
            public string FabricType { get; internal set; }
            public string Colour { get; internal set; }
            public double QtyUEN { get; internal set; }
            public string UomUEN { get; internal set; }
            public double QtyOut { get; internal set; }
            public string UomUnit { get; internal set; }
        }

        public async Task<MemoryStream> Handle(GetXlsGarmentSubconDLOCuttingSewingReportQuery request, CancellationToken cancellationToken)
        {
            var QueryUEN = (from a in garmentSubconDeliveryLetterOutRepository.Query
                            join b in garmentSubconDeliveryLetterOutItemRepository.Query on a.Identity equals b.SubconDeliveryLetterOutId
                            where a.Deleted == false && b.Deleted == false
                            && a.DLDate.AddHours(7).Date >= request.dateFrom
                            && a.DLDate.AddHours(7).Date <= request.dateTo.Date && a.UENId != null
                            && a.ContractType == "SUBCON GARMENT" && a.SubconCategory == "SUBCON CUTTING SEWING"
                           select a.UENId).Distinct();
            List<int> _uen = new List<int>();
            foreach (var item in QueryUEN)
            {
                _uen.Add(item);
            }
            GarmentExpenditureNoteReport gmtExpNoteReport = await GetDataUEN(_uen, request.token);

            var Query = (from a in garmentSubconDeliveryLetterOutRepository.Query
                         join b in garmentSubconDeliveryLetterOutItemRepository.Query on a.Identity equals b.SubconDeliveryLetterOutId
                         //where a.Deleted == false && b.Deleted == false
                         where
                         a.DLDate.AddHours(7).Date >= request.dateFrom
                         && a.DLDate.AddHours(7).Date <= request.dateTo.Date
                         //&& a.ContractType == "SUBCON GARMENT" && a.SubconCategory == "SUBCON CUTTING SEWING"                   
                         select new monitoringViewTemp
                         {
                             DLType = a.DLType,
                             DLNo = a.DLNo,
                             DLDate = a.DLDate,
                             ContractNo = a.EPONo,
                             ContractType = a.ContractType,
                             SubConCategory = a.SubconCategory,
                             UENNo = a.UENNo,
                             PONo = a.PONo,
                             ProductCode = b.ProductCode,
                             ProductName = b.ProductName,
                             Colour = b.DesignColor,
                             QtyOut = b.Quantity,
                             UomUnit = b.UomUnit,
                             UENDate = (from UEN in gmtExpNoteReport.data where UEN.UENId == a.UENId select UEN.UENDate).FirstOrDefault(),
                             RONo = (from UEN in gmtExpNoteReport.data where UEN.UENId == a.UENId select UEN.RONo).FirstOrDefault(),
                             UnitRequestName = (from UEN in gmtExpNoteReport.data where UEN.UENId == a.UENId select UEN.UnitRequestName).FirstOrDefault(),
                             UnitSenderName = (from UEN in gmtExpNoteReport.data where UEN.UENId == a.UENId select UEN.UnitSenderName).FirstOrDefault(),
                             FabricType = (from UEN in gmtExpNoteReport.data where UEN.UENId == a.UENId select UEN.FabricType).FirstOrDefault(),
                             QtyUEN = (from UEN in gmtExpNoteReport.data where UEN.UENId == a.UENId select UEN.Quntity).FirstOrDefault(),
                             UomUEN = (from UEN in gmtExpNoteReport.data where UEN.UENId == a.UENId select UEN.UOMUnit).FirstOrDefault(),
                         }).ToList().OrderBy(x => x.DLNo).ThenBy(x => x.UENNo);

                          //.GroupBy(x => new {
                          //    x.DLType,
                          //    x.DLNo,
                          //    x.DLDate,
                          //    x.ContractNo,
                          //    x.ContractType,
                          //    x.SubConCategory,
                          //    x.UENNo,
                          //    x.UENDate,
                          //    x.RONo,
                          //    x.UnitName,
                          //    x.PONo,
                          //    x.ComodityCode,
                          //    x.ComoditytName,
                          //    x.ProductCode,
                          //    x.ProductName,
                          //    x.Size,
                          //    x.Colour,
                          //    x.Quantity,
                          //    x.UomUnit
                          //}, (key, group) => new
                          //{
                          //    dlType = key.DLType,
                          //    dlNo = key.DLNo,
                          //    dlDate = key.DLDate,
                          //    contractNo = key.ContractNo,
                          //    contractType = key.ContractType,
                          //    subConCategory = key.SubConCategory,
                          //    subConNo = key.SubConNo,
                          //    subConDate = key.SubConDate,
                          //    rONo = key.RONo,
                          //    unitName = key.UnitName,
                          //    pONo = key.PONo,
                          //    comodityCode = key.ComodityCode,
                          //    comoditytName = key.ComoditytName,                             
                          //    productCode = key.ProductCode,
                          //    productName = key.ProductName,
                          //    size = key.Size,
                          //    colour = key.Colour,
                          //    quantity = group.Sum(x => x.Quantity),
                          //    uomUnit = key.UomUnit
                          //}).ToList();


            GarmentSubconDLOCuttingSewingReportListViewModel listViewModel = new GarmentSubconDLOCuttingSewingReportListViewModel();
            List<GarmentSubconDLOCuttingSewingReportDto> rekapcuttingsewing = new List<GarmentSubconDLOCuttingSewingReportDto>();

            foreach (var i in Query)
            {
                GarmentSubconDLOCuttingSewingReportDto report = new GarmentSubconDLOCuttingSewingReportDto
                {
                    DLType = i.DLType,
                    DLNo = i.DLNo,
                    DLDate = i.DLDate,
                    ContractNo = i.ContractNo,
                    ContractType = i.ContractType,
                    SubConCategory = i.SubConCategory,
                    UENNo = i.UENNo,
                    UENDate = i.UENDate,
                    RONo = i.RONo,
                    PONo = i.PONo,
                    UnitRequestName = i.UnitRequestName,
                    UnitSenderName = i.UnitSenderName,
                    ProductCode = i.ProductCode,
                    ProductName = i.ProductName,
                    FabricType = i.FabricType,
                    Colour = i.Colour,
                    QtyUEN = i.QtyUEN,
                    UomUEN = i.UomUEN,
                    QtyOut = i.QtyOut,
                    UomUnit = i.UomUnit
                };

                rekapcuttingsewing.Add(report);
            }

            listViewModel.garmentSubconDLOCuttingSewingReportDto = rekapcuttingsewing;

            DataTable reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis SJ SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kategori SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No SJ SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl SJ SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No PO External", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Bon Keluar", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Bon Keluar", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO No", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No PO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Yang Meminta", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Uniy Yang Mengirim", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis Fabric", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Colour", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Bon", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan Bon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Qty Keluar", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan Keluar", DataType = typeof(string) });

            var index = 1;
            int idx = 1;
            var rCount = 0;
            Dictionary<string, string> Rowcount = new Dictionary<string, string>();

            if (listViewModel.garmentSubconDLOCuttingSewingReportDto.Count() == 0)
            {
                reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", 0, "");
            }
            else
            {                
                foreach (var item in listViewModel.garmentSubconDLOCuttingSewingReportDto)
                {
                    idx++;
                    if (!Rowcount.ContainsKey(item.DLNo))
                    {
                        rCount = 0;
                        var index1 = idx;
                        Rowcount.Add(item.DLNo, index1.ToString());
                    }
                    else
                    {
                        rCount += 1;
                        Rowcount[item.DLNo] = Rowcount[item.DLNo] + "-" + rCount.ToString();
                        var val = Rowcount[item.DLNo].Split("-");
                        if ((val).Length > 0)
                        {
                            Rowcount[item.DLNo] = val[0] + "-" + rCount.ToString();
                        }
                    }

                    string dlDate = item.DLDate == null ? "-" : item.DLDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string uenDate = item.UENDate == null ? "-" : item.UENDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                    reportDataTable.Rows.Add(item.DLType, item.ContractType, item.SubConCategory, item.DLNo, dlDate, item.ContractNo, item.UENDate, uenDate,
                                             item.RONo, item.PONo, item.UnitRequestName, item.UnitSenderName, item.ProductCode, item.ProductName, item.FabricType,
                                             item.Colour, item.QtyUEN, item.UomUEN, item.QtyOut, item.UomUnit);
                }
            }

            using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells["A4"].LoadFromDataTable(reportDataTable, true);

                var countdata = Query.Count();

                worksheet.Cells["A" + 1 + ":K" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "LAPORAN SURAT JALAN SUBCON | GARMENT CUTTING SEWING";
                worksheet.Cells["A2"].Value = "Periode " + request.dateFrom.ToString("dd-MM-yyyy") + " S/D " + request.dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A" + 1 + ":T" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":T" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":T" + 4 + ""].Style.Font.Bold = true;
                
                if (countdata > 0)
                {
                    //worksheet.Cells["S" + 5 + ":S" + (4 + countdata) + ""].Merge = true;
                    //worksheet.Cells["S" + 5 + ":S" + (4 + countdata) + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"A{(5 + countdata)}:N{(5 + countdata)}"].Merge = true;
                    worksheet.Cells[$"A{(5 + countdata)}:O{(5 + countdata)}"].Style.Font.Bold = true;
                    //ADD SUMMARY OF QUANTITY
                    worksheet.Cells[$"A{(5 + countdata)}"].Value = "TOTAL SURAT JALAN SUBCON | GARMENT CUTTING SEWING :";
                    worksheet.Cells[$"P{(5 + countdata)}"].Formula = "SUM(" + worksheet.Cells["P" + 5 + ":P" + (4 + countdata) + ""].Address + ")";
                    worksheet.Cells[$"R{(5 + countdata)}"].Formula = "SUM(" + worksheet.Cells["R" + 5 + ":R" + (4 + countdata) + ""].Address + ")";
                    //worksheet.Cells[$"S{(5 + countdata)}"].Value = "PCS";
                    worksheet.Calculate();
                }

                //
                foreach (var a in Rowcount)
                {
                    var UnitrowNum = a.Value.Split("-");
                    int rowNum2 = 1;
                    int rowNum1 = Convert.ToInt32(UnitrowNum[0]);
                    if (UnitrowNum.Length > 1)
                    {
                        rowNum2 = Convert.ToInt32(rowNum1) + Convert.ToInt32(UnitrowNum[1]);
                    }
                    else
                    {
                        rowNum2 = Convert.ToInt32(rowNum1);
                    }

                    worksheet.Cells[$"A{(rowNum1 + 3)}:A{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"A{(rowNum1 + 3)}:A{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"A{(rowNum1 + 3)}:A{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"B{(rowNum1 + 3)}:B{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"B{(rowNum1 + 3)}:B{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"B{(rowNum1 + 3)}:B{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"C{(rowNum1 + 3)}:C{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"C{(rowNum1 + 3)}:C{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"C{(rowNum1 + 3)}:C{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"D{(rowNum1 + 3)}:D{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"D{(rowNum1 + 3)}:D{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"D{(rowNum1 + 3)}:D{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"E{(rowNum1 + 3)}:E{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"E{(rowNum1 + 3)}:E{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"E{(rowNum1 + 3)}:E{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"F{(rowNum1 + 3)}:F{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"F{(rowNum1 + 3)}:F{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"F{(rowNum1 + 3)}:F{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"G{(rowNum1 + 3)}:G{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"G{(rowNum1 + 3)}:G{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"G{(rowNum1 + 3)}:G{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"H{(rowNum1 + 3)}:H{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"H{(rowNum1 + 3)}:H{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"H{(rowNum1 + 3)}:H{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"I{(rowNum1 + 3)}:I{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"I{(rowNum1 + 3)}:I{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"I{(rowNum1 + 3)}:I{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"J{(rowNum1 + 3)}:J{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"J{(rowNum1 + 3)}:J{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"J{(rowNum1 + 3)}:J{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"K{(rowNum1 + 3)}:K{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"K{(rowNum1 + 3)}:K{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"K{(rowNum1 + 3)}:K{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"L{(rowNum1 + 3)}:L{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"L{(rowNum1 + 3)}:L{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"L{(rowNum1 + 3)}:L{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"M{(rowNum1 + 3)}:M{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"M{(rowNum1 + 3)}:M{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"M{(rowNum1 + 3)}:M{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"N{(rowNum1 + 3)}:N{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"N{(rowNum1 + 3)}:N{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"N{(rowNum1 + 3)}:N{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"O{(rowNum1 + 3)}:O{(rowNum2) + 3}"].Merge = true;
                    worksheet.Cells[$"O{(rowNum1 + 3)}:O{(rowNum2) + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"O{(rowNum1 + 3)}:O{(rowNum2) + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
				return stream;

            }
        }
    }
}

