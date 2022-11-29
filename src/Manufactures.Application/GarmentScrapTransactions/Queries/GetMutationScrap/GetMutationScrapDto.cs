using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap
{
    public class GetMutationScrapDto
    {
        
        public GetMutationScrapDto()
        {
        }

        public string ClassificationCode { get; set; }
        public string ClassificationName { get; set; }
        public string UnitQtyName { get; set; }
        public double SaldoAwal { get; set; }
        public double Pemasukan { get; set; }
        public double Pengeluaran { get; set; }
        public double Penyesuaian { get; set; }
        public double StockOpname { get; set; }
        public double Selisih { get; set; }
        public double SaldoAkhir { get; set; }
        public GetMutationScrapDto(GetMutationScrapDto getMutationScrapDto)
        {
            ClassificationCode = getMutationScrapDto.ClassificationCode;
            ClassificationName = getMutationScrapDto.ClassificationName;
            UnitQtyName = getMutationScrapDto.UnitQtyName;
            SaldoAwal = getMutationScrapDto.SaldoAwal;
            Pemasukan = getMutationScrapDto.Pemasukan;
            Pengeluaran = getMutationScrapDto.Pengeluaran;
            Penyesuaian = getMutationScrapDto.Penyesuaian;
            StockOpname = getMutationScrapDto.StockOpname;
            Selisih = getMutationScrapDto.Selisih;
            SaldoAkhir = getMutationScrapDto.SaldoAkhir;
        }
        
    }
}
