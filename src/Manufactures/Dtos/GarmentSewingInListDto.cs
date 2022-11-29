using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSewingInListDto : BaseDto
    {
        public GarmentSewingInListDto(GarmentSewingIn garmentSewingInList)
        {
            Id = garmentSewingInList.Identity;
            SewingInNo = garmentSewingInList.SewingInNo;
            SewingFrom = garmentSewingInList.SewingFrom;
            LoadingId = garmentSewingInList.LoadingId;
            LoadingNo = garmentSewingInList.LoadingNo;
            UnitFrom = new UnitDepartment(garmentSewingInList.UnitFromId.Value, garmentSewingInList.UnitFromCode, garmentSewingInList.UnitFromName);
            Unit = new UnitDepartment(garmentSewingInList.UnitId.Value, garmentSewingInList.UnitCode, garmentSewingInList.UnitName);
            RONo = garmentSewingInList.RONo;
            Article = garmentSewingInList.Article;
            Comodity = new GarmentComodity(garmentSewingInList.ComodityId.Value, garmentSewingInList.ComodityCode, garmentSewingInList.ComodityName);
            SewingInDate = garmentSewingInList.SewingInDate;
            CreatedBy = garmentSewingInList.AuditTrail.CreatedBy;
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
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public List<string> Products { get; set; }
        public List<GarmentSewingInItemDto> Items { get; set; }
    }
}