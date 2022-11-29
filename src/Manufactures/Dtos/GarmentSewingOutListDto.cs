using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSewingOutListDto : BaseDto
    {
        public GarmentSewingOutListDto(GarmentSewingOut garmentSewingOutList)
        {
            Id = garmentSewingOutList.Identity;
            SewingOutNo = garmentSewingOutList.SewingOutNo;
            UnitTo = new UnitDepartment(garmentSewingOutList.UnitToId.Value, garmentSewingOutList.UnitToCode, garmentSewingOutList.UnitToName);
            Unit = new UnitDepartment(garmentSewingOutList.UnitId.Value, garmentSewingOutList.UnitCode, garmentSewingOutList.UnitName);
            RONo = garmentSewingOutList.RONo;
            Article = garmentSewingOutList.Article;
            SewingOutDate = garmentSewingOutList.SewingOutDate;
            SewingTo = garmentSewingOutList.SewingTo;
            CreatedBy = garmentSewingOutList.AuditTrail.CreatedBy;
            Items = new List<GarmentSewingOutItemDto>();
        }

        public Guid Id { get; set; }
        public string SewingOutNo { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string SewingTo { get;  set; }
        public UnitDepartment UnitTo { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public DateTimeOffset SewingOutDate { get;  set; }
        public List<string> Colors { get; set; }
        public List<string> Products { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public List<GarmentSewingOutItemDto> Items { get; set; }
    }
}

