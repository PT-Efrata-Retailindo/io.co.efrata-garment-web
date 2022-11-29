using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings
{
    public class GarmentServiceSubconSewingItem : AggregateRoot<GarmentServiceSubconSewingItem, GarmentServiceSubconSewingItemReadModel>
    {
        public Guid ServiceSubconSewingId { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }
        
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }


        public GarmentServiceSubconSewingItem(Guid identity, Guid serviceSubconSewingId, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, BuyerId buyerId, string buyerCode, string buyerName, UnitDepartmentId unitId, string unitCode, string unitName) : base(identity)
        {
            Identity = identity;
            ServiceSubconSewingId = serviceSubconSewingId;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            BuyerCode = buyerCode;
            BuyerId = buyerId;
            BuyerName = buyerName;
            UnitId = unitId;
            UnitName = unitName;
            UnitCode = unitCode;

            ReadModel = new GarmentServiceSubconSewingItemReadModel(identity)
            {
                ServiceSubconSewingId = ServiceSubconSewingId,
                RONo = RONo,
                Article = Article,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                BuyerCode = BuyerCode,
                BuyerId = BuyerId.Value,
                BuyerName = BuyerName,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName
                
                
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconSewingPlaced(Identity));

        }

        public GarmentServiceSubconSewingItem(GarmentServiceSubconSewingItemReadModel readModel) : base(readModel)
        {
            ServiceSubconSewingId = readModel.ServiceSubconSewingId;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            BuyerCode = readModel.BuyerCode;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerName = readModel.BuyerName;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconSewingItem GetEntity()
        {
            return this;
        }
    }
}
