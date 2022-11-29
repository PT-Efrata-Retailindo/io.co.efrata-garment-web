using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentLoadingItemDto : BaseDto
    {
        public GarmentLoadingItemDto(GarmentLoadingItem garmentLoadingItem)
        {
            Id = garmentLoadingItem.Identity;
            Product = new Product(garmentLoadingItem.ProductId.Value, garmentLoadingItem.ProductCode, garmentLoadingItem.ProductName);
            DesignColor = garmentLoadingItem.DesignColor;
            Size= new SizeValueObject(garmentLoadingItem.SizeId.Value, garmentLoadingItem.SizeName);
            Quantity = garmentLoadingItem.Quantity;
            Uom = new Uom(garmentLoadingItem.UomId.Value, garmentLoadingItem.UomUnit);
            Color = garmentLoadingItem.Color;
            RemainingQuantity = garmentLoadingItem.RemainingQuantity;
            BasicPrice = garmentLoadingItem.BasicPrice;
            SewingDOItemId = garmentLoadingItem.SewingDOItemId;
            LoadingId = garmentLoadingItem.LoadingId;
            Price = garmentLoadingItem.Price;
        }

        public Guid Id { get; set; }
        public Guid SewingDOItemId { get; set; }

        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public Guid LoadingId { get; set; }
        public double Price { get; set; }
    }
}
