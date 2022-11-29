using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
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

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport
{
    public class GetXlsGarmentSubconContractReportQueryHandler : IQueryHandler<GetXlsGarmentSubconContractReporQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        //private readonly IGarmentSubconCustomsInRepository garmentSubconCustomsInRepository;
        //private readonly IGarmentSubconCustomsInItemRepository garmentSubconCustomsInItemRepository;
        //private readonly IGarmentSubconCustomsOutRepository garmentSubconCustomsOutRepository;
        //private readonly IGarmentSubconCustomsOutItemRepository garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconContractRepository garmentSubconContractRepository;
        //private readonly IGarmentSubconContractItemRepository garmentSubconContractItemRepository;

        public GetXlsGarmentSubconContractReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            //garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            //garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
            //garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            //garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            //garmentSubconContractItemRepository = storage.GetRepository<IGarmentSubconContractItemRepository>();



            _http = serviceProvider.GetService<IHttpClientService>();
        }

        class monitoringViewTemp
        {
            public string ContractType { get; set; }
            public string ContractNo { get; set; }
            public string AgreementNo { get; set; }
            public int SupplierId { get; set; }
            public string SupplierCode { get; set; }
            public string SupplierName { get; set; }
            public string JobType { get; set; }
            public string BPJNo { get; set; }
            public string FinishedGoodType { get; set; }
            public double Quantity { get; set; }
            public DateTimeOffset DueDate { get; set; }
            public DateTimeOffset ContractDate { get; set; }
            public bool IsUsed { get; set; }
            public int BuyerId { get; set; }
            public string BuyerCode { get; set; }
            public string BuyerName { get; set; }
            public string SubconCategory { get; set; }
            public int UomId { get; set; }
            public string UomUnit { get; set; }
            public string SKEPNo { get; set; }
            public DateTimeOffset AgreementDate { get; set; }
        }

        public async Task<MemoryStream> Handle(GetXlsGarmentSubconContractReporQuery request, CancellationToken cancellationToken)
        {
            GarmentSubconContactReportListViewModel listViewModel = new GarmentSubconContactReportListViewModel();
            List<GarmentSubconContactReportDto> monitorincontact = new List<GarmentSubconContactReportDto>();

            var result = from a in garmentSubconContractRepository.Query
                         where a.SupplierId == (request.supplierNo != 0 ? request.supplierNo : a.SupplierId)
                         && a.ContractType == (string.IsNullOrWhiteSpace(request.contractType) ? a.ContractType : request.contractType)
                         && a.ContractDate >= request.dateFrom.Date
                         && a.ContractDate <= request.dateTo.Date
                         select new monitoringViewTemp
                         {
                             ContractType = a.ContractType,
                             ContractNo = a.ContractNo,
                             AgreementNo = a.AgreementNo,
                             SupplierId = a.SupplierId,
                             SupplierCode = a.SupplierCode,
                             SupplierName = a.SupplierName,
                             JobType = a.JobType,
                             BPJNo = a.BPJNo,
                             FinishedGoodType = a.FinishedGoodType,
                             Quantity = a.Quantity,
                             DueDate = a.DueDate,
                             ContractDate = a.ContractDate,
                             IsUsed = a.IsUsed,
                             BuyerId = a.BuyerId,
                             BuyerName = a.BuyerName,
                             BuyerCode = a.BuyerCode,
                             SubconCategory = a.SubconCategory,
                             UomId = a.UomId,
                             UomUnit = a.UomUnit,
                             SKEPNo = a.SKEPNo,
                             AgreementDate = a.AgreementDate
                         };

            foreach (var i in result)
            {
                GarmentSubconContactReportDto report = new GarmentSubconContactReportDto
                {
                    ContractType = i.ContractType,
                    ContractNo = i.ContractNo,
                    AgreementNo = i.AgreementNo,
                    SupplierId = i.SupplierId,
                    SupplierCode = i.SupplierCode,
                    SupplierName = i.SupplierName,
                    JobType = i.JobType,
                    BPJNo = i.BPJNo,
                    FinishedGoodType = i.FinishedGoodType,
                    Quantity = i.Quantity,
                    DueDate = i.DueDate,
                    ContractDate = i.ContractDate,
                    IsUsed = i.IsUsed,
                    BuyerId = i.BuyerId,
                    BuyerName = i.BuyerName,
                    BuyerCode = i.BuyerCode,
                    SubconCategory = i.SubconCategory,
                    UomId = i.UomId,
                    UomUnit = i.UomUnit,
                    SKEPNo = i.SKEPNo,
                    AgreementDate = i.AgreementDate
                };

                monitorincontact.Add(report);
            }

            listViewModel.garmentSubconContactReportDto = monitorincontact;

            DataTable ReportDataTable = new DataTable();
            //var nodatatable = new DataTable();

            ExcelPackage package = new ExcelPackage();
            //var sheet = package.Workbook.Worksheets.Add("Data");

            var headers = new string[] { "No", "No/Tanggal Kontrak", "Jenis Kontrak", "Penerima Subkon", "Buyer", "Tanggal Persetujuan", "Quanty", "Satuan", "Jenis Pekerjaan", "Jenis Barang Jadi", "Tanggal Jatuh Tempo" };

            for (int i = 0; i < 6; i++)
            {
                ReportDataTable.Columns.Add(new DataColumn() { ColumnName = headers[i], DataType = typeof(string) });
            }
            ReportDataTable.Columns.Add(new DataColumn() { ColumnName = headers[6], DataType = typeof(double) });
            ReportDataTable.Columns.Add(new DataColumn() { ColumnName = headers[7], DataType = typeof(string) });
            ReportDataTable.Columns.Add(new DataColumn() { ColumnName = headers[8], DataType = typeof(string) });
            ReportDataTable.Columns.Add(new DataColumn() { ColumnName = headers[9], DataType = typeof(string) });
            ReportDataTable.Columns.Add(new DataColumn() { ColumnName = headers[10], DataType = typeof(string) });

            //nodatatable.Columns.Add(new DataColumn { ColumnName = "NO", DataType = typeof(Double) });
            int index = 1;

            if (listViewModel.garmentSubconContactReportDto.ToArray().Count() == 0)
            {
                ReportDataTable.Rows.Add("", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                foreach (var item in listViewModel.garmentSubconContactReportDto)
                {
                   
                    string duedate = item.DueDate == null ? "-" : item.DueDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string contractdate = item.ContractDate == null ? "-" : item.ContractDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string agreementdate = item.AgreementDate == null ? "-" : item.AgreementDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                    ReportDataTable.Rows.Add(index++, item.ContractNo, item.ContractType, item.SupplierName, item.BuyerName, agreementdate, item.Quantity, item.UomUnit, item.JobType, item.FinishedGoodType, duedate);
                }
            }

            
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                var col = (char)('A' + 10);

                worksheet.Cells[$"A1:{col}1"].Value = "LAPORAN SUBKONTRAK PT. DAN LIRIS";
                worksheet.Cells[$"A1:{col}1"].Merge = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells[$"A2:{col}2"].Value = "KEPADA";
                worksheet.Cells[$"A2:{col}2"].Merge = true;
                worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //var buyer = (from a in garmentSubconContractRepository.Query
                //             where a.SupplierId == request.supplierNo
                //             && a.Deleted == false
                //             && a.ContractType == request.contractType
                //             && a.ContractDate >= request.dateFrom.Date
                //             && a.ContractDate <= request.dateTo.Date
                //             select a).FirstOrDefault();

                if (request.supplierNo != null)
                {
                    worksheet.Cells[$"A3:{col}3"].Value = listViewModel.garmentSubconContactReportDto.Select(x=> x.SupplierName);
                }
                else
                {
                    worksheet.Cells[$"A3:{col}3"].Value = "";
                }
                worksheet.Cells[$"A3:{col}3"].Merge = true;
                worksheet.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //worksheet.Cells["A4:B4"].Value = "NOMOR KONTRAK :"; worksheet.Cells["C4"].Value = buyer.ContractNo;
                //worksheet.Cells["A4:B4"].Merge = true;
                //worksheet.Cells["A5:B5"].Value = "BPJ NO :"; worksheet.Cells["C5"].Value = buyer.BPJNo;
                //worksheet.Cells["A5:B5"].Merge = true;
                //worksheet.Cells["A6:B6"].Value = "TGL JATUH TEMPO :"; worksheet.Cells["C6"].Value = buyer.DueDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                //worksheet.Cells["A6:B6"].Merge = true;

                foreach (var i in Enumerable.Range(0, 11))
                {
                    col = (char)('A' + i);
                    worksheet.Cells[$"{col}8"].Value = headers[i];
                    worksheet.Cells[$"{col}8:{col}9"].Merge = true;
                    worksheet.Cells[$"{col}8:{col}9"].Style.WrapText = true;
                    worksheet.Cells[$"{col}8:{col}9"].Style.Font.Bold = true;
                    worksheet.Cells[$"{col}8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"{col}8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"{col}8:{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                }

                //set header widht by px
                var widths = new int[] { 3, 22, 15, 32, 26, 12, 8, 8, 17, 17, 13};
                foreach (var i in Enumerable.Range(0, headers.Length))
                {
                    worksheet.Column(i + 1).Width = widths[i];
                }

                worksheet.Cells["A10"].LoadFromDataTable(ReportDataTable, false, OfficeOpenXml.Table.TableStyles.Light16);


                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;

            }
        }
    }
}

