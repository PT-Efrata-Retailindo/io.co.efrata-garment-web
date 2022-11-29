using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAdjustments
{
    public class GarmentAdjustment : AggregateRoot<GarmentAdjustment, GarmentAdjustmentReadModel>
    {
        public string AdjustmentNo { get; internal set; }
        public string AdjustmentType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public GarmentComodityId ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public UnitDepartmentId UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public DateTimeOffset AdjustmentDate { get; internal set; }
        public string AdjustmentDesc { get; internal set; }

        public GarmentAdjustment(Guid identity, string adjustmentNo, string adjustmentType, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset adjustmentDate, GarmentComodityId comodityId, string comodityCode, string comodityName, string adjustmentDesc) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            //MarkTransient();

            Identity = identity;
            AdjustmentNo = adjustmentNo;
            AdjustmentType = adjustmentType;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            AdjustmentDate = adjustmentDate;
            AdjustmentDesc = adjustmentDesc;

            ReadModel = new GarmentAdjustmentReadModel(Identity)
            {
                AdjustmentDate = AdjustmentDate,
                AdjustmentNo = AdjustmentNo,
                AdjustmentType=AdjustmentType,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                AdjustmentDesc= AdjustmentDesc
            };

            ReadModel.AddDomainEvent(new OnGarmentAdjustmentPlaced(Identity));
        }

        public GarmentAdjustment(GarmentAdjustmentReadModel readModel) : base(readModel)
        {
            AdjustmentNo = readModel.AdjustmentNo;
            AdjustmentType = readModel.AdjustmentType;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityName = readModel.ComodityName;
            ComodityCode = readModel.ComodityCode;
            AdjustmentDate = readModel.AdjustmentDate;
            AdjustmentDesc = readModel.AdjustmentDesc;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentAdjustment GetEntity()
        {
            return this;
        }
    }
}

