using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSewingInDto : BaseDto
    {
        public GarmentSewingInDto(GarmentSewingIn garmentSewingIn)
        {
            Id = garmentSewingIn.Identity;
            SewingInNo = garmentSewingIn.SewingInNo;
            SewingFrom = garmentSewingIn.SewingFrom;
            LoadingId = garmentSewingIn.LoadingId;
            LoadingNo = garmentSewingIn.LoadingNo;
            UnitFrom = new UnitDepartment(garmentSewingIn.UnitFromId.Value, garmentSewingIn.UnitFromCode, garmentSewingIn.UnitFromName);
            Unit = new UnitDepartment(garmentSewingIn.UnitId.Value, garmentSewingIn.UnitCode, garmentSewingIn.UnitName);
            RONo = garmentSewingIn.RONo;
            Article = garmentSewingIn.Article;
            Comodity = new GarmentComodity(garmentSewingIn.ComodityId.Value, garmentSewingIn.ComodityCode, garmentSewingIn.ComodityName);
            SewingInDate = garmentSewingIn.SewingInDate;

            Items = new List<GarmentSewingInItemDto>();
        }

        public Guid Id { get; set; }
        public string SewingInNo { get; set; }
        public string SewingFrom { get; set; }
        public Guid LoadingId { get; set; }
        public string LoadingNo { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset SewingInDate { get; set; }
        public List<GarmentSewingInItemDto> Items { get; set; }
    }
}