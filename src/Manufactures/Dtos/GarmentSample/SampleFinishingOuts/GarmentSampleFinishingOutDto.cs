using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleFinishingOuts
{
    public class GarmentSampleFinishingOutDto : BaseDto
    {
        public GarmentSampleFinishingOutDto(GarmentSampleFinishingOut GarmentSampleFinishingOutList)
        {
            Id = GarmentSampleFinishingOutList.Identity;
            FinishingOutNo = GarmentSampleFinishingOutList.FinishingOutNo;
            UnitTo = new UnitDepartment(GarmentSampleFinishingOutList.UnitToId.Value, GarmentSampleFinishingOutList.UnitToCode, GarmentSampleFinishingOutList.UnitToName);
            Unit = new UnitDepartment(GarmentSampleFinishingOutList.UnitId.Value, GarmentSampleFinishingOutList.UnitCode, GarmentSampleFinishingOutList.UnitName);
            RONo = GarmentSampleFinishingOutList.RONo;
            Article = GarmentSampleFinishingOutList.Article;
            FinishingOutDate = GarmentSampleFinishingOutList.FinishingOutDate;
            FinishingTo = GarmentSampleFinishingOutList.FinishingTo;
            Comodity = new GarmentComodity(GarmentSampleFinishingOutList.ComodityId.Value, GarmentSampleFinishingOutList.ComodityCode, GarmentSampleFinishingOutList.ComodityName);
            IsDifferentSize = GarmentSampleFinishingOutList.IsDifferentSize;

            Items = new List<GarmentSampleFinishingOutItemDto>();
        }

        public Guid Id { get; internal set; }
        public string FinishingOutNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string FinishingTo { get; set; }
        public UnitDepartment UnitTo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset FinishingOutDate { get; set; }
        public bool IsDifferentSize { get; set; }

        public virtual List<GarmentSampleFinishingOutItemDto> Items { get; internal set; }
    }
}

