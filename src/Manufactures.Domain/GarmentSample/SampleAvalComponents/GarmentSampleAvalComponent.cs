using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents
{
    public class GarmentSampleAvalComponent : AggregateRoot<GarmentSampleAvalComponent, GarmentSampleAvalComponentReadModel>
    {
        public string SampleAvalComponentNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string SampleAvalComponentType { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public bool IsReceived { get; private set; }

        public GarmentSampleAvalComponent(Guid identity, string sampleAvalComponentNo, UnitDepartmentId unitId, string unitCode, string unitName, string sampleAvalComponentType, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, DateTimeOffset date, bool isReceived) : base(identity)
        {
            Identity = identity;
            SampleAvalComponentNo = sampleAvalComponentNo;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            SampleAvalComponentType = sampleAvalComponentType;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            Date = date;
            IsReceived = isReceived;

            ReadModel = new GarmentSampleAvalComponentReadModel(Identity)
            {
                SampleAvalComponentNo = sampleAvalComponentNo,
                UnitId = unitId.Value,
                UnitCode = unitCode,
                UnitName = unitName,
                SampleAvalComponentType = sampleAvalComponentType,
                RONo = rONo,
                Article = article,
                ComodityId = comodityId.Value,
                ComodityCode = comodityCode,
                ComodityName = comodityName,
                Date = date,
                IsReceived = isReceived,
            };
        }

        public GarmentSampleAvalComponent(GarmentSampleAvalComponentReadModel readModel) : base(readModel)
        {
            SampleAvalComponentNo = readModel.SampleAvalComponentNo;
            UnitId = new UnitDepartmentId((int)readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            SampleAvalComponentType = readModel.SampleAvalComponentType;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId((int)readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            Date = readModel.Date;
            IsReceived = readModel.IsReceived;
        }

        protected override GarmentSampleAvalComponent GetEntity()
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
