using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SamplePreparings
{
    public class GarmentSamplePreparingDto
    {
        public GarmentSamplePreparingDto()
        {
            Items = new List<GarmentSamplePreparingItemDto>();
        }

        public GarmentSamplePreparingDto(GarmentSamplePreparing garmentSamplePreparing)
        {
            Id = garmentSamplePreparing.Identity;

            LastModifiedDate = garmentSamplePreparing.AuditTrail.ModifiedDate ?? garmentSamplePreparing.AuditTrail.CreatedDate;
            LastModifiedBy = garmentSamplePreparing.AuditTrail.ModifiedBy ?? garmentSamplePreparing.AuditTrail.CreatedBy;

            UENId = garmentSamplePreparing.UENId;
            UENNo = garmentSamplePreparing.UENNo;
            Unit = new UnitDepartment(garmentSamplePreparing.UnitId.Value, garmentSamplePreparing.UnitName, garmentSamplePreparing.UnitCode);
            ProcessDate = garmentSamplePreparing.ProcessDate;
            RONo = garmentSamplePreparing.RONo;
            Article = garmentSamplePreparing.Article;
            IsCuttingIn = garmentSamplePreparing.IsCuttingIn;
            CreatedBy = garmentSamplePreparing.AuditTrail.CreatedBy;
            Buyer = new Domain.Shared.ValueObjects.Buyer(garmentSamplePreparing.BuyerId.Value, garmentSamplePreparing.BuyerCode, garmentSamplePreparing.BuyerName);
        }

        public Guid Id { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
        public int UENId { get; set; }
        public string UENNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public bool IsCuttingIn { get; set; }
        public string CreatedBy { get; set; }
        public decimal TotalQuantity { get; set; }
        public Domain.Shared.ValueObjects.Buyer Buyer { get; set; }
        public List<GarmentSamplePreparingItemDto> Items { get; set; }
    }
}
