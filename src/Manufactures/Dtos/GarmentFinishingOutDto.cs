using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentFinishingOutDto : BaseDto
    {
        public GarmentFinishingOutDto(GarmentFinishingOut garmentFinishingOutList)
        {
            Id = garmentFinishingOutList.Identity;
            FinishingOutNo = garmentFinishingOutList.FinishingOutNo;
            UnitTo = new UnitDepartment(garmentFinishingOutList.UnitToId.Value, garmentFinishingOutList.UnitToCode, garmentFinishingOutList.UnitToName);
            Unit = new UnitDepartment(garmentFinishingOutList.UnitId.Value, garmentFinishingOutList.UnitCode, garmentFinishingOutList.UnitName);
            RONo = garmentFinishingOutList.RONo;
            Article = garmentFinishingOutList.Article;
            FinishingOutDate = garmentFinishingOutList.FinishingOutDate;
            FinishingTo = garmentFinishingOutList.FinishingTo;
            Comodity = new GarmentComodity(garmentFinishingOutList.ComodityId.Value, garmentFinishingOutList.ComodityCode, garmentFinishingOutList.ComodityName);
            IsDifferentSize = garmentFinishingOutList.IsDifferentSize;

            Items = new List<GarmentFinishingOutItemDto>();
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

        public virtual List<GarmentFinishingOutItemDto> Items { get; internal set; }
    }
}
