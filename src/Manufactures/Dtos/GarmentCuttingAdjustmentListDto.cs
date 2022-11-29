using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentCuttingAdjustments;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos
{
    public class GarmentCuttingAdjustmentListDto
    {
        public GarmentCuttingAdjustmentListDto(GarmentCuttingAdjustment garmentCuttingAdjustment)
        {
            Id = garmentCuttingAdjustment.Identity;
            CutInNo = garmentCuttingAdjustment.CutInNo;
            AdjustmentNo = garmentCuttingAdjustment.AdjustmentNo;
            RONo = garmentCuttingAdjustment.RONo;
            CutInId = garmentCuttingAdjustment.CutInId;
            Unit = new UnitDepartment(garmentCuttingAdjustment.UnitId.Value, garmentCuttingAdjustment.UnitCode, garmentCuttingAdjustment.UnitName);
            AdjustmentDate = garmentCuttingAdjustment.AdjustmentDate;
            TotalFC = garmentCuttingAdjustment.TotalFC;
            TotalActualFC = garmentCuttingAdjustment.TotalActualFC;
            TotalQuantity = garmentCuttingAdjustment.TotalQuantity;
            TotalActualQuantity = garmentCuttingAdjustment.TotalActualQuantity;
            CreatedBy = garmentCuttingAdjustment.AuditTrail.CreatedBy;
        }

        public Guid Id { get; set; }

        public string AdjustmentNo { get; set; }
        public string CutInNo { get; set; }
        public Guid CutInId { get; set; }
        public string RONo { get; set; }
        public decimal TotalFC { get; set; }
        public decimal TotalActualFC { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalActualQuantity { get; set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset AdjustmentDate { get; set; }
        public string CreatedBy { get; set; }

    }
}