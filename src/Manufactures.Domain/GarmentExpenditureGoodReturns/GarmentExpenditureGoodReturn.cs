using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoodReturns
{
    public class GarmentExpenditureGoodReturn : AggregateRoot<GarmentExpenditureGoodReturn, GarmentExpenditureGoodReturnReadModel>
    {

        public string ReturNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string ReturType { get; private set; }
        public string ExpenditureNo { get; private set; }
        public string DONo { get; private set; }
        public string URNNo { get; private set; }
        public string BCNo { get; private set; }
        public string BCType { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }
        public DateTimeOffset ReturDate { get; private set; }
        public string Invoice { get; private set; }
        public string ReturDesc { get; private set; }

        public GarmentExpenditureGoodReturn(Guid identity, string returNo, string returType, string expenditureNo, string doNo, string urnNo, string bcNo, string bcType, UnitDepartmentId unitId, string unitCode, string unitName, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, BuyerId buyerId, string buyerCode, string buyerName, DateTimeOffset returDate, string invoice, string returDesc) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);

            //MarkTransient();
            ReturNo = returNo;
            Identity = identity;
            ReturType = returType;
            ExpenditureNo = expenditureNo;
            DONo = doNo;
            URNNo = urnNo;
            BCNo = bcNo;
            BCType = bcType;
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
            ReturDate = returDate;
            Invoice = invoice;
            ReturDesc = returDesc;

            ReadModel = new GarmentExpenditureGoodReturnReadModel(Identity)
            {
                ReturNo = ReturNo,
                ReturType = ReturType,
                ReturDate = ReturDate,
                ExpenditureNo = ExpenditureNo,
                DONo = DONo,
                URNNo = urnNo,
                BCNo = BCNo,
                BCType = BCType,
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
                ReturDesc = ReturDesc
            };

            ReadModel.AddDomainEvent(new OnGarmentExpenditureGoodReturnPlaced(Identity));
        }

        public GarmentExpenditureGoodReturn(GarmentExpenditureGoodReturnReadModel readModel) : base(readModel)
        {
            ReturNo = readModel.ReturNo;
            ExpenditureNo = readModel.ExpenditureNo;
            DONo = readModel.DONo;
            URNNo = readModel.URNNo;
            BCNo = readModel.BCNo;
            BCType = readModel.BCType;
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
            ReturDate = readModel.ReturDate;
            ReturType = readModel.ReturType;
            Invoice = readModel.Invoice;
            ReturDesc = readModel.ReturDesc;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentExpenditureGoodReturn GetEntity()
        {
            return this;
        }
    }
}
