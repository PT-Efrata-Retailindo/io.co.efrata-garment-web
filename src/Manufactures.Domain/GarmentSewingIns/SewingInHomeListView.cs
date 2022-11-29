using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingIns
{
    public class SewingInHomeListView : AggregateRoot<SewingInHomeListView, SewingInHomeListViewReadModel>
    {
        //Enhance Jason Aug 2021
        public string SewingInNo { get; set; }
        public string Article { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public string RONo { get; set; }
        public string UnitFromCode { get; private set; }
        public string UnitCode { get; private set; }
        public string SewingFrom { get; set; }
        public DateTimeOffset SewingInDate { get; set; }
        public string Products { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public SewingInHomeListView(Guid identity, string sewingInNo, string article, double totalQuantity, double totalRemainingQuantity, string roNo, string unitFromCode, string unitCode, string sewingFrom, DateTimeOffset sewingInDate, string listProducts) : base(identity)
        {
            Identity = identity;
            SewingInNo = sewingInNo;
            SewingFrom = sewingFrom;
            UnitFromCode = unitFromCode;
            UnitCode = unitCode;
            TotalQuantity = totalQuantity;
            TotalRemainingQuantity = totalRemainingQuantity;
            RONo = roNo;
            Article = article;
            SewingInDate = sewingInDate;
            Products = listProducts;

            ReadModel = new SewingInHomeListViewReadModel(Identity)
            {
                SewingInNo = SewingInNo,
                SewingFrom=SewingFrom,
                UnitFromCode = UnitFromCode,
                UnitCode = UnitCode,
                TotalQuantity = TotalQuantity,
                TotalRemainingQuantity = TotalRemainingQuantity,
                RONo = RONo,
                Article = Article,
                SewingInDate = SewingInDate,
                Products = Products
            };
            //ReadModel.AddDomainEvent(new OnGarmentSewingInPlaced(Identity));
        }

        public SewingInHomeListView(SewingInHomeListViewReadModel readModel) : base(readModel)
        {
            SewingInNo = readModel.SewingInNo;
            SewingFrom = readModel.SewingFrom;
            UnitFromCode = readModel.UnitFromCode;
            UnitCode = readModel.UnitCode;
            TotalQuantity = readModel.TotalQuantity;
            TotalRemainingQuantity = readModel.TotalRemainingQuantity;
            RONo = readModel.RONo;
            Article = readModel.Article;
            SewingInDate = readModel.SewingInDate;
            Products = readModel.Products;
        }
        public void setDate(DateTimeOffset sewingInDate)
        {
            if (sewingInDate != SewingInDate)
            {
                SewingInDate = sewingInDate;
                ReadModel.SewingInDate = sewingInDate;

                MarkModified();
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override SewingInHomeListView GetEntity()
        {
            return this;
        }
    }
}