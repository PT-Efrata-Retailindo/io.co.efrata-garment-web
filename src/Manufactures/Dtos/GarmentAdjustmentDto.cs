using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentAdjustmentDto : BaseDto
    {
        public GarmentAdjustmentDto(GarmentAdjustment garmentAdjustment)
        {
            Id = garmentAdjustment.Identity;
            AdjustmentNo = garmentAdjustment.AdjustmentNo;
            AdjustmentType = garmentAdjustment.AdjustmentType;
            RONo = garmentAdjustment.RONo;
            Article = garmentAdjustment.Article;
            Unit = new UnitDepartment(garmentAdjustment.UnitId.Value, garmentAdjustment.UnitCode, garmentAdjustment.UnitName);
            Comodity = new GarmentComodity(garmentAdjustment.ComodityId.Value, garmentAdjustment.ComodityCode, garmentAdjustment.ComodityName);
            AdjustmentDate = garmentAdjustment.AdjustmentDate;
            AdjustmentDesc = garmentAdjustment.AdjustmentDesc;

            Items = new List<GarmentAdjustmentItemDto>();
        }

        public Guid Id { get; internal set; }
        public string AdjustmentNo { get; internal set; }
        public string AdjustmentType { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset AdjustmentDate { get; internal set; }
        public string AdjustmentDesc { get; internal set; }

        public virtual List<GarmentAdjustmentItemDto> Items { get; internal set; }
    }
}
