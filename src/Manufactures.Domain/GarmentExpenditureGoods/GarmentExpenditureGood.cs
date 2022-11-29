using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods
{
    public class GarmentExpenditureGood : AggregateRoot<GarmentExpenditureGood, GarmentExpenditureGoodReadModel>
    {

        public string ExpenditureGoodNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string ExpenditureType { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }
        public DateTimeOffset ExpenditureDate { get; private set; }
        public string Invoice { get; private set; }
        public int PackingListId { get; private set; }
        public string ContractNo { get; private set; }
        public double Carton { get; private set; }
        public string Description { get; private set; }
        public bool IsReceived { get; private set; }

        public GarmentExpenditureGood(Guid identity, string expenditureGoodNo, string expenditureType, UnitDepartmentId unitId, string unitCode, string unitName, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, BuyerId buyerId, string buyerCode, string buyerName, DateTimeOffset expenditureDate, string invoice, string contractNo, double carton, string description, bool isReceived, int packingListId) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);

            //MarkTransient();
            ExpenditureGoodNo = expenditureGoodNo;
            Identity = identity;
            ExpenditureType = expenditureType;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;
            ExpenditureDate = expenditureDate;
            Invoice = invoice;
            ContractNo = contractNo;
            Carton = carton;
            Description = description;
            IsReceived = isReceived;
            PackingListId = packingListId;

            ReadModel = new GarmentExpenditureGoodReadModel(Identity)
            {
                ExpenditureGoodNo = ExpenditureGoodNo,
                ExpenditureType = ExpenditureType,
                ExpenditureDate = ExpenditureDate,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName,
                BuyerId = BuyerId.Value,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                Invoice = Invoice,
                ContractNo = ContractNo,
                Carton= Carton,
                Description= Description,
                IsReceived= IsReceived,
                PackingListId = PackingListId
            };

            ReadModel.AddDomainEvent(new OnGarmentExpenditureGoodPlaced(Identity));
        }

        public GarmentExpenditureGood(GarmentExpenditureGoodReadModel readModel) : base(readModel)
        {
            ExpenditureGoodNo = readModel.ExpenditureGoodNo;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
            BuyerId = new BuyerId(readModel.BuyerId);
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityName = readModel.ComodityName;
            ComodityCode = readModel.ComodityCode;
            ExpenditureDate = readModel.ExpenditureDate;
            ExpenditureType = readModel.ExpenditureType;
            Invoice = readModel.Invoice;
            ContractNo = readModel.ContractNo;
            Carton = readModel.Carton;
            Description = readModel.Description;
            IsReceived = readModel.IsReceived;
            PackingListId = readModel.PackingListId;
        }

        public void SetCarton(double Carton)
        {
            if (this.Carton != Carton)
            {
                this.Carton = Carton;
                ReadModel.Carton = Carton;
            }
        }

        public void SetInvoice(string Invoice)
        {
            if (this.Invoice != Invoice)
            {
                this.Invoice = Invoice;
                ReadModel.Invoice = Invoice;
            }
        }
        public void SetPackingListId(int packingListId)
        {
            if (this.PackingListId != packingListId)
            {
                this.PackingListId = packingListId;
                ReadModel.PackingListId = packingListId;
            }
        }

        public void SetIsReceived(bool IsReceived)
        {
            if (this.IsReceived != IsReceived)
            {
                this.IsReceived = IsReceived;
                ReadModel.IsReceived = IsReceived;
            }
        }

        public void SetExpenditureDate(DateTimeOffset ExpenditureDate)
        {
            if (this.ExpenditureDate != ExpenditureDate)
            {
                this.ExpenditureDate = ExpenditureDate;
                ReadModel.ExpenditureDate = ExpenditureDate;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentExpenditureGood GetEntity()
        {
            return this;
        }
    }
}
