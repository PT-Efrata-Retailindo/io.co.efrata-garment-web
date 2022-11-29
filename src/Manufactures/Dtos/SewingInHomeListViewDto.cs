using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class SewingInHomeListViewDto : BaseDto
    {
        //Enhance Jason Aug 2021

        public Guid Identity { get; set; }
        public string SewingInNo { get; set; }
        public string Article { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public string RONo { get; set; }
        public string UnitFromCode { get; internal set; }
        public string UnitCode { get; internal set; }
        public string SewingFrom { get; set; }
        public DateTimeOffset SewingInDate { get; set; }
        public string Products { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public SewingInHomeListViewDto(SewingInHomeListView sewingInHomeListView)
        {
            Identity = sewingInHomeListView.Identity;
            SewingInNo = sewingInHomeListView.SewingInNo;
            Article = sewingInHomeListView.Article;
            RONo = sewingInHomeListView.RONo;
            UnitFromCode = sewingInHomeListView.UnitFromCode;
            UnitCode = sewingInHomeListView.UnitCode;
            TotalQuantity = sewingInHomeListView.TotalQuantity;
            TotalRemainingQuantity = sewingInHomeListView.TotalRemainingQuantity;
            SewingFrom = sewingInHomeListView.SewingFrom;
            SewingInDate = sewingInHomeListView.SewingInDate;
            Products = sewingInHomeListView.Products;
        }

        #region "Previous Code"
        //public GarmentSewingInListDto(GarmentSewingIn garmentSewingInList)
        //{
        //    Id = garmentSewingInList.Identity;
        //    SewingInNo = garmentSewingInList.SewingInNo;
        //    SewingFrom = garmentSewingInList.SewingFrom;
        //    LoadingId = garmentSewingInList.LoadingId;
        //    LoadingNo = garmentSewingInList.LoadingNo;
        //    UnitFrom = new UnitDepartment(garmentSewingInList.UnitFromId.Value, garmentSewingInList.UnitFromCode, garmentSewingInList.UnitFromName);
        //    Unit = new UnitDepartment(garmentSewingInList.UnitId.Value, garmentSewingInList.UnitCode, garmentSewingInList.UnitName);
        //    RONo = garmentSewingInList.RONo;
        //    Article = garmentSewingInList.Article;
        //    Comodity = new GarmentComodity(garmentSewingInList.ComodityId.Value, garmentSewingInList.ComodityCode, garmentSewingInList.ComodityName);
        //    SewingInDate = garmentSewingInList.SewingInDate;
        //    CreatedBy = garmentSewingInList.AuditTrail.CreatedBy;
        //    Items = new List<GarmentSewingInItemDto>();
        //}

        //public Guid Id { get; set; }
        //public string SewingInNo { get; set; }
        //public string SewingFrom { get; set; }
        //public Guid LoadingId { get; set; }
        //public string LoadingNo { get; set; }
        //public UnitDepartment UnitFrom { get; set; }
        //public UnitDepartment Unit { get; set; }
        //public string RONo { get; set; }
        //public string Article { get; set; }
        //public GarmentComodity Comodity { get; set; }
        //public DateTimeOffset SewingInDate { get; set; }
        //public double TotalQuantity { get; set; }
        //public double TotalRemainingQuantity { get; set; }
        //public List<string> Products { get; set; }
        //public List<GarmentSewingInItemDto> Items { get; set; }
        #endregion

    }
}