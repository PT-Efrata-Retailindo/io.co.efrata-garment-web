using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleFinishingOuts
{
    public class GarmentSampleFinishingOutItemDto : BaseDto
    {
        public GarmentSampleFinishingOutItemDto(GarmentSampleFinishingOutItem garmentSampleFinishingOutItem)
        {
            Id = garmentSampleFinishingOutItem.Identity;
            FinishingOutId = garmentSampleFinishingOutItem.FinishingOutId;
            FinishingInId = garmentSampleFinishingOutItem.FinishingInId;
            FinishingInItemId = garmentSampleFinishingOutItem.FinishingInItemId;
            Product = new Product(garmentSampleFinishingOutItem.ProductId.Value, garmentSampleFinishingOutItem.ProductCode, garmentSampleFinishingOutItem.ProductName);
            Size = new SizeValueObject(garmentSampleFinishingOutItem.SizeId.Value, garmentSampleFinishingOutItem.SizeName);
            DesignColor = garmentSampleFinishingOutItem.DesignColor;
            Quantity = garmentSampleFinishingOutItem.Quantity;
            Uom = new Uom(garmentSampleFinishingOutItem.UomId.Value, garmentSampleFinishingOutItem.UomUnit);
            Color = garmentSampleFinishingOutItem.Color;
            RemainingQuantity = garmentSampleFinishingOutItem.RemainingQuantity;
            BasicPrice = garmentSampleFinishingOutItem.BasicPrice;
            Price = garmentSampleFinishingOutItem.Price;

            Details = new List<GarmentSampleFinishingOutDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid FinishingOutId { get; set; }
        public Guid FinishingInId { get; set; }
        public Guid FinishingInItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
        public List<GarmentSampleFinishingOutDetailDto> Details { get; set; }
    }
}
