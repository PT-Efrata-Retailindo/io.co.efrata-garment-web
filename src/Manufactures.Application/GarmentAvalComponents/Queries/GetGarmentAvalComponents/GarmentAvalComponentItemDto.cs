using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.GarmentAvalComponents.Queries.GetGarmentAvalComponents
{
    public class GarmentAvalComponentItemDto
    {
        public Guid Id { get; set; }
        public Guid AvalComponentId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Guid SewingOutItemId { get; set; }
        public Guid SewingOutDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public string Color { get; set; }
        public double Quantity { get; set; }
        public double RemainingQuantity { get; set; }
        public SizeValueObject Size { get; set; }
        public decimal Price { get; set; }

        public GarmentAvalComponentItemDto(GarmentAvalComponentItem garmentAvalComponentItem)
        {
            Id = garmentAvalComponentItem.Identity;
            AvalComponentId = garmentAvalComponentItem.AvalComponentId;
            CuttingInDetailId = garmentAvalComponentItem.CuttingInDetailId;
            SewingOutItemId = garmentAvalComponentItem.SewingOutItemId;
            SewingOutDetailId = garmentAvalComponentItem.SewingOutDetailId;
            Product = new Product(garmentAvalComponentItem.ProductId.Value, garmentAvalComponentItem.ProductCode, garmentAvalComponentItem.ProductName);
            DesignColor = garmentAvalComponentItem.DesignColor;
            Color = garmentAvalComponentItem.Color;
            Quantity = garmentAvalComponentItem.Quantity;
            RemainingQuantity = garmentAvalComponentItem.RemainingQuantity;
            Size = garmentAvalComponentItem.SizeId.Value > 0 ? new SizeValueObject(garmentAvalComponentItem.SizeId.Value, garmentAvalComponentItem.SizeName) : null;
            Price = garmentAvalComponentItem.Price;
        }
    }
}