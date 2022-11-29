using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentLoadingDto : BaseDto
    {
        public GarmentLoadingDto(GarmentLoading garmentLoading)
        {
            Id = garmentLoading.Identity;
            LoadingNo = garmentLoading.LoadingNo;
            SewingDOId = garmentLoading.SewingDOId;
            SewingDONo = garmentLoading.SewingDONo;
            RONo = garmentLoading.RONo;
            Article = garmentLoading.Article;
            Unit = new UnitDepartment(garmentLoading.UnitId.Value, garmentLoading.UnitCode, garmentLoading.UnitName);
            UnitFrom = new UnitDepartment(garmentLoading.UnitFromId.Value, garmentLoading.UnitFromCode, garmentLoading.UnitFromName);
            Comodity = new GarmentComodity(garmentLoading.ComodityId.Value, garmentLoading.ComodityCode, garmentLoading.ComodityName);
            LoadingDate = garmentLoading.LoadingDate;

            Items = new List<GarmentLoadingItemDto>();
        }

        public Guid Id { get; internal set; }
        public string LoadingNo { get; internal set; }

        public Guid SewingDOId { get; internal set; }
        public string SewingDONo { get; internal set; }
        public UnitDepartment UnitFrom { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }
        public DateTimeOffset LoadingDate { get; internal set; }

        public virtual List<GarmentLoadingItemDto> Items { get; internal set; }
    }
}
