using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleCuttingOuts
{
    public class GarmentSampleCuttingOutDetailDto : BaseDto
    {
        public GarmentSampleCuttingOutDetailDto(GarmentSampleCuttingOutDetail GarmentSampleCuttingOutDetail)
        {
            Id = GarmentSampleCuttingOutDetail.Identity;
            CuttingOutItemId = GarmentSampleCuttingOutDetail.CuttingOutItemId;
            Size = new SizeValueObject(GarmentSampleCuttingOutDetail.SizeId.Value, GarmentSampleCuttingOutDetail.SizeName);
            CuttingOutQuantity = GarmentSampleCuttingOutDetail.CuttingOutQuantity;
            CuttingOutUom = new Uom(GarmentSampleCuttingOutDetail.CuttingOutUomId.Value, GarmentSampleCuttingOutDetail.CuttingOutUomUnit);
            Color = GarmentSampleCuttingOutDetail.Color;
            RemainingQuantity = GarmentSampleCuttingOutDetail.RemainingQuantity;
            BasicPrice = GarmentSampleCuttingOutDetail.BasicPrice;
            Price = GarmentSampleCuttingOutDetail.Price;

        }

        public Guid Id { get; set; }
        public Guid CuttingOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double CuttingOutQuantity { get; set; }
        public Uom CuttingOutUom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
