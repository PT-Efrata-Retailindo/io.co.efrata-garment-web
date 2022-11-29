using Infrastructure.Domain;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Events.GarmentSubcon;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings
{
    public class GarmentServiceSubconCuttingItem : AggregateRoot<GarmentServiceSubconCuttingItem, GarmentServiceSubconCuttingItemReadModel>
    {

        public Guid ServiceSubconCuttingId { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }

        public GarmentServiceSubconCuttingItem(Guid identity, Guid serviceSubconCuttingId, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName) : base(identity)
        {
            Identity = identity;
            ServiceSubconCuttingId = serviceSubconCuttingId;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;

            ReadModel = new GarmentServiceSubconCuttingItemReadModel(Identity)
            {
                ServiceSubconCuttingId=ServiceSubconCuttingId,
                Article = Article,
                ComodityCode = ComodityCode,
                ComodityId = ComodityId.Value,
                ComodityName = ComodityName,
                RONo = RONo
            };

            ReadModel.AddDomainEvent(new OnServiceSubconCuttingPlaced(Identity));
        }

        public GarmentServiceSubconCuttingItem(GarmentServiceSubconCuttingItemReadModel readModel) : base(readModel)
        {
            ServiceSubconCuttingId = readModel.ServiceSubconCuttingId;
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            Article = readModel.Article;
            RONo = readModel.RONo;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconCuttingItem GetEntity()
        {
            return this;
        }
    }
}
