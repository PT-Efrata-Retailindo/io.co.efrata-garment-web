using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingDto : BaseDto
    {
        public GarmentServiceSubconCuttingDto(GarmentServiceSubconCutting garmentServiceSubconCutting)
        {
            Id = garmentServiceSubconCutting.Identity;
            SubconNo = garmentServiceSubconCutting.SubconNo;
            SubconType = garmentServiceSubconCutting.SubconType;
            SubconDate = garmentServiceSubconCutting.SubconDate;
            Unit = new UnitDepartment(garmentServiceSubconCutting.UnitId.Value, garmentServiceSubconCutting.UnitCode, garmentServiceSubconCutting.UnitName);
            CreatedBy = garmentServiceSubconCutting.AuditTrail.CreatedBy;
            IsUsed = garmentServiceSubconCutting.IsUsed;
            Buyer = new Buyer(garmentServiceSubconCutting.BuyerId.Value, garmentServiceSubconCutting.BuyerCode, garmentServiceSubconCutting.BuyerName);
            Uom = new Uom(garmentServiceSubconCutting.UomId.Value, garmentServiceSubconCutting.UomUnit);
            QtyPacking = garmentServiceSubconCutting.QtyPacking;
            Items = new List<GarmentServiceSubconCuttingItemDto>();
        }

        public Guid Id { get; set; }
        public string SubconNo { get; set; }
        public string SubconType { get; set; }

        public DateTimeOffset SubconDate { get; set; }
        public UnitDepartment Unit { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public Uom Uom { get; set; }
        public int QtyPacking {get; set;}
        public List<GarmentServiceSubconCuttingItemDto> Items { get; set; }
    }
}