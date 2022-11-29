using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleFinishingOuts
{
    public class GarmentSampleFinishingOutListDto : BaseDto
    {
        public GarmentSampleFinishingOutListDto(GarmentSampleFinishingOut GarmentSampleFinishingOutList)
        {
            Id = GarmentSampleFinishingOutList.Identity;
            FinishingOutNo = GarmentSampleFinishingOutList.FinishingOutNo;
            UnitTo = new UnitDepartment(GarmentSampleFinishingOutList.UnitToId.Value, GarmentSampleFinishingOutList.UnitToCode, GarmentSampleFinishingOutList.UnitToName);
            Unit = new UnitDepartment(GarmentSampleFinishingOutList.UnitId.Value, GarmentSampleFinishingOutList.UnitCode, GarmentSampleFinishingOutList.UnitName);
            RONo = GarmentSampleFinishingOutList.RONo;
            Article = GarmentSampleFinishingOutList.Article;
            FinishingOutDate = GarmentSampleFinishingOutList.FinishingOutDate;
            FinishingTo = GarmentSampleFinishingOutList.FinishingTo;
            CreatedBy = GarmentSampleFinishingOutList.AuditTrail.CreatedBy;
            Items = new List<GarmentSampleFinishingOutItemDto>();
        }

        public Guid Id { get; set; }
        public string FinishingOutNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string FinishingTo { get; set; }
        public UnitDepartment UnitTo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset FinishingOutDate { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Products { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public List<GarmentSampleFinishingOutItemDto> Items { get; set; }
    }
}
