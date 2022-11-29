using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.GarmentSectionResult;

namespace Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample
{
    public class GetXlsReceiptSampleQueryHandler : IQueryHandler<GetXlsReceiptSampleQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentSampleRequestRepository garmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository garmentSampleRequestProductRepository;

        public GetXlsReceiptSampleQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
            garmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
        }
        public async Task<GarmentSectionResult> GetSectionNameById(List<int> id, string token)
        {
            List<GarmentSectionViewModel> sectionViewModels = new List<GarmentSectionViewModel>();

            GarmentSectionResult sectionResult = new GarmentSectionResult();
            foreach (var item in id.Distinct())
            {
                var uri = MasterDataSettings.Endpoint + $"master/garment-sections/{item}";
                var httpResponse = await _http.GetAsync(uri, token);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var a = httpResponse.Content.ReadAsStringAsync().Result;
                    Dictionary<string, object> keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);
                    var b = keyValues.GetValueOrDefault("data").ToString();

                    var garmentSection = JsonConvert.DeserializeObject<GarmentSectionViewModel>(b);
                    GarmentSectionViewModel garmentSectionViewModel = new GarmentSectionViewModel
                    {
                        Id = garmentSection.Id,
                        Name = garmentSection.Name
                    };
                    sectionViewModels.Add(garmentSectionViewModel);
                }

            }
            sectionResult.data = sectionViewModels;
            return sectionResult;
        }
        class monitoringView
        {
            public string sampleRequestNo { get; set; }
            public string roNoSample { get; set; }
            public string sampleCategory { get; set; }
            public string sampleType { get; set; }
            public string buyer { get; set; }
            public string style { get; set; }
            public string color { get; set; }
            public string sizeName { get; set; }
            public string sizeDescription { get; set; }
            public double quantity { get; set; }
            public DateTimeOffset sentDate { get; set; }
            public DateTimeOffset receivedDate { get; set; }
            public string garmentSectionName { get; set; }
            public DateTimeOffset sampleRequestDate { get; set; }
            public string sampleTo { get; set; }
        }
        public async Task<MemoryStream> Handle(GetXlsReceiptSampleQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset receivedDateFrom = new DateTimeOffset(request.receivedDateFrom);
            receivedDateFrom.AddHours(7);
            DateTimeOffset receivedDateTo = new DateTimeOffset(request.receivedDateTo);
            receivedDateTo = receivedDateTo.AddHours(7);

            var QuerySampleRequest = from a in (from aa in garmentSampleRequestRepository.Query
                                                where aa.ReceivedDate >= receivedDateFrom && aa.ReceivedDate <= receivedDateTo
                                                && aa.IsReceived == true && aa.IsRejected == false && aa.IsRevised == false
                                                select new
                                                {
                                                    aa.Identity,
                                                    aa.RONoSample,
                                                    aa.SampleCategory,
                                                    aa.SampleRequestNo,
                                                    aa.SampleType,
                                                    aa.BuyerCode,
                                                    aa.BuyerName,
                                                    aa.SentDate,
                                                    aa.ReceivedDate,
                                                    aa.Date,
                                                    aa.SectionId,
                                                    aa.SampleTo
                                                })
                                     join b in garmentSampleRequestProductRepository.Query on a.Identity equals b.SampleRequestId
                                     select new
                                     {
                                         Style = b.Style,
                                         Color = b.Color,
                                         SizeName = b.SizeName,
                                         SizeDescription = b.SizeDescription,
                                         Quantity = b.Quantity,
                                         RoNoSample = a.RONoSample,
                                         SampleCategory = a.SampleCategory,
                                         SampleRequestNo = a.SampleRequestNo,
                                         SampleType = a.SampleType,
                                         BuyerCode = a.BuyerCode,
                                         BuyerName = a.BuyerName,
                                         SentDate = a.SentDate,
                                         ReceivedDate = a.ReceivedDate,
                                         SampleRequestDate = a.Date,
                                         SectionId = a.SectionId,
                                         a.SampleTo
                                     };
            List<int> _sectionId = new List<int>();
            foreach (var item in QuerySampleRequest)
            {
                _sectionId.Add(item.SectionId);

            }
            _sectionId.Distinct();
            GarmentSectionResult garmentSectionResult = await GetSectionNameById(_sectionId, request.token);

            GarmentMonitoringReceiptSampleViewModel sampleViewModel = new GarmentMonitoringReceiptSampleViewModel();
            List<GarmentMonitoringReceiptSampleDto> sampleDtosList = new List<GarmentMonitoringReceiptSampleDto>();
            foreach (var item in QuerySampleRequest.OrderByDescending(s => s.ReceivedDate).OrderByDescending(s => s.RoNoSample))
            {
                GarmentMonitoringReceiptSampleDto receiptSampleDto = new GarmentMonitoringReceiptSampleDto()
                {
                    buyer = item.BuyerCode + " - " + item.BuyerName,
                    color = item.Color,
                    quantity = item.Quantity,
                    receivedDate = item.ReceivedDate,
                    roNoSample = item.RoNoSample,
                    sampleCategory = item.SampleCategory,
                    sampleRequestDate = item.SampleRequestDate,
                    sampleRequestNo = item.SampleRequestNo,
                    sampleType = item.SampleType,
                    sentDate = item.SentDate,
                    sizeDescription = item.SizeDescription,
                    style = item.Style,
                    sizeName = item.SizeName,
                    sampleTo=item.SampleTo,
                    garmentSectionName = (from aa in garmentSectionResult.data where aa.Id == item.SectionId select aa.Name).FirstOrDefault()
                };
                sampleDtosList.Add(receiptSampleDto);
            }
            sampleViewModel.garmentMonitorings = sampleDtosList;

            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Surat Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kategori Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tipe Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Color", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Size", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Keterangan Size", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Quantity", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Shipment", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Terima Surat Sample", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Md", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Pembuatan Surat Sample", DataType = typeof(string) });
            int counter = 5;
            int idx = 1;
            var rCount = 0;
            Dictionary<string, string> Rowcount = new Dictionary<string, string>();

            if (sampleViewModel.garmentMonitorings.Count > 0)
            {
                foreach (var report in sampleViewModel.garmentMonitorings)
                {
                    idx++;
                    if (!Rowcount.ContainsKey(report.sampleRequestNo))
                    {
                        rCount = 0;
                        var index = idx;
                        Rowcount.Add(report.sampleRequestNo, index.ToString());
                    }
                    else
                    {
                        rCount += 1;
                        Rowcount[report.sampleRequestNo] = Rowcount[report.sampleRequestNo] + "-" + rCount.ToString();
                        var val = Rowcount[report.sampleRequestNo].Split("-");
                        if ((val).Length > 0)
                        {
                            Rowcount[report.sampleRequestNo] = val[0] + "-" + rCount.ToString();
                        }
                    }
 
                    reportDataTable.Rows.Add(report.sampleRequestNo, report.roNoSample, report.sampleCategory, report.sampleTo, report.sampleType, report.buyer, report.style, report.color, report.sizeName, report.sizeDescription, report.quantity, report.sentDate.AddHours(7).ToString("dd MMMM yyyy"), report.receivedDate.Value.ToString("dd MMMM yyyy"), report.garmentSectionName, report.sampleRequestDate.AddHours(7).ToString("dd MMMM yyyy"));
                    counter++;

                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");


                worksheet.Cells["A" + 5 + ":O" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":O" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":O" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":O" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(5).Style.Font.Bold = true;

                worksheet.Cells["A1"].Value = "Monitoring Penerimaan Sample";
                worksheet.Cells["A2"].Value = "Periode " + receivedDateFrom.ToString("dd-MM-yyyy") + " s/d " + receivedDateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A3"].Value = "  ";
                worksheet.Cells["A" + 1 + ":O" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":O" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":O" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":O" + 3 + ""].Style.Font.Size = 15;
                worksheet.Cells["A" + 1 + ":O" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 6 + ":J" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["L" + 6 + ":O" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 1 + ":O" + 5 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells["E" + 5 + ":O" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 5 + ":O" + counter + ""].AutoFitColumns();
                var stream = new MemoryStream();
                foreach (var rowMerge in Rowcount)
                {
                    var UnitrowNum = rowMerge.Value.Split("-");
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

                    worksheet.Cells[$"E{(rowNum1 + 4)}:E{(rowNum2 + 4)}"].Merge = true;
                    worksheet.Cells[$"E{(rowNum1 + 4)}:E{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"E{(rowNum1 + 4)}:E{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells[$"F{(rowNum1 + 4)}:F{(rowNum2 + 4)}"].Merge = true;
                    worksheet.Cells[$"F{(rowNum1 + 4)}:F{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"F{(rowNum1 + 4)}:F{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells[$"K{(rowNum1 + 4)}:K{(rowNum2 + 4)}"].Merge = true;
                    worksheet.Cells[$"K{(rowNum1 + 4)}:K{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"K{(rowNum1 + 4)}:K{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells[$"L{(rowNum1 + 4)}:L{(rowNum2 + 4)}"].Merge = true;
                    worksheet.Cells[$"L{(rowNum1 + 4)}:L{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"L{(rowNum1 + 4)}:L{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells[$"M{(rowNum1 + 4)}:M{(rowNum2 + 4)}"].Merge = true;
                    worksheet.Cells[$"M{(rowNum1 + 4)}:M{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"M{(rowNum1 + 4)}:M{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells[$"N{(rowNum1 + 4)}:N{(rowNum2 + 4)}"].Merge = true;
                    worksheet.Cells[$"N{(rowNum1 + 4)}:N{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[$"N{(rowNum1 + 4)}:N{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }

                package.SaveAs(stream);

                return stream;
            }
        }
    }
}