using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentAdjustmentItemDto : BaseDto
    {
        public GarmentAdjustmentItemDto(GarmentAdjustmentItem garmentAdjustmentItem)
        {
            Id = garmentAdjustmentItem.Identity;
            Product = new Product(garmentAdjustmentItem.ProductId.Value, garmentAdjustmentItem.ProductCode, garmentAdjustmentItem.ProductName);
            DesignColor = garmentAdjustmentItem.DesignColor;
            Size = new SizeValueObject(garmentAdjustmentItem.SizeId.Value, garmentAdjustmentItem.SizeName);
            Quantity = garmentAdjustmentItem.Quantity;
            Uom = new Uom(garmentAdjustmentItem.UomId.Value, garmentAdjustmentItem.UomUnit);
            Color = garmentAdjustmentItem.Color;
            BasicPrice = garmentAdjustmentItem.BasicPrice;
            SewingDOItemId = garmentAdjustmentItem.SewingDOItemId;
            AdjustmentId = garmentAdjustmentItem.AdjustmentId;
            Price = garmentAdjustmentItem.Price;
			FinishedGoodStockId = garmentAdjustmentItem.FinishedGoodStockId;
        }

        public Guid Id { get; set; }
        public Guid SewingDOItemId { get; set; }

        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double BasicPrice { get; set; }
        public Guid AdjustmentId { get; set; }
		public Guid FinishedGoodStockId { get; set; }
		public double Price { get; set; }
    }
}
