using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GarmentSewingDOs
{
    public class GarmentSewingDO : AggregateRoot<GarmentSewingDO, GarmentSewingDOReadModel>
    {
        public string SewingDONo { get; private set; }
        public Guid CuttingOutId { get; private set; }
        public UnitDepartmentId UnitFromId { get; private set; }
        public string UnitFromCode { get; private set; }
        public string UnitFromName { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public DateTimeOffset SewingDODate { get; private set; }

        public GarmentSewingDO(Guid identity, string sewingDONo, Guid cuttingOutId, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, UnitDepartmentId unitId, string unitCode, string unitName, string roNo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, DateTimeOffset sewingDODate) : base(identity)
        {
            Identity = identity;
            SewingDONo = sewingDONo;
            CuttingOutId = cuttingOutId;
            UnitFromId = unitFromId;
            UnitFromCode = unitFromCode;
            UnitFromName = unitFromName;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            RONo = roNo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            SewingDODate = sewingDODate;

            ReadModel = new GarmentSewingDOReadModel(Identity)
            {
                SewingDONo = SewingDONo,
                CuttingOutId = CuttingOutId,
                UnitFromId = UnitFromId.Value,
                UnitFromCode = UnitFromCode,
                UnitFromName = UnitFromName,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                RONo = RONo,
                Article = Article,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                SewingDODate = SewingDODate
            };
            ReadModel.AddDomainEvent(new OnGarmentSewingDOPlaced(Identity));
        }

        public GarmentSewingDO(GarmentSewingDOReadModel readModel) : base(readModel)
        {
            SewingDONo = readModel.SewingDONo;
            CuttingOutId = readModel.CuttingOutId;
            UnitFromId = new UnitDepartmentId(readModel.UnitFromId);
            UnitFromCode = readModel.UnitFromCode;
            UnitFromName = readModel.UnitFromName;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            SewingDODate = readModel.SewingDODate;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSewingDO GetEntity()
        {
            return this;
        }
    }
}