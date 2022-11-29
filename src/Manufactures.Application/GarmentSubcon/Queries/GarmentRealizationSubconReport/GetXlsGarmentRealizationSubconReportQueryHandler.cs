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

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport
{
    public class GetXlsGarmentRealizationSubconReportQueryHandler : IQueryHandler<GetXlsGarmentRealizationSubconReportQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentSubconCustomsInRepository garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository garmentSubconCustomsInItemRepository;
        private readonly IGarmentSubconCustomsOutRepository garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconContractRepository garmentSubconContractRepository;
        private readonly IGarmentSubconContractItemRepository garmentSubconContractItemRepository;

        public GetXlsGarmentRealizationSubconReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
            garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            garmentSubconContractItemRepository = storage.GetRepository<IGarmentSubconContractItemRepository>();



            _http = serviceProvider.GetService<IHttpClientService>();
        }

        class monitoringViewTemp
        {
            public string bcNoOut { get; set; }
            public DateTimeOffset bcDateOut { get; set; }
            public double quantityOut { get; set; }
            public string uomOut { get; set; }
            public string jobtype { get; set; }
            public string subconNo { get; set; }
            public string bpjNo { get; set; }
            public DateTimeOffset dueDate { get; set; }
        }

        public async Task<MemoryStream> Handle(GetXlsGarmentRealizationSubconReportQuery request, CancellationToken cancellationToken)
        {
            GarmentRealizationSubconReportListViewModel listViewModel = new GarmentRealizationSubconReportListViewModel();
            List<GarmentRealizationSubconReportDto> monitoringDtos = new List<GarmentRealizationSubconReportDto>();
            List<GarmentRealizationSubconReportDto> monitoringDtosOut = new List<GarmentRealizationSubconReportDto>();

            var QueryKeluar = from a in garmentSubconCustomsOutRepository.Query
                              join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                              join c in garmentSubconContractRepository.Query on a.SubconContractId equals c.Identity
                              where a.SubconContractNo == request.subconcontractNo
                              //&& a.Deleted == false && b.Deleted == false && c.Deleted == false
                              select new monitoringViewTemp
                              {
                                  bcDateOut = a.CustomsOutDate,
                                  bcNoOut = a.CustomsOutNo,
                                  quantityOut = b.Quantity,
                                  uomOut = c.UomUnit,
                                  jobtype = c.JobType,
                                  subconNo = c.ContractNo,
                                  bpjNo = c.BPJNo,
                                  dueDate = c.DueDate
                              };

            var QueryKeluar2 = from a in garmentSubconContractRepository.Query
                               join b in garmentSubconContractItemRepository.Query on a.Identity equals b.SubconContractId
                               where a.ContractNo == request.subconcontractNo
                               //&& a.Deleted == false && b.Deleted == false
                               select new monitoringViewTemp
                               {
                                   quantityOut = b.Quantity,
                                   uomOut = b.UomUnit,
                                   jobtype = b.ProductCode + "-" + b.ProductName,
                                   subconNo = a.ContractNo,
                                   bpjNo = a.BPJNo,
                                   dueDate = a.DueDate
                               };

            var QueryKeluar3 = QueryKeluar.Union(QueryKeluar2).AsEnumerable();

            var QueryMasuk = from a in garmentSubconCustomsInRepository.Query
                             join b in garmentSubconCustomsInItemRepository.Query on a.Identity equals b.SubconCustomsInId
                             join c in garmentSubconContractRepository.Query on a.SubconContractId equals c.Identity
                             where a.SubconContractNo == request.subconcontractNo
                             //&& a.Deleted == false && b.Deleted == false && c.Deleted == false
                             select new monitoringViewTemp
                             {
                                 bcDateOut = a.BcDate,
                                 bcNoOut = a.BcNo,
                                 quantityOut = (double)b.Quantity,
                                 uomOut = c.UomUnit,
                                 jobtype = c.JobType,
                                 subconNo = c.ContractNo,
                                 bpjNo = c.BPJNo,
                                 dueDate = c.DueDate
                             };

            foreach (var i in QueryKeluar3)
            {
                GarmentRealizationSubconReportDto dto = new GarmentRealizationSubconReportDto
                {
                    bcDateOut = i.bcDateOut,
                    bcNoOut = i.bcNoOut,
                    quantityOut = i.quantityOut,
                    uomOut = i.uomOut,
                    jobType = i.jobtype,
                    subconNo = i.subconNo,
                    bpjNo = i.bpjNo,
                    dueDate = i.dueDate
                };

                monitoringDtosOut.Add(dto);
            }

            foreach (var i in QueryMasuk)
            {
                GarmentRealizationSubconReportDto dto = new GarmentRealizationSubconReportDto
                {
                    bcDateOut = i.bcDateOut,
                    bcNoOut = i.bcNoOut,
                    quantityOut = i.quantityOut,
                    uomOut = i.uomOut,
                    jobType = i.jobtype,
                    subconNo = i.subconNo,
                    bpjNo = i.bpjNo,
                    dueDate = i.dueDate
                };

                monitoringDtos.Add(dto);
            }

            listViewModel.garmentRealizationSubconReportDtos = monitoringDtos;
            listViewModel.garmentRealizationSubconReportDtosOUT = monitoringDtosOut;


            var reportDataTableIN = new DataTable();
            var reportDataTableOut = new DataTable();
            var nodatatable = new DataTable();
            var headers = new string[] { "BC 261", "BC 261_1", "JENIS BARANG SUBCON", "JUMLAH BARANG", "SATUAN"};
            var headers2 = new string[] { "BC 262", "BC 262_1", "JENIS BARANG HASIL SUBCON", "JUMLAH BARANG", "SATUAN" };
            var subheaders = new string[] { "NO BC", "TANGGAL" };

            for (int i = 0; i < 3; i++)
            {
                reportDataTableIN.Columns.Add(new DataColumn() { ColumnName = headers2[i], DataType = typeof(string) });
            }

            for (int i = 0; i < 3; i++)
            {
                reportDataTableOut.Columns.Add(new DataColumn() { ColumnName = headers[i], DataType = typeof(string) });
            }

            reportDataTableIN.Columns.Add(new DataColumn() { ColumnName = headers2[3], DataType = typeof(Double) });
            reportDataTableIN.Columns.Add(new DataColumn() { ColumnName = headers2[4], DataType = typeof(string) });
            reportDataTableOut.Columns.Add(new DataColumn() { ColumnName = headers[3], DataType = typeof(Double) });
            reportDataTableOut.Columns.Add(new DataColumn() { ColumnName = headers[4], DataType = typeof(string) });

            nodatatable.Columns.Add(new DataColumn { ColumnName = "NO", DataType = typeof(Double) });

            //var indexin = 1;

            foreach (var item in listViewModel.garmentRealizationSubconReportDtos)
            {
                
                string date = item.bcDateOut == null ? "-" : item.bcDateOut.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                reportDataTableIN.Rows.Add(item.bcNoOut, date, item.jobType, item.quantityOut, item.uomOut);
            }
            //var indexout = 1;
            foreach (var item in listViewModel.garmentRealizationSubconReportDtosOUT)
            {

                string date = item.bcDateOut == null ? "-" : item.bcDateOut.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                reportDataTableOut.Rows.Add(item.bcNoOut, date, item.jobType, item.quantityOut, item.uomOut);
            }
            var index = 1;
            if (listViewModel.garmentRealizationSubconReportDtos.Count() > listViewModel.garmentRealizationSubconReportDtosOUT.Count())
            {
                foreach(var i in listViewModel.garmentRealizationSubconReportDtos)
                {
                    nodatatable.Rows.Add(index);
                    index++;
                }
            }
            else
            {
                foreach (var i in listViewModel.garmentRealizationSubconReportDtosOUT)
                {
                    nodatatable.Rows.Add(index);
                    index++;
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                var col = (char)('A' + 10);

                worksheet.Cells[$"A1:{col}1"].Value = "LAPORAN RALISASI PENGELUARAN (BC. 2.6.1) DAN PEMASUKAN (BC. 2.6.2)";
                worksheet.Cells[$"A1:{col}1"].Merge = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells[$"A2:{col}2"].Value = "SUBKONTRAK PT. DAN LIRIS KEPADA";
                worksheet.Cells[$"A2:{col}2"].Merge = true;
                worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var buyer = (from a in garmentSubconContractRepository.Query where a.ContractNo == request.subconcontractNo select a).FirstOrDefault();

                worksheet.Cells[$"A3:{col}3"].Value = buyer.SupplierName;
                worksheet.Cells[$"A3:{col}3"].Merge = true;
                worksheet.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells["A4"].Value = "NOMOR KONTRAK :"; worksheet.Cells["B4"].Value = buyer.ContractNo;
                worksheet.Cells["A5"].Value = "BPJ NO :"; worksheet.Cells["B5"].Value = buyer.BPJNo;
                worksheet.Cells["A6"].Value = "TGL JATUH TEMPO :"; worksheet.Cells["B6"].Value = buyer.DueDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                worksheet.Cells["B8"].Value = headers[0];
                worksheet.Cells["B8:C8"].Merge = true;
                worksheet.Cells["B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["B8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["B8:C8"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                worksheet.Cells["G8"].Value = headers2[0];
                worksheet.Cells["G8:H8"].Merge = true;
                worksheet.Cells["G8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["G8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["G8:H8"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                foreach (var i in Enumerable.Range(0, 1))
                {
                    col = (char)('A' + i);
                    worksheet.Cells[$"{col}8"].Value = "NO";
                    worksheet.Cells[$"{col}8:{col}9"].Merge = true;
                    worksheet.Cells[$"{col}8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"{col}8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"{col}8:{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                }

                foreach (var i in Enumerable.Range(0, 3))
                {
                    col = (char)('D' + i);
                    worksheet.Cells[$"{col}8"].Value = headers[i + 2];
                    worksheet.Cells[$"{col}8:{col}9"].Merge = true;
                    worksheet.Cells[$"{col}8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"{col}8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"{col}8:{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                }

                foreach (var i in Enumerable.Range(0, 3))
                {
                    col = (char)('I' + i);
                    worksheet.Cells[$"{col}8"].Value = headers2[i + 2];
                    worksheet.Cells[$"{col}8:{col}9"].Merge = true;
                    worksheet.Cells[$"{col}8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"{col}8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"{col}8:{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                }

                for (var i = 0; i < 2; i++)
                {
                    col = (char)('B' + i);
                    worksheet.Cells[$"{col}9"].Value = subheaders[i];
                    worksheet.Cells[$"{col}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    var col2 = (char)('G' + i);
                    worksheet.Cells[$"{col2}9"].Value = subheaders[i];
                    worksheet.Cells[$"{col2}9"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                }

                worksheet.Cells["B10"].LoadFromDataTable(reportDataTableOut, false, OfficeOpenXml.Table.TableStyles.Light16);
                worksheet.Cells["G10"].LoadFromDataTable(reportDataTableIN, false, OfficeOpenXml.Table.TableStyles.Light16);
                worksheet.Cells["A10"].LoadFromDataTable(nodatatable, false, OfficeOpenXml.Table.TableStyles.Light16);

                worksheet.Cells["B2"].Value = subheaders[0];
                worksheet.Cells["C2"].Value = subheaders[1];
                worksheet.Cells["H2"].Value = subheaders[0];
                worksheet.Cells["I2"].Value = subheaders[1];

                if(listViewModel.garmentRealizationSubconReportDtos.Count() > listViewModel.garmentRealizationSubconReportDtosOUT.Count())
                {
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "T O T A L  . . . . . . . . . . . . . . .";
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}:D{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Merge = true;
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}:D{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}:D{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}:D{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"E{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = listViewModel.garmentRealizationSubconReportDtosOUT.Sum(x=>x.quantityOut);
                    worksheet.Cells[$"J{10 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = listViewModel.garmentRealizationSubconReportDtos.Sum(x=>x.quantityOut);
                    worksheet.Cells[$"A{12 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "Kesimpulan";
                    worksheet.Cells[$"A{13 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "Pengeluaran dan pemasukan barang tersebut diatas";
                    worksheet.Cells[$"A{14 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "Sesuai dengan BC. 261 dengan BC. 262";
                    worksheet.Cells[$"A{15 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "Ijin Subkon = ";
                    worksheet.Cells[$"B{15 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = buyer.Quantity;
                    worksheet.Cells[$"A{16 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "Realisasi = ";
                    worksheet.Cells[$"B{16 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = listViewModel.garmentRealizationSubconReportDtosOUT.Sum(x => x.quantityOut);
                    worksheet.Cells[$"A{17 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "Sisa = ";
                    worksheet.Cells[$"B{17 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = buyer.Quantity - listViewModel.garmentRealizationSubconReportDtosOUT.Sum(x => x.quantityOut);
                    worksheet.Cells[$"J{12 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = string.Format("Sukoharjo, {0}",DateTimeOffset.Now.ToString("dd MMM yyyy", new CultureInfo("id-ID")));
                    worksheet.Cells[$"J{13 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "Mengetahui";
                    worksheet.Cells[$"J{13 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"J{13 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"J{14 + listViewModel.garmentRealizationSubconReportDtos.Count()}"].Value = "Pemeriksa Bea dan Cukai Pertama/Ahli Pertama";
                }
                else
                {
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "T O T A L  . . . . . . . . . . . . . . .";
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}:D{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Merge = true;
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}:D{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}:D{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}:D{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"E{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = listViewModel.garmentRealizationSubconReportDtosOUT.Sum(x => x.quantityOut);
                    worksheet.Cells[$"J{10 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = listViewModel.garmentRealizationSubconReportDtos.Sum(x => x.quantityOut);
                    worksheet.Cells[$"A{12 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "Kesimpulan";
                    worksheet.Cells[$"A{13 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "Pengeluaran dan pemasukan barang tersebut diatas";
                    worksheet.Cells[$"A{14 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "Sesuai dengan BC. 261 dengan BC. 262";
                    worksheet.Cells[$"A{15 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "Ijin Subkon = ";
                    worksheet.Cells[$"B{15 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = buyer.Quantity;
                    worksheet.Cells[$"A{16 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "Realisasi = ";
                    worksheet.Cells[$"B{16 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = listViewModel.garmentRealizationSubconReportDtosOUT.Sum(x => x.quantityOut);
                    worksheet.Cells[$"A{17 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "Sisa = ";
                    worksheet.Cells[$"B{17 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = buyer.Quantity - listViewModel.garmentRealizationSubconReportDtosOUT.Sum(x => x.quantityOut);
                    worksheet.Cells[$"J{12 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = string.Format("Sukoharjo, {0}", DateTimeOffset.Now.ToString("dd MMM yyyy", new CultureInfo("id-ID")));
                    worksheet.Cells[$"J{13 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "Mengetahui";
                    worksheet.Cells[$"J{13 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"J{13 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"J{14 + listViewModel.garmentRealizationSubconReportDtosOUT.Count()}"].Value = "Pemeriksa Bea dan Cukai Pertama/Ahli Pertama";
                }



                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;

            }

        }
    }
}
