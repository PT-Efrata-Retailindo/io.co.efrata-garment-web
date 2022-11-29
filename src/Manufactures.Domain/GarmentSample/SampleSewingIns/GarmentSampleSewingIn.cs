using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingIns
{
    public class GarmentSampleSewingIn : AggregateRoot<GarmentSampleSewingIn, GarmentSampleSewingInReadModel>
    {
        public string SewingInNo { get; private set; }
        public string SewingFrom { get; private set; }
        public Guid CuttingOutId { get; private set; }
        public string CuttingOutNo { get; private set; }
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
        public DateTimeOffset SewingInDate { get; private set; }

        public GarmentSampleSewingIn(Guid identity, string sewingInNo, string sewingFrom, Guid cuttingOutId, string cuttingOutNo, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, UnitDepartmentId unitId, string unitCode, string unitName, string roNo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, DateTimeOffset sewingInDate) : base(identity)
        {
            Identity = identity;
            SewingInNo = sewingInNo;
            SewingFrom = sewingFrom;
            CuttingOutId = cuttingOutId;
            CuttingOutNo = cuttingOutNo;
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
            SewingInDate = sewingInDate;

            ReadModel = new GarmentSampleSewingInReadModel(Identity)
            {
                SewingInNo = SewingInNo,
                SewingFrom = SewingFrom,
                CuttingOutId = CuttingOutId,
                CuttingOutNo = CuttingOutNo,
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
                SewingInDate = SewingInDate
            };
            ReadModel.AddDomainEvent(new OnGarmentSampleSewingInPlaced(Identity));
        }

        public GarmentSampleSewingIn(GarmentSampleSewingInReadModel readModel) : base(readModel)
        {
            SewingInNo = readModel.SewingInNo;
            SewingFrom = readModel.SewingFrom;
            CuttingOutId = readModel.CuttingOutId;
            CuttingOutNo = readModel.CuttingOutNo;
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
            SewingInDate = readModel.SewingInDate;
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

        protected override GarmentSampleSewingIn GetEntity()
        {
            return this;
        }
    }
}
