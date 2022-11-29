using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentFinishedGoodStockDto
    {
        public GarmentFinishedGoodStockDto(GarmentFinishedGoodStock finishedGoodStock)
        {
            Id = finishedGoodStock.Identity;
            FinishedGoodStockNo = finishedGoodStock.FinishedGoodStockNo;
            RONo = finishedGoodStock.RONo;
            Article = finishedGoodStock.Article;
            Unit = new UnitDepartment( finishedGoodStock.UnitId.Value, finishedGoodStock.UnitCode, finishedGoodStock.UnitName);
            Comodity = new GarmentComodity (finishedGoodStock.ComodityId.Value, finishedGoodStock.ComodityCode, finishedGoodStock.ComodityName);
            Size = new SizeValueObject(finishedGoodStock.SizeId.Value, finishedGoodStock.SizeName);
            Quantity = finishedGoodStock.Quantity;
            Uom = new Uom( finishedGoodStock.UomId.Value, finishedGoodStock.UomUnit) ;
            BasicPrice = finishedGoodStock.BasicPrice;
            Price = finishedGoodStock.Price;
        }
        public Guid Id { get; internal set; }
        public string FinishedGoodStockNo { get; private set; }
        public UnitDepartment Unit { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodity Comodity { get; private set; }
        public SizeValueObject Size { get; private set; }
        public double Quantity { get; private set; }
        public Uom Uom { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }
    }
}
