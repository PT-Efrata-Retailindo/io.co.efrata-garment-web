using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;
using Manufactures.Domain.GarmentLoadings;

namespace Manufactures.Dtos
{
    public class GarmentLoadingListDto : BaseDto
    {
        public GarmentLoadingListDto(GarmentLoading garmentLoading)
        {
            Id = garmentLoading.Identity;
            LoadingNo = garmentLoading.LoadingNo;
            SewingDONo = garmentLoading.SewingDONo;
            RONo = garmentLoading.RONo;
            Article = garmentLoading.Article;
            Unit = new UnitDepartment(garmentLoading.UnitId.Value, garmentLoading.UnitCode, garmentLoading.UnitName);
            UnitFrom = new UnitDepartment(garmentLoading.UnitFromId.Value, garmentLoading.UnitFromCode, garmentLoading.UnitFromName);
            Comodity = new GarmentComodity (garmentLoading.ComodityId.Value, garmentLoading.ComodityCode, garmentLoading.ComodityName);
            LoadingDate = garmentLoading.LoadingDate;
            CreatedBy = garmentLoading.AuditTrail.CreatedBy;
        }

        public Guid Id { get; internal set; }
        public string LoadingNo { get; internal set; }

        public string SewingDONo { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset LoadingDate { get; internal set; }

        public double TotalRemainingQuantity { get; set; }
        public double TotalLoadingQuantity { get; set; }
        public List<string> Products { get; set; }
        public List<string> Colors { get; set; }
    }
}
