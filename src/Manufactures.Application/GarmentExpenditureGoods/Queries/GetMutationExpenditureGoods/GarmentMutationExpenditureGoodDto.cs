using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods
{
    public class GarmentMutationExpenditureGoodDto
    {
        public GarmentMutationExpenditureGoodDto()
        {
        }

        public string KodeBarang { get; internal set; }
        public string NamaBarang { get; internal set; }
        public double Pemasukan { get; internal set; }
        public double Pengeluaran { get; internal set; }
        public double SaldoAwal { get; internal set; }
        public double SaldoBuku { get; internal set; }
        public string UnitQtyName { get; internal set; }
        public double Penyesuaian { get; internal set; }
        public double StockOpname { get; internal set; }
        public double Selisih { get; internal set; }
        public string Storage { get; internal set; }


        public GarmentMutationExpenditureGoodDto(GarmentMutationExpenditureGoodDto garmentMutation)
        {

            KodeBarang = garmentMutation.KodeBarang;
            NamaBarang = garmentMutation.NamaBarang;
            Pemasukan = garmentMutation.Pemasukan;
            Pengeluaran = garmentMutation.Pengeluaran;
            SaldoAwal = garmentMutation.SaldoAwal;
            SaldoBuku = garmentMutation.SaldoBuku;
            UnitQtyName = garmentMutation.UnitQtyName;
            Penyesuaian = garmentMutation.Penyesuaian;
            StockOpname = garmentMutation.StockOpname;
            Selisih = garmentMutation.Selisih;
            Storage = garmentMutation.Storage;
        }
    }
}
