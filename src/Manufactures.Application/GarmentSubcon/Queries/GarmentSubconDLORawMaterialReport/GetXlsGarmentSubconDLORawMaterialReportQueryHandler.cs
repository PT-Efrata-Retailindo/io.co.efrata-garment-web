using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
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

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport
{
    public class GetXlsGarmentSubconDLORawMaterialReportQueryHandler : IQueryHandler<GetXlsGarmentSubconDLORawMaterialReportQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentSubconDeliveryLetterOutRepository garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository garmentSubconDeliveryLetterOutItemRepository;

        private readonly IGarmentServiceSubconShrinkagePanelRepository garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconShrinkagePanelItemRepository garmentServiceSubconShrinkagePanelItemRepository;
        private readonly IGarmentServiceSubconShrinkagePanelDetailRepository garmentServiceSubconShrinkagePanelDetailRepository;

        private readonly IGarmentServiceSubconFabricWashRepository garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconFabricWashItemRepository garmentServiceSubconFabricWashItemRepository;
        private readonly IGarmentServiceSubconFabricWashDetailRepository garmentServiceSubconFabricWashDetailRepository;


        public GetXlsGarmentSubconDLORawMaterialReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;

            garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            garmentSubconDeliveryLetterOutItemRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();

            garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            garmentServiceSubconShrinkagePanelItemRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelItemRepository>();
            garmentServiceSubconShrinkagePanelDetailRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelDetailRepository>();

            garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            garmentServiceSubconFabricWashItemRepository = storage.GetRepository<IGarmentServiceSubconFabricWashItemRepository>();
            garmentServiceSubconFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSubconFabricWashDetailRepository>();

            _http = serviceProvider.GetService<IHttpClientService>();
        }

        class monitoringViewTemp
        {
            public string DLType { get; internal set; }
            public string DLNo { get; internal set; }
            public DateTimeOffset DLDate { get; internal set; }
            public string ContractNo { get; internal set; }
            public string ContractType { get; internal set; }
            public string SubConCategory { get; internal set; }
            public string SubConNo { get; internal set; }
            public DateTimeOffset SubConDate { get; internal set; }
            public string UENNo { get; internal set; }
            public DateTimeOffset UENDate { get; internal set; }
            public string UnitSenderName { get; internal set; }
            public string UnitRequestName { get; internal set; }
            public string ProductCode { get; internal set; }
            public string ProductName { get; internal set; }
            public string DesignColour { get; internal set; }
            public decimal Quantity { get; internal set; }
            public string UomUnit { get; internal set; }
        }

        public async Task<MemoryStream> Handle(GetXlsGarmentSubconDLORawMaterialReportQuery request, CancellationToken cancellationToken)
        {
            var Query1 = (from a in garmentSubconDeliveryLetterOutRepository.Query
                          join b in garmentSubconDeliveryLetterOutItemRepository.Query on a.Identity equals b.SubconDeliveryLetterOutId
                          join c in garmentServiceSubconShrinkagePanelRepository.Query on b.SubconId equals c.Identity
                          join d in garmentServiceSubconShrinkagePanelItemRepository.Query on c.Identity equals d.ServiceSubconShrinkagePanelId
                          join e in garmentServiceSubconShrinkagePanelDetailRepository.Query on d.Identity equals e.ServiceSubconShrinkagePanelItemId
                          where a.Deleted == false && b.Deleted == false
                          && c.Deleted == false && d.Deleted == false && e.Deleted == false
                          && a.DLDate.AddHours(7).Date >= request.dateFrom
                          && a.DLDate.AddHours(7).Date <= request.dateTo.Date
                          && a.ContractType == "SUBCON BAHAN BAKU" && a.SubconCategory == "SUBCON BB SHRINKAGE/PANEL"

                          select new monitoringViewTemp
                          {
                              DLType = a.DLType,
                              DLNo = a.DLNo,
                              DLDate = a.DLDate,
                              ContractNo = a.EPONo,
                              ContractType = a.ContractType,
                              SubConCategory = a.SubconCategory,
                              SubConNo = b.SubconNo,
                              SubConDate = c.ServiceSubconShrinkagePanelDate,
                              UENNo = d.UnitExpenditureNo,
                              UENDate = d.ExpenditureDate,
                              UnitRequestName = d.UnitRequestName,
                              UnitSenderName = d.UnitSenderName,
                              ProductCode = e.ProductCode,
                              ProductName = e.ProductName,
                              DesignColour = e.DesignColor,
                              Quantity = e.Quantity,
                              UomUnit = e.UomUnit
                          }).GroupBy(x => new {
                              x.DLType,
                              x.DLNo,
                              x.DLDate,
                              x.ContractNo,
                              x.ContractType,
                              x.SubConCategory,
                              x.SubConNo,
                              x.SubConDate,
                              x.UENNo,
                              x.UENDate,
                              x.UnitRequestName,
                              x.UnitSenderName,
                              x.ProductCode,
                              x.ProductName,
                              x.DesignColour,
                              x.UomUnit
                          }, (key, group) => new
                          {
                              dlType = key.DLType,
                              dlNo = key.DLNo,
                              dlDate = key.DLDate,
                              contractNo = key.ContractNo,
                              contractType = key.ContractType,
                              subConCategory = key.SubConCategory,
                              subConNo = key.SubConNo,
                              subConDate = key.SubConDate,
                              uenNo = key.UENNo,
                              uenDate = key.UENDate,
                              unitRequestName = key.UnitRequestName,
                              unitSenderName = key.UnitSenderName,
                              productCode = key.ProductCode,
                              productName = key.ProductName,
                              designColour = key.DesignColour,
                              quantity = group.Sum(x => x.Quantity),
                              uomUnit = key.UomUnit
                          }).ToList();

            var Query2 = (from a in garmentSubconDeliveryLetterOutRepository.Query
                         join b in garmentSubconDeliveryLetterOutItemRepository.Query on a.Identity equals b.SubconDeliveryLetterOutId
                         join c in garmentServiceSubconFabricWashRepository.Query on b.SubconId equals c.Identity
                         join d in garmentServiceSubconFabricWashItemRepository.Query on c.Identity equals d.ServiceSubconFabricWashId
                         join e in garmentServiceSubconFabricWashDetailRepository.Query on d.Identity equals e.ServiceSubconFabricWashItemId
                          where a.Deleted == false && b.Deleted == false
                          && c.Deleted == false && d.Deleted == false && e.Deleted == false
                          && a.DLDate.AddHours(7).Date >= request.dateFrom
                          && a.DLDate.AddHours(7).Date <= request.dateTo.Date
                          && a.ContractType == "SUBCON BAHAN BAKU" && a.SubconCategory == "SUBCON BB FABRIC WASH/PRINT"

                          select new monitoringViewTemp
                         {
                             DLType = a.DLType,
                             DLNo = a.DLNo,
                             DLDate = a.DLDate,
                             ContractNo = a.EPONo,
                             ContractType = a.ContractType,
                             SubConCategory = a.SubconCategory,
                             SubConNo = b.SubconNo,
                             SubConDate = c.ServiceSubconFabricWashDate,
                             UENNo = d.UnitExpenditureNo == null ? "-" : d.UnitExpenditureNo,
                             UENDate = d.ExpenditureDate == null ? new DateTime(1970, 1, 1) : d.ExpenditureDate,
                             UnitRequestName = d.UnitRequestName == null ? "-" : d.UnitRequestName,
                             UnitSenderName = d.UnitSenderName == null ? "-" :  d.UnitSenderName ,
                             ProductCode = e.ProductCode,
                             ProductName = e.ProductName,
                             DesignColour = e.DesignColor,
                             Quantity = e.Quantity,
                             UomUnit = e.UomUnit
                         }).GroupBy(x => new {
                             x.DLType,
                             x.DLNo,
                             x.DLDate,
                             x.ContractNo,
                             x.ContractType,
                             x.SubConCategory,
                             x.SubConNo,
                             x.SubConDate,
                             x.UENNo,
                             x.UENDate,
                             x.UnitRequestName,
                             x.UnitSenderName,
                             x.ProductCode,
                             x.ProductName,
                             x.DesignColour,
                             x.UomUnit
                         }, (key, group) => new
                         {
                             dlType = key.DLType,
                             dlNo = key.DLNo,
                             dlDate = key.DLDate,
                             contractNo = key.ContractNo,
                             contractType = key.ContractType,
                             subConCategory = key.SubConCategory,
                             subConNo = key.SubConNo,
                             subConDate = key.SubConDate,
                             uenNo = key.UENNo,
                             uenDate = key.UENDate,
                             unitRequestName = key.UnitRequestName,
                             unitSenderName = key.UnitSenderName,
                             productCode = key.ProductCode,
                             productName = key.ProductName,
                             designColour = key.DesignColour,
                             quantity = group.Sum(x => x.Quantity),
                             uomUnit = key.UomUnit
                         }).ToList();


            var Query = Query1.Union(Query2).OrderBy(x => x.contractType).ThenBy(x => x.subConCategory).ThenBy(x => x.dlNo).ThenBy(x => x.contractNo).ThenBy(x => x.subConNo);

            GarmentSubconDLORawMaterialReportListViewModel listViewModel = new GarmentSubconDLORawMaterialReportListViewModel();
            List<GarmentSubconDLORawMaterialReportDto> rekapgarmentrawmaterial = new List<GarmentSubconDLORawMaterialReportDto>();

            foreach (var i in Query)
            {
                GarmentSubconDLORawMaterialReportDto report = new GarmentSubconDLORawMaterialReportDto
                {
                    DLType = i.dlType,
                    DLNo = i.dlNo,
                    DLDate = i.dlDate,
                    ContractNo = i.contractNo,
                    ContractType = i.contractType,
                    SubConCategory = i.subConCategory,
                    SubConNo = i.subConNo,
                    SubConDate = i.subConDate,
                    UENNo = i.uenNo,
                    UENDate = i.uenDate,
                    UnitRequestName = i.unitRequestName,
                    UnitSenderName = i.unitSenderName,
                    ProductCode = i.productCode,
                    ProductName = i.productName,
                    DesignColour = i.designColour,
                    Quantity = i.quantity,
                    UomUnit = i.uomUnit                                   
                };

                rekapgarmentrawmaterial.Add(report);
            }

            listViewModel.garmentSubconDLORawMaterialReportDto = rekapgarmentrawmaterial;

            DataTable reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis SJ SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kategori SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No SJ SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl SJ SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No PO External", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl SubCon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Bon Keluar", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Bon Keluar", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Yang Meminta", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Yang Mengirim", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Desain Warna", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Quantity", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });
         
            var index = 1;
            int idx = 1;
            var rCount = 0;
            Dictionary<string, string> Rowcount = new Dictionary<string, string>();

            if (listViewModel.garmentSubconDLORawMaterialReportDto.Count() == 0)
            {
                reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "");
            }
            else
            {                
                foreach (var item in listViewModel.garmentSubconDLORawMaterialReportDto)
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
                    string scDate = item.SubConDate == null ? "-" : item.SubConDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string uenDate = item.UENDate == new DateTime(1970, 1, 1) ? "-" : item.UENDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                    reportDataTable.Rows.Add(item.DLType, item.ContractType, item.SubConCategory, item.DLNo, dlDate, item.ContractNo, item.SubConNo, scDate, item.UENNo, uenDate,
                                             item.UnitRequestName, item.UnitSenderName, item.ProductCode, item.ProductName, item.DesignColour, item.Quantity, item.UomUnit);
                }
            }

            using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells["A4"].LoadFromDataTable(reportDataTable, true);

                var countdata = Query.Count();

                worksheet.Cells["A" + 1 + ":K" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "LAPORAN SURAT JALAN SUBCON | BAHAN BAKU";
                worksheet.Cells["A2"].Value = "Periode " + request.dateFrom.ToString("dd-MM-yyyy") + " S/D " + request.dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A" + 1 + ":Q" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":Q" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":Q" + 4 + ""].Style.Font.Bold = true;
                
                if (countdata > 0)
                {
                    worksheet.Cells["Q" + 5 + ":Q" + (4 + countdata) + ""].Merge = true;
                    worksheet.Cells["Q" + 5 + ":Q" + (4 + countdata) + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    worksheet.Cells[$"A{(5 + countdata)}:O{(5 + countdata)}"].Merge = true;
                    worksheet.Cells[$"A{(5 + countdata)}:P{(5 + countdata)}"].Style.Font.Bold = true;
                    //ADD SUMMARY OF QUANTITY
                    worksheet.Cells[$"A{(5 + countdata)}"].Value = "TOTAL SURAT JALAN SUBCON | BAHAN BAKU :";
                    worksheet.Cells[$"P{(5 + countdata)}"].Formula = "SUM(" + worksheet.Cells["P" + 5 + ":P" + (4 + countdata) + ""].Address + ")";
                    worksheet.Cells[$"Q{(5 + countdata)}"].Value = "MT";
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

