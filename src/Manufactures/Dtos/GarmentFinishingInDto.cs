using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentFinishingInDto : BaseDto
    {
        public GarmentFinishingInDto(GarmentFinishingIn garmentFinishingIn)
        {
            Id = garmentFinishingIn.Identity;
            FinishingInNo = garmentFinishingIn.FinishingInNo;
            RONo = garmentFinishingIn.RONo;
            Article = garmentFinishingIn.Article;
            Unit = new UnitDepartment(garmentFinishingIn.UnitId.Value, garmentFinishingIn.UnitCode, garmentFinishingIn.UnitName);
            UnitFrom = new UnitDepartment(garmentFinishingIn.UnitFromId.Value, garmentFinishingIn.UnitFromCode, garmentFinishingIn.UnitFromName);
            Comodity = new GarmentComodity(garmentFinishingIn.ComodityId.Value, garmentFinishingIn.ComodityCode, garmentFinishingIn.ComodityName);
            FinishingInDate = garmentFinishingIn.FinishingInDate;
            FinishingInType = garmentFinishingIn.FinishingInType;
            Comodity = new GarmentComodity(garmentFinishingIn.ComodityId.Value, garmentFinishingIn.ComodityCode, garmentFinishingIn.ComodityName);
            DOId = garmentFinishingIn.DOId;
            DONo = garmentFinishingIn.DONo;
            SubconType = garmentFinishingIn.SubconType;
            Items = new List<GarmentFinishingInItemDto>();
        }

        public Guid Id { get; internal set; }
        public string FinishingInNo { get; internal set; }
        public string FinishingInType { get; internal set; }

        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset FinishingInDate { get; internal set; }
        public long DOId { get; internal set; }
        public string DONo { get; internal set; }
        public string SubconType { get; internal set; }

        public virtual List<GarmentFinishingInItemDto> Items { get; internal set; }
    }
}
