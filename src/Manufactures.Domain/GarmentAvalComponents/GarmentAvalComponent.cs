using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.GarmentAvalComponents
{
    public class GarmentAvalComponent : AggregateRoot<GarmentAvalComponent, GarmentAvalComponentReadModel>
    {
        public string AvalComponentNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string AvalComponentType { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public bool IsReceived { get; private set; }

        public GarmentAvalComponent(Guid identity, string avalComponentNo, UnitDepartmentId unitId, string unitCode, string unitName, string avalComponentType, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, DateTimeOffset date, bool isReceived) : base(identity)
        {
            Identity = identity;
            AvalComponentNo = avalComponentNo;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            AvalComponentType = avalComponentType;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            Date = date;
            IsReceived = isReceived;

            ReadModel = new GarmentAvalComponentReadModel(Identity)
            {
                AvalComponentNo = avalComponentNo,
                UnitId = unitId.Value,
                UnitCode = unitCode,
                UnitName = unitName,
                AvalComponentType = avalComponentType,
                RONo = rONo,
                Article = article,
                ComodityId = comodityId.Value,
                ComodityCode = comodityCode,
                ComodityName = comodityName,
                Date = date,
                IsReceived = isReceived,
            };
        }

        public GarmentAvalComponent(GarmentAvalComponentReadModel readModel) : base(readModel)
        {
            AvalComponentNo = readModel.AvalComponentNo;
            UnitId = new UnitDepartmentId((int)readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            AvalComponentType = readModel.AvalComponentType;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId((int)readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            Date = readModel.Date;
            IsReceived = readModel.IsReceived;
        }

        protected override GarmentAvalComponent GetEntity()
        {
            return this;
        }

        public void SetIsReceived(bool IsReceived)
        {
            if (this.IsReceived != IsReceived)
            {
                this.IsReceived = IsReceived;
                ReadModel.IsReceived = IsReceived;
            }
        }

        public void SetDeleted()
        {
            MarkModified();
        }
    }
}
