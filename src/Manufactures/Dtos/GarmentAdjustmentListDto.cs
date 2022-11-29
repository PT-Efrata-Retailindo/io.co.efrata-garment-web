using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentAdjustmentListDto : BaseDto
    {
        public GarmentAdjustmentListDto(GarmentAdjustment garmentAdjustment)
        {
            Id = garmentAdjustment.Identity;
            AdjustmentNo = garmentAdjustment.AdjustmentNo;
            RONo = garmentAdjustment.RONo;
            Article = garmentAdjustment.Article;
            Unit = new UnitDepartment(garmentAdjustment.UnitId.Value, garmentAdjustment.UnitCode, garmentAdjustment.UnitName);
            Comodity = new GarmentComodity(garmentAdjustment.ComodityId.Value, garmentAdjustment.ComodityCode, garmentAdjustment.ComodityName);
            AdjustmentDate = garmentAdjustment.AdjustmentDate;
            CreatedBy = garmentAdjustment.AuditTrail.CreatedBy;
        }

        public Guid Id { get; internal set; }
        public string AdjustmentNo { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset AdjustmentDate { get; internal set; }

        public double TotalRemainingQuantity { get; set; }
        public double TotalAdjustmentQuantity { get; set; }
    }
}
