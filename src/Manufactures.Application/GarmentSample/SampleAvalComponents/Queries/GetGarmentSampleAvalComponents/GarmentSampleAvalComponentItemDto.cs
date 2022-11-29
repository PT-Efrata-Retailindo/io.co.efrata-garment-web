using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetGarmentSampleAvalComponents
{
    public class GarmentSampleAvalComponentItemDto
    {
        public Guid Id { get; set; }
        public Guid SampleAvalComponentId { get; set; }
        public Guid SampleCuttingInDetailId { get; set; }
        public Guid SampleSewingOutItemId { get; set; }
        public Guid SampleSewingOutDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public string Color { get; set; }
        public double Quantity { get; set; }
        public double RemainingQuantity { get; set; }
        public SizeValueObject Size { get; set; }
        public decimal Price { get; set; }

        public GarmentSampleAvalComponentItemDto(GarmentSampleAvalComponentItem garmentSampleAvalComponentItem)
        {
            Id = garmentSampleAvalComponentItem.Identity;
            SampleAvalComponentId = garmentSampleAvalComponentItem.SampleAvalComponentId;
            SampleCuttingInDetailId = garmentSampleAvalComponentItem.SampleCuttingInDetailId;
            SampleSewingOutItemId = garmentSampleAvalComponentItem.SampleSewingOutItemId;
            SampleSewingOutDetailId = garmentSampleAvalComponentItem.SampleSewingOutDetailId;
            Product = new Product(garmentSampleAvalComponentItem.ProductId.Value, garmentSampleAvalComponentItem.ProductCode, garmentSampleAvalComponentItem.ProductName);
            DesignColor = garmentSampleAvalComponentItem.DesignColor;
            Color = garmentSampleAvalComponentItem.Color;
            Quantity = garmentSampleAvalComponentItem.Quantity;
            RemainingQuantity = garmentSampleAvalComponentItem.RemainingQuantity;
            Size = garmentSampleAvalComponentItem.SizeId.Value > 0 ? new SizeValueObject(garmentSampleAvalComponentItem.SizeId.Value, garmentSampleAvalComponentItem.SizeName) : null;
            Price = garmentSampleAvalComponentItem.Price;
        }
    }
}
