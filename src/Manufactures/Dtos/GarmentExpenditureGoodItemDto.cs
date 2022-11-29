using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentExpenditureGoodItemDto : BaseDto
    {
        public GarmentExpenditureGoodItemDto(GarmentExpenditureGoodItem garmentExpenditureGoodItem)
        {
            Id = garmentExpenditureGoodItem.Identity;
            ExpenditureGoodId = garmentExpenditureGoodItem.ExpenditureGoodId;
            FinishedGoodStockId = garmentExpenditureGoodItem.FinishedGoodStockId;
            Size = new SizeValueObject(garmentExpenditureGoodItem.SizeId.Value, garmentExpenditureGoodItem.SizeName);
            Quantity = garmentExpenditureGoodItem.Quantity;
            Uom = new Uom(garmentExpenditureGoodItem.UomId.Value, garmentExpenditureGoodItem.UomUnit);
            Description = garmentExpenditureGoodItem.Description;
            BasicPrice = garmentExpenditureGoodItem.BasicPrice;
            Price = garmentExpenditureGoodItem.Price;
            ReturQuantity = garmentExpenditureGoodItem.ReturQuantity;
        }

        public Guid Id { get; set; }
        public Guid ExpenditureGoodId { get; set; }
        public Guid FinishedGoodStockId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public double ReturQuantity { get; set; }
        public Uom Uom { get; set; }
        public string Description { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
