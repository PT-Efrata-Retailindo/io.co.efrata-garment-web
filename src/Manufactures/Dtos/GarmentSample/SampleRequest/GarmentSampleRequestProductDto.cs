using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleRequest
{
    public class GarmentSampleRequestProductDto : BaseDto
    {
        public GarmentSampleRequestProductDto(GarmentSampleRequestProduct garmentSampleRequestProduct)
        {
            Id = garmentSampleRequestProduct.Identity;
            SampleRequestId = garmentSampleRequestProduct.SampleRequestId;
            Style = garmentSampleRequestProduct.Style;
            Color = garmentSampleRequestProduct.Color;
            Fabric = garmentSampleRequestProduct.Fabric;
            Size = new SizeValueObject(garmentSampleRequestProduct.SizeId.Value, garmentSampleRequestProduct.SizeName);
            SizeDescription = garmentSampleRequestProduct.SizeDescription;
            Quantity = garmentSampleRequestProduct.Quantity;
            Index = garmentSampleRequestProduct.Index;
        }
        public Guid Id { get; set; }
        public Guid SampleRequestId { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }

        public string Fabric { get; set; }

        public SizeValueObject Size { get; set; }

        public string SizeDescription { get; set; }
        public double Quantity { get; set; }
        public int Index { get; set; }
    }
}
