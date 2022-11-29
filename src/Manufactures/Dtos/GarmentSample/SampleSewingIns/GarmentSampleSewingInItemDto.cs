using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleSewingIns
{
    public class GarmentSampleSewingInItemDto : BaseDto
    {

        public GarmentSampleSewingInItemDto(GarmentSampleSewingInItem garmentSewingInItemReadModel)
        {
            Id = garmentSewingInItemReadModel.Identity;
            SewingInId = garmentSewingInItemReadModel.SewingInId;
            CuttingOutDetailId = garmentSewingInItemReadModel.CuttingOutDetailId;
            CuttingOutItemId = garmentSewingInItemReadModel.CuttingOutItemId;
            Product = new Product(garmentSewingInItemReadModel.ProductId.Value, garmentSewingInItemReadModel.ProductCode, garmentSewingInItemReadModel.ProductName);
            DesignColor = garmentSewingInItemReadModel.DesignColor;
            Size = new SizeValueObject(garmentSewingInItemReadModel.SizeId.Value, garmentSewingInItemReadModel.SizeName);
            Quantity = garmentSewingInItemReadModel.Quantity;
            Uom = new Uom(garmentSewingInItemReadModel.UomId.Value, garmentSewingInItemReadModel.UomUnit);
            Color = garmentSewingInItemReadModel.Color;
            RemainingQuantity = garmentSewingInItemReadModel.RemainingQuantity;
            BasicPrice = garmentSewingInItemReadModel.BasicPrice;
            Price = garmentSewingInItemReadModel.Price;
        }


        public Guid Id { get; set; }
        public Guid SewingInId { get; set; }
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
