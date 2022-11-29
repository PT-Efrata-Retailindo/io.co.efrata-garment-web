using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SampleStocks.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries.ArchiveMonitoring
{
    public class GarmentArchiveMonitoringXlsQueryHandler : IQueryHandler<GarmentArchiveMonitoringXlsQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleStockRepository _GarmentSampleStockRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        public GarmentArchiveMonitoringXlsQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            _GarmentSampleStockRepository = storage.GetRepository<IGarmentSampleStockRepository>();
            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
        }

        public async Task<MemoryStream> Handle(GarmentArchiveMonitoringXlsQuery request, CancellationToken cancellationToken)
        {
            List<GarmentArchiveMonitoringDto> monitoringDtos = new List<GarmentArchiveMonitoringDto>();
            monitoringDtos = (from a in _GarmentSampleStockRepository.Query
                              where a.ArchiveType == (request.type == null ? a.ArchiveType : request.type)
                              && a.RONo == (request.roNo == null ? a.RONo : request.roNo)
                              && a.ComodityCode == (request.comodity == null ? a.ComodityCode : request.comodity)
                              select new GarmentArchiveMonitoringDto
                              {
                                  comodity = a.ComodityName,
                                  archiveType = a.ArchiveType,
                                  article = a.Article,
                                  roNo = a.RONo,
                                  size = a.SizeName,
                                  qty = a.Quantity,
                                  uom = a.UomUnit,
                                  description = a.Description,
                                  buyer = (from sample in GarmentSampleRequestRepository.Query where sample.RONoSample == a.RONo select sample.BuyerName).FirstOrDefault()
                              }).OrderByDescending(a => a.roNo).ThenBy(b=>b.archiveType).ToList();

            GarmentArchiveMonitoringViewModel listViewModel = new GarmentArchiveMonitoringViewModel();

            listViewModel.garmentMonitorings = monitoringDtos;

            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tempat Arsip", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO No", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Artikel", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Comodity", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Size", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Quantity", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Keterangan", DataType = typeof(string) });
            int counter = 5;
            int idx = 1;
            var rCount = 0;
            Dictionary<string, string> Rowcount = new Dictionary<string, string>();

            if (listViewModel.garmentMonitorings.Count > 0)
            {
                foreach (var report in listViewModel.garmentMonitorings)
                {
                    idx++;
                    if (!Rowcount.ContainsKey(report.archiveType+report.roNo))
                    {
                        rCount = 0;
                        var index = idx;
                        Rowcount.Add(report.archiveType + report.roNo, index.ToString());
                    }
                    else
                    {
                        rCount += 1;
                        Rowcount[report.archiveType + report.roNo] = Rowcount[report.archiveType + report.roNo] + "-" + rCount.ToString();
                        var val = Rowcount[report.archiveType + report.roNo].Split("-");
                        if ((val).Length > 0)
                        {
                            Rowcount[report.archiveType + report.roNo] = val[0] + "-" + rCount.ToString();
                        }
                    }

                    reportDataTable.Rows.Add(report.archiveType, report.roNo, report.article, report.buyer, report.comodity, report.size, report.qty, report.uom, report.description);
                    counter++;

                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");


                worksheet.Cells["A" + 5 + ":I" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":I" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":I" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 5 + ":I" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(5).Style.Font.Bold = true;

                worksheet.Cells["A1"].Value = "Monitoring Arsip Sampel/MD";
                worksheet.Cells["A2"].Value = "  ";
                worksheet.Cells["A3"].Value = "  ";
                worksheet.Cells["A" + 1 + ":I" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":I" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":I" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 1 + ":I" + 3 + ""].Style.Font.Size = 15;
                worksheet.Cells["A" + 1 + ":I" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 6 + ":I" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["G" + 6 + ":I" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 1 + ":I" + 5 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
                worksheet.Cells["C" + 5 + ":I" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + 5 + ":I" + counter + ""].AutoFitColumns();
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

                    //worksheet.Cells[$"E{(rowNum1 + 4)}:E{(rowNum2 + 4)}"].Merge = true;
                    //worksheet.Cells[$"E{(rowNum1 + 4)}:E{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //worksheet.Cells[$"E{(rowNum1 + 4)}:E{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //worksheet.Cells[$"K{(rowNum1 + 4)}:K{(rowNum2 + 4)}"].Merge = true;
                    //worksheet.Cells[$"K{(rowNum1 + 4)}:K{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //worksheet.Cells[$"K{(rowNum1 + 4)}:K{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //worksheet.Cells[$"L{(rowNum1 + 4)}:L{(rowNum2 + 4)}"].Merge = true;
                    //worksheet.Cells[$"L{(rowNum1 + 4)}:L{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //worksheet.Cells[$"L{(rowNum1 + 4)}:L{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //worksheet.Cells[$"M{(rowNum1 + 4)}:M{(rowNum2 + 4)}"].Merge = true;
                    //worksheet.Cells[$"M{(rowNum1 + 4)}:M{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //worksheet.Cells[$"M{(rowNum1 + 4)}:M{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //worksheet.Cells[$"N{(rowNum1 + 4)}:N{(rowNum2 + 4)}"].Merge = true;
                    //worksheet.Cells[$"N{(rowNum1 + 4)}:N{(rowNum2 + 4)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //worksheet.Cells[$"N{(rowNum1 + 4)}:N{(rowNum2 + 4)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }

                package.SaveAs(stream);

                return stream;
            }
        }
    }
}
