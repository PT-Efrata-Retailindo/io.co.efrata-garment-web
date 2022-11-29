using Manufactures.Domain.GarmentSample.SampleRequests;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleRequest
{
    public class GarmentSampleRequestSpecificationDto : BaseDto
    {
        public GarmentSampleRequestSpecificationDto(GarmentSampleRequestSpecification garmentSampleRequestSpecification)
        {
            Id = garmentSampleRequestSpecification.Identity;
            SampleRequestId = garmentSampleRequestSpecification.SampleRequestId;
            Inventory = garmentSampleRequestSpecification.Inventory;
            Inventory = garmentSampleRequestSpecification.Inventory;
            SpecificationDetail = garmentSampleRequestSpecification.SpecificationDetail;
            Quantity = garmentSampleRequestSpecification.Quantity;
            Remark = garmentSampleRequestSpecification.Remark;
            Index = garmentSampleRequestSpecification.Index;
            Uom = new Uom(garmentSampleRequestSpecification.UomId.Value, garmentSampleRequestSpecification.UomUnit);
        }
        public Guid Id { get; set; }
        public Guid SampleRequestId { get; set; }
        public string Inventory { get; set; }
        public string SpecificationDetail { get; set; }
        public double Quantity { get; set; }
        public string Remark { get; set; }
        public Uom Uom { get; set; }
        public int Index { get; set; }
    }
}
