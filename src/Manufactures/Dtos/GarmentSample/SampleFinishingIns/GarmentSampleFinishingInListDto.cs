using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleFinishingIns
{
    public class GarmentSampleFinishingInListDto : BaseDto
    {
        public GarmentSampleFinishingInListDto(GarmentSampleFinishingIn garmentFinishingIn)
        {
            Id = garmentFinishingIn.Identity;
            FinishingInNo = garmentFinishingIn.FinishingInNo;
            RONo = garmentFinishingIn.RONo;
            Article = garmentFinishingIn.Article;
            Unit = new UnitDepartment(garmentFinishingIn.UnitId.Value, garmentFinishingIn.UnitCode, garmentFinishingIn.UnitName);
            UnitFrom = new UnitDepartment(garmentFinishingIn.UnitFromId.Value, garmentFinishingIn.UnitFromCode, garmentFinishingIn.UnitFromName);
            FinishingInDate = garmentFinishingIn.FinishingInDate;
            FinishingInType = garmentFinishingIn.FinishingInType;
            Comodity = new GarmentComodity(garmentFinishingIn.ComodityId.Value, garmentFinishingIn.ComodityCode, garmentFinishingIn.ComodityName);
            CreatedBy = garmentFinishingIn.AuditTrail.CreatedBy;
        }

        public Guid Id { get; internal set; }
        public string FinishingInNo { get; internal set; }
        public string FinishingInType { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public DateTimeOffset FinishingInDate { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }

        public double TotalRemainingQuantity { get; set; }
        public double TotalFinishingInQuantity { get; set; }

        public List<string> Products { get; set; }
    }
}
