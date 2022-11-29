using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleSewingOuts
{
    public class GarmentSampleSewingOutItemDto : BaseDto
    {
        public GarmentSampleSewingOutItemDto(GarmentSampleSewingOutItem garmentSewingOutItem)
        {
            Id = garmentSewingOutItem.Identity;
            SewingOutId = garmentSewingOutItem.SampleSewingOutId;
            SewingInId = garmentSewingOutItem.SampleSewingInId;
            SewingInItemId = garmentSewingOutItem.SampleSewingInItemId;
            Product = new Product(garmentSewingOutItem.ProductId.Value, garmentSewingOutItem.ProductCode, garmentSewingOutItem.ProductName);
            Size = new SizeValueObject(garmentSewingOutItem.SizeId.Value, garmentSewingOutItem.SizeName);
            DesignColor = garmentSewingOutItem.DesignColor;
            Quantity = garmentSewingOutItem.Quantity;
            Uom = new Uom(garmentSewingOutItem.UomId.Value, garmentSewingOutItem.UomUnit);
            Color = garmentSewingOutItem.Color;
            RemainingQuantity = garmentSewingOutItem.RemainingQuantity;
            BasicPrice = garmentSewingOutItem.BasicPrice;
            Price = garmentSewingOutItem.Price;

            Details = new List<GarmentSampleSewingOutDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid SewingOutId { get; set; }
        public Guid SewingInId { get; set; }
        public Guid SewingInItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
        public List<GarmentSampleSewingOutDetailDto> Details { get; set; }
    }
}
