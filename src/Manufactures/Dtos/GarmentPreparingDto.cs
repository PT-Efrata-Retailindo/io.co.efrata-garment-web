using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos
{
    public class GarmentPreparingDto
    {
        public GarmentPreparingDto()
        {
            Items = new List<GarmentPreparingItemDto>();
        }

        public GarmentPreparingDto(GarmentPreparing garmentPreparing)
        {
            Id = garmentPreparing.Identity;

            LastModifiedDate = garmentPreparing.AuditTrail.ModifiedDate ?? garmentPreparing.AuditTrail.CreatedDate;
            LastModifiedBy = garmentPreparing.AuditTrail.ModifiedBy ?? garmentPreparing.AuditTrail.CreatedBy;

            UENId = garmentPreparing.UENId;
            UENNo = garmentPreparing.UENNo;
            Unit = new UnitDepartment(garmentPreparing.UnitId.Value, garmentPreparing.UnitName, garmentPreparing.UnitCode);
            ProcessDate = garmentPreparing.ProcessDate;
            RONo = garmentPreparing.RONo;
            Article = garmentPreparing.Article;
            IsCuttingIn = garmentPreparing.IsCuttingIn;
            CreatedBy = garmentPreparing.AuditTrail.CreatedBy;
            Buyer = new Domain.Shared.ValueObjects.Buyer(garmentPreparing.BuyerId.Value, garmentPreparing.BuyerCode, garmentPreparing.BuyerName);

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
        public List<GarmentPreparingItemDto> Items { get; set; }
    }
}