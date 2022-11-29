using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentFinishingOutListDto : BaseDto
    {
        public GarmentFinishingOutListDto(GarmentFinishingOut garmentFinishingOutList)
        {
            Id = garmentFinishingOutList.Identity;
            FinishingOutNo = garmentFinishingOutList.FinishingOutNo;
            UnitTo = new UnitDepartment(garmentFinishingOutList.UnitToId.Value, garmentFinishingOutList.UnitToCode, garmentFinishingOutList.UnitToName);
            Unit = new UnitDepartment(garmentFinishingOutList.UnitId.Value, garmentFinishingOutList.UnitCode, garmentFinishingOutList.UnitName);
            RONo = garmentFinishingOutList.RONo;
            Article = garmentFinishingOutList.Article;
            FinishingOutDate = garmentFinishingOutList.FinishingOutDate;
            FinishingTo = garmentFinishingOutList.FinishingTo;
            CreatedBy = garmentFinishingOutList.AuditTrail.CreatedBy;
            Items = new List<GarmentFinishingOutItemDto>();
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
        public List<GarmentFinishingOutItemDto> Items { get; set; }
    }
}

