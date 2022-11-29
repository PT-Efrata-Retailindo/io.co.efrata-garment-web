using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleSewingOuts
{
    public class GarmentSampleSewingOutDetailDto : BaseDto
    {
        public GarmentSampleSewingOutDetailDto(GarmentSampleSewingOutDetail garmentSewingOutDetail)
        {
            Id = garmentSewingOutDetail.Identity;
            SewingOutItemId = garmentSewingOutDetail.SampleSewingOutItemId;
            Size = new SizeValueObject(garmentSewingOutDetail.SizeId.Value, garmentSewingOutDetail.SizeName);
            Quantity = garmentSewingOutDetail.Quantity;
            Uom = new Uom(garmentSewingOutDetail.UomId.Value, garmentSewingOutDetail.UomUnit);

        }

        public Guid Id { get; set; }
        public Guid SewingOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
    }
}
