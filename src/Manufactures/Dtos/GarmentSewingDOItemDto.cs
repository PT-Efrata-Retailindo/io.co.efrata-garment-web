using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSewingDOItemDto : BaseDto
    {
        public GarmentSewingDOItemDto(GarmentSewingDOItem garmentSewingDOItem)
        {
            Id = garmentSewingDOItem.Identity;
            SewingDOId = garmentSewingDOItem.SewingDOId;
            CuttingOutDetailId = garmentSewingDOItem.CuttingOutDetailId;
            CuttingOutItemId = garmentSewingDOItem.CuttingOutItemId;
            Product = new Product(garmentSewingDOItem.ProductId.Value, garmentSewingDOItem.ProductCode, garmentSewingDOItem.ProductName);
            DesignColor = garmentSewingDOItem.DesignColor;
            Size = new SizeValueObject(garmentSewingDOItem.SizeId.Value, garmentSewingDOItem.SizeName);
            Quantity = garmentSewingDOItem.Quantity;
            Uom = new Uom(garmentSewingDOItem.UomId.Value, garmentSewingDOItem.UomUnit);
            Color = garmentSewingDOItem.Color;
            RemainingQuantity = garmentSewingDOItem.RemainingQuantity;
            BasicPrice = garmentSewingDOItem.BasicPrice;
            Price = garmentSewingDOItem.Price;


        }

        public Guid Id { get; set; }
        public Guid SewingDOId { get; set; }
        public Guid CuttingOutDetailId { get; set; }
        public Guid CuttingOutItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}