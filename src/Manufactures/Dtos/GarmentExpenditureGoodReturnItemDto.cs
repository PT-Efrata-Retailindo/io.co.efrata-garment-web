using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentExpenditureGoodReturnItemDto : BaseDto
    {
        public GarmentExpenditureGoodReturnItemDto(GarmentExpenditureGoodReturnItem garmentExpenditureGoodReturnItem)
        {
            Id = garmentExpenditureGoodReturnItem.Identity;
            ReturId = garmentExpenditureGoodReturnItem.ReturId;
            ExpenditureGoodId = garmentExpenditureGoodReturnItem.ExpenditureGoodId;
            ExpenditureGoodItemId = garmentExpenditureGoodReturnItem.ExpenditureGoodItemId;
            FinishedGoodStockId = garmentExpenditureGoodReturnItem.FinishedGoodStockId;
            Size = new SizeValueObject(garmentExpenditureGoodReturnItem.SizeId.Value, garmentExpenditureGoodReturnItem.SizeName);
            Quantity = garmentExpenditureGoodReturnItem.Quantity;
            Uom = new Uom(garmentExpenditureGoodReturnItem.UomId.Value, garmentExpenditureGoodReturnItem.UomUnit);
            Description = garmentExpenditureGoodReturnItem.Description;
            BasicPrice = garmentExpenditureGoodReturnItem.BasicPrice;
            Price = garmentExpenditureGoodReturnItem.Price;
        }

        public Guid Id { get; set; }
        public Guid ReturId { get; set; }
        public Guid ExpenditureGoodId { get; set; }
        public Guid ExpenditureGoodItemId { get; set; }
        public Guid FinishedGoodStockId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Description { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
