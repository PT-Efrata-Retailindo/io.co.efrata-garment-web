using ExtCore.Data.Abstractions;
using Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;
using Infrastructure.Domain.Queries;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetReportExpenditureGoods
{
    public class GetXlsReportExpenditureGoodsQueryHandler : IQueryHandler<GetXlsReportExpenditureGoodsQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
        private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;

        public GetXlsReportExpenditureGoodsQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;

            garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
        }

        public async Task<MemoryStream> Handle(GetXlsReportExpenditureGoodsQuery request, CancellationToken cancellationToken)
        {
            GarmentReportExpenditureGoodListViewModel expenditureGoodListViewModel = new GarmentReportExpenditureGoodListViewModel();
            List<GarmentReportExpenditureGoodDto> mutationExpenditureGoodDto = new List<GarmentReportExpenditureGoodDto>();

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));

            var factexpend = from a in (from aa in garmentExpenditureGoodRepository.Query
                                        where aa.ExpenditureDate >= dateFrom && aa.ExpenditureDate <= dateTo //&& aa.ComodityCode == "BR"
                                        select aa)
                             join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                             select new GarmentReportExpenditureGoodDto
                             {
                                 Article = a.Article,
                                 BuyerContract = a.ContractNo,
                                 ComodityCode = a.ComodityCode,
                                 ComodityName = a.ComodityName,
                                 Description = a.Description,
                                 Descriptionb = b.Description,
                                 ExpenditureGoodId = a.ExpenditureGoodNo,
                                 ExpenditureTypeName = a.ExpenditureType,
                                 Qty = b.Quantity,
                                 RO = a.RONo,
                                 SizeNumber = b.SizeName,
                                 UnitCode = a.UnitName
                             };

            List<string> ComodityCode = factexpend.Select(x => x.ComodityCode).Distinct().ToList();
            var ComodityName = (from x in garmentCuttingOutRepository.Query
                                         where ComodityCode.Contains(x.ComodityCode)
                                         select new { x.ComodityName, x.ComodityCode }).Distinct().ToList();

            foreach (var a in factexpend)
            {
                var comodity = ComodityName.FirstOrDefault(x => x.ComodityCode == x.ComodityCode).ComodityName;

                GarmentReportExpenditureGoodDto dto = new GarmentReportExpenditureGoodDto
                {
                    Article = a.Article,
                    BuyerContract = a.BuyerContract,
                    ComodityCode = a.ComodityCode,
                    ComodityName = comodity,
                    Description = a.Description,
                    Descriptionb = a.Description,
                    ExpenditureGoodId = a.ExpenditureGoodId,
                    ExpenditureTypeName = a.ExpenditureTypeName,
                    Qty = a.Qty,
                    RO = a.RO,
                    SizeNumber = a.SizeNumber,
                    UnitCode = a.UnitCode
                };

                mutationExpenditureGoodDto.Add(dto);
            }

            expenditureGoodListViewModel.garmentReports = mutationExpenditureGoodDto.OrderBy(x => x.ComodityCode).ToList();

            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Bon", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No RO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Artikel", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Unit", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer Contract", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tipe", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Deskripsi", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Komoditi", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Ukuran", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Warna", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah", DataType = typeof(double) });
            int counter = 1;
            foreach (var report in expenditureGoodListViewModel.garmentReports)
            {
                reportDataTable.Rows.Add(counter, report.ExpenditureGoodId, report.RO, report.Article, report.UnitCode, report.BuyerContract, report.ExpenditureTypeName, report.Description, report.ComodityCode, report.ComodityName, report.SizeNumber, report.Descriptionb, report.Qty);
                counter++;
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A2"].LoadFromDataTable(reportDataTable, true, OfficeOpenXml.Table.TableStyles.Light16);

                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;

            }
        }
    }
}
