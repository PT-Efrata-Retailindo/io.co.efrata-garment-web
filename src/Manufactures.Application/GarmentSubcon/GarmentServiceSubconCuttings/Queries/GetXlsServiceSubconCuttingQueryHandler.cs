using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System.Net.Http;
using System.Text;
using System.Globalization;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.Queries
{
    public class GetXlsServiceSubconCuttingQueryHandler : IQueryHandler<GetXlsServiceSubconCuttingQuery, MemoryStream>
    {

        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconCuttingRepository garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository garmentServiceSubconCuttingItemRepository;
        private readonly IGarmentServiceSubconCuttingDetailRepository garmentServiceSubconCuttingDetailRepository;
        private readonly IGarmentServiceSubconCuttingSizeRepository garmentServiceSubconCuttingSizeRepository;

        public GetXlsServiceSubconCuttingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
            garmentServiceSubconCuttingDetailRepository = storage.GetRepository<IGarmentServiceSubconCuttingDetailRepository>();
            garmentServiceSubconCuttingSizeRepository = storage.GetRepository<IGarmentServiceSubconCuttingSizeRepository>();

           _http = serviceProvider.GetService<IHttpClientService>();
        }

        class monitoringView
        {

            public string subconNo { get; internal set; }
            public string subconType { get; internal set; }
            public string unitName { get; internal set; }
            public DateTimeOffset subconDate { get; internal set; }
            public string roNo { get; internal set; }
            public string article { get; internal set; }
            public string comodity { get; internal set; }
            public string designColor { get; internal set; }
            public double cuttingInQuantity { get; internal set; }
            public string sizeName { get; internal set; }
            public double quantity { get; internal set; }
            public string uomUnit { get; internal set; }
            public string color { get; internal set; }
            public string uomUnitPacking { get; internal set; }
            public int qtyPacking { get; internal set; }

        }

        public async Task<MemoryStream> Handle(GetXlsServiceSubconCuttingQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            dateTo = dateTo.AddHours(7);


            var Query1 = (from a in garmentServiceSubconCuttingRepository.Query
                         join b in garmentServiceSubconCuttingItemRepository.Query on a.Identity equals b.ServiceSubconCuttingId
                         join c in garmentServiceSubconCuttingDetailRepository.Query on b.Identity equals c.ServiceSubconCuttingItemId
                         join d in garmentServiceSubconCuttingSizeRepository.Query on c.Identity equals d.ServiceSubconCuttingDetailId
                         where 
                         a.Deleted == false
                         && b.Deleted ==  false
                         && c.Deleted == false
                         && d.Deleted == false 
                         && a.SubconDate >= dateFrom
                         && a.SubconDate <= dateTo
                         select new
                         { 
                            subconNo = a.SubconNo,
                            subconType = a.SubconType,
                            unitName = a.UnitName,
                            subconDate = a.SubconDate,
                            roNo = b.RONo,
                            buyer = a.BuyerName,
                            article = b.Article,
                            comodity = b.ComodityName,
                            designColor = c.DesignColor,
                            //cuttingInQuantity = c.Quantity,
                            sizeName = d.SizeName,
                            quantity = d.Quantity,
                            uomUnit = d.UomUnit,
                            color = d.Color,
                            uomUnitPacking = a.UomUnit,
                            qtyPacking = a.QtyPacking,

                         }

                         
                );

            var Query = Query1.ToList().GroupBy(x => new { x.subconNo, x.subconType, x.unitName, x.roNo, x.subconDate, x.buyer, x.article, x.comodity, x.designColor, x.sizeName, x.uomUnit, x.color, x.uomUnitPacking, x.qtyPacking }, (key, group) => new
            {
                subconNo = key.subconNo,
                subconType = key.subconType,
                unitName = key.unitName,
                subconDate = key.subconDate,
                roNo = key.roNo,
                buyer = key.buyer,
                article = key.article,
                comodity = key.comodity,
                designColor = key.designColor,
                //cuttingInQuantity = c.Quantity,
                sizeName = key.sizeName,
                quantity = group.Sum(x => x.quantity),
                uomUnit = key.uomUnit,
                color = key.color,
                uomUnitPacking = key.uomUnitPacking,
                qtyPacking = key.qtyPacking

            }).OrderBy(s => s.subconNo);
            GarmentMonitoringServiceSubconCuttingViewModel listViewModel = new GarmentMonitoringServiceSubconCuttingViewModel();

            List<GarmentMonitoringServiceSubconCuttingDto> monitoringDtos = new List<GarmentMonitoringServiceSubconCuttingDto>();

            foreach (var item in Query)
            {
                //var peb = Pebs.data.FirstOrDefault(x => x.BonNo.Trim() == item.invoices);
                GarmentMonitoringServiceSubconCuttingDto dto = new GarmentMonitoringServiceSubconCuttingDto
                {
                    subconNo = item.subconNo,
                    subconType = item.subconType,
                    unitName = item.unitName,
                    subconDate = item.subconDate,
                    buyerName = item.buyer,
                    roNo = item.roNo,
                    article = item.article,
                    comodity = item.comodity,
                    designColor = item.designColor,
                    //cuttingInQuantity = c.Quantity,
                    sizeName = item.sizeName,
                    quantity = item.quantity,
                    uomUnit = item.uomUnit,
                    color = item.color,
                    uomUnitPacking = item.uomUnitPacking,
                    qtyPacking = item.qtyPacking
                };
                monitoringDtos.Add(dto);
            }

            var data = from a in monitoringDtos
                       where a.quantity > 0
                       select a;
            monitoringDtos = data.ToList();
            double quantity = 0;
            //decimal nominal = 0;
            foreach (var item in data)
            {
                quantity += item.quantity;
               // nominal += item.nominal;

            }
            GarmentMonitoringServiceSubconCuttingDto dtos = new GarmentMonitoringServiceSubconCuttingDto
            {
                subconNo = "",
                subconType = "",
                unitName = "",
                subconDate = null,
                buyerName = "",
                roNo = "",
                article = "",
                comodity = "",
                designColor = "",
                //cuttingInQuantity = c.Quantity,
                sizeName = "",
                quantity = quantity,
                uomUnit = "",
                color = "",
                uomUnitPacking = "",
                qtyPacking = 0
            };
            monitoringDtos.Add(dtos);
            listViewModel.garmentMonitorings = monitoringDtos;


            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Subcon Jasa Komponen", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jenis Subcon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Unit Asal", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tgl Subcon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan Packing", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Packing", DataType = typeof(int) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO No", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Artikel", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Desain Warna", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Ukuran", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Warna", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah", DataType = typeof(double) });
            int counter = 4;
            if (listViewModel.garmentMonitorings.Count > 0)
            {
                foreach (var report in listViewModel.garmentMonitorings)
                {

                    string subconDate = report.subconDate.GetValueOrDefault() == new DateTime(1970, 1, 1) || report.subconDate.GetValueOrDefault().ToString("dd MMM yyyy") == "01 Jan 0001" ? "-" : report.subconDate.GetValueOrDefault().ToString("dd MMM yyy");
                    //Console.WriteLine(pebDate);
                    reportDataTable.Rows.Add(report.subconNo, report.subconType, report.unitName, subconDate,
                    report.buyerName, report.uomUnitPacking, report.qtyPacking, report.roNo, report.article, report.comodity, report.designColor, report.sizeName, report.uomUnit, report.color, report.quantity);
                    counter++;
                    //Console.WriteLine(counter);
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A" + 4 + ":N" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A1"].Value = "Laporan  Subcon Jasa Komponen "; worksheet.Cells["A" + 1 + ":N" + 1 + ""].Merge = true;
                worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
                //worksheet.Cells["A3"].Value = "Konfeksi " + ;
                worksheet.Cells["A" + 1 + ":N" + 1 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":N" + 2 + ""].Merge = true;
                worksheet.Cells["A" + 3 + ":N" + 3 + ""].Merge = true;
                //worksheet.Cells["A" + 1 + ":L" + 2 + ""].Style.Font.Size = 15;
                worksheet.Cells["A" + 1 + ":N" + 4 + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 1 + ":N" + 4 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A" + 1 + ":N" + 4 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["A4"].LoadFromDataTable(reportDataTable, true);
                //worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["K" + 2 + ":M" + counter + ""].Style.Numberformat.Format = "#,##0.00";
                //worksheet.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["O" + 2 + ":O" + counter + ""].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells["O" + counter + ":O" + counter + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + counter].Value = "T O T A L";
                worksheet.Cells["A" + counter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["A" + counter].Style.Font.Bold = true;
                worksheet.Cells["A" + counter + ":N" + counter + ""].Merge = true;
                worksheet.Cells["A" + 4 + ":O" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 4 + ":O" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 4 + ":O" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 4 + ":O" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["K" + (counter) + ":L" + (counter) + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 4 + ":O" + 4 + ""].Style.Font.Bold = true;
                var stream = new MemoryStream();
                //if (request.type != "bookkeeping")
                //{
                //    worksheet.Cells["A" + (counter) + ":I" + (counter) + ""].Merge = true;

                //    worksheet.Column(9).Hidden = true;
                //}
                //else
                //{
                //    worksheet.Cells["A" + (counter) + ":H" + (counter) + ""].Merge = true;
                //}
                package.SaveAs(stream);

                return stream;
            }


            //var stream = new MemoryStream();

            //return stream;
        }
    }
}
