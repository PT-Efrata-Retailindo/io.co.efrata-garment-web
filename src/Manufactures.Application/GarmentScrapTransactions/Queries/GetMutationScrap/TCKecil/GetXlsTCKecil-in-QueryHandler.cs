using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.TCKecil
{
    public class GetXlsTCKecil_in_QueryHandler : IQueryHandler<GetXlsTCKecil_in_Query, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;
        private readonly IGarmentScrapTransactionItemRepository _garmentScrapTransactionItemRepository;
        //private readonly IGarmentScrapClassificationRepository _garmentScrapClassificationRepository;
        //private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;
        //private readonly IGarmentScrapDestinationRepository _garmentScrapDestinationRepository;
        //private readonly IGarmentScrapStockRepository _garmentScrapStockRepository;

        public GetXlsTCKecil_in_QueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentScrapTransactionRepository = storage.GetRepository<IGarmentScrapTransactionRepository>();
            _garmentScrapTransactionItemRepository = storage.GetRepository<IGarmentScrapTransactionItemRepository>();
            //_garmentScrapClassificationRepository = storage.GetRepository<IGarmentScrapClassificationRepository>();
            //_garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
            //_garmentScrapDestinationRepository = storage.GetRepository<IGarmentScrapDestinationRepository>();
            //_garmentScrapStockRepository = storage.GetRepository<IGarmentScrapStockRepository>();

        }
        class monitoringView
        {
            public string transactionNo { get; set; }
            public DateTimeOffset transactionDate { get; set; }
            public string scrapSourceName { get; set; }
            public double quantity { get; set; }
            public string uomUnit { get; set; }
        }

        public async Task<MemoryStream> Handle(GetXlsTCKecil_in_Query request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(0, 0, 0));
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(0, 0, 0));

            List<ScrapDto> scrapDtos = new List<ScrapDto>();

            var ScrapTCKecil = (from a in _garmentScrapTransactionRepository.Query
                                join b in _garmentScrapTransactionItemRepository.Query on a.Identity equals b.ScrapTransactionId
                                where a.CreatedDate >= dateFrom && a.CreatedDate <= dateTo && a.Deleted == false && b.Deleted == false
                                && a.TransactionType == "IN" && b.ScrapClassificationName == "AVAL TC KECIL"
                                select new monitoringView
                                {
                                    transactionNo = a.TransactionNo,
                                    transactionDate = a.TransactionDate,
                                    scrapSourceName = a.ScrapSourceName,
                                    quantity = b.Quantity,
                                    uomUnit = b.UomUnit,
                                }).GroupBy(x => new { x.transactionNo, x.transactionDate, x.scrapSourceName, x.uomUnit }, (key, group) => new monitoringView
                                {
                                    transactionNo = key.transactionNo,
                                    transactionDate = key.transactionDate,
                                    scrapSourceName = key.scrapSourceName,
                                    quantity = group.Sum(x => x.quantity),
                                    uomUnit = key.uomUnit

                                });

            foreach (var a in ScrapTCKecil)
            {
                scrapDtos.Add(new ScrapDto
                {
                    TransactionNo = a.transactionNo,
                    TransactionDate = a.transactionDate,
                    ScrapSourceName = a.scrapSourceName,
                    Quantity = Math.Round(a.quantity, 2),
                    UomUnit = a.uomUnit,

                });
            }

            ScrapListViewModel scrapListViewModel = new ScrapListViewModel();
            scrapListViewModel.scrapDtos = scrapDtos;

            var reportDataTable = new DataTable();

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NOMOR BON MASUK", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TANGGAL PEMASUKAN", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "ASAL TERIMA", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "QUANTITY", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SATUAN", DataType = typeof(string) });

            if (scrapListViewModel.scrapDtos.Count > 0)
            {
                var index = 1;
                foreach (var report in scrapListViewModel.scrapDtos)
                {

                    var TransaksiDate = report.TransactionDate.ToString("dd MMM yyyy");
                    reportDataTable.Rows.Add(index++, report.TransactionNo, TransaksiDate, report.ScrapSourceName, report.Quantity, report.UomUnit);

                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                var countdata = scrapListViewModel.scrapDtos.Count();

                worksheet.Cells["A" + 1 + ":F" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "Laporan Monitoring Pemasukan Aval TC Kecil";
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                worksheet.Cells["A" + 1 + ":F" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":F" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":F" + 4 + ""].Style.Font.Bold = true;

                if(countdata > 0)
                {
                    worksheet.Cells["F" + 5 + ":F" + (4 + countdata) + ""].Merge = true;
                    worksheet.Cells["F" + 5 + ":F" + (4 + countdata) + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells["D" + 5 + ":D" + (4 + countdata) + ""].Merge = true;
                    worksheet.Cells["D" + 5 + ":D" + (4 + countdata) + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells["A4"].LoadFromDataTable(reportDataTable, true);

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;

            }
        }
    }
}
