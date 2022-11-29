using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts
{
    public class GarmentFinishingOut : AggregateRoot<GarmentFinishingOut, GarmentFinishingOutReadModel>
    {
        public string FinishingOutNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string FinishingTo { get; private set; }
        public UnitDepartmentId UnitToId { get; private set; }
        public string UnitToCode { get; private set; }
        public string UnitToName { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset FinishingOutDate { get; private set; }
        public bool IsDifferentSize { get; private set; }


        public GarmentFinishingOut(Guid identity, string finishingOutNo, UnitDepartmentId unitToId, string unitToCode, string unitToName, string finishingTo, DateTimeOffset finishingOutDate, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, GarmentComodityId comodityId, string comodityCode, string comodityName, bool isDifferentSize) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => unitToId);
            Validator.ThrowIfNull(() => rONo);

            //MarkTransient();

            Identity = identity;
            FinishingOutNo = finishingOutNo;
            FinishingTo = finishingTo;
            UnitToId = unitToId;
            UnitToCode = unitToCode;
            UnitToName = unitToName;
            FinishingOutDate = finishingOutDate;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            IsDifferentSize = isDifferentSize;

            ReadModel = new GarmentFinishingOutReadModel(Identity)
            {
                FinishingOutNo = FinishingOutNo,
                FinishingTo = FinishingTo,
                UnitToId = UnitToId.Value,
                UnitToCode = UnitToCode,
                UnitToName = UnitToName,
                FinishingOutDate = FinishingOutDate,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                IsDifferentSize = IsDifferentSize,
            };

            ReadModel.AddDomainEvent(new OnGarmentFinishingOutPlaced(Identity));
        }

        public GarmentFinishingOut(GarmentFinishingOutReadModel readModel) : base(readModel)
        {
            FinishingOutNo = readModel.FinishingOutNo;
            FinishingTo = readModel.FinishingTo;
            UnitToId = new UnitDepartmentId(readModel.UnitToId);
            UnitToCode = readModel.UnitToCode;
            UnitToName = readModel.UnitToName;
            FinishingOutDate = readModel.FinishingOutDate;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            IsDifferentSize = readModel.IsDifferentSize;
        }
        public void SetDate(DateTimeOffset FinishingOutDate)
        {
            if (this.FinishingOutDate != FinishingOutDate)
            {
                this.FinishingOutDate = FinishingOutDate;
                ReadModel.FinishingOutDate = FinishingOutDate;
            }
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentFinishingOut GetEntity()
        {
            return this;
        }
    }
}
