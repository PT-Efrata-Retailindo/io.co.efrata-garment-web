using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingOut : AggregateRoot<GarmentSubconCuttingOut, GarmentCuttingOutReadModel>
    {
        public string CutOutNo { get; private set; }
        public string CuttingOutType { get; private set; }

        public UnitDepartmentId UnitFromId { get; private set; }
        public string UnitFromCode { get; private set; }
        public string UnitFromName { get; private set; }
        public DateTimeOffset CuttingOutDate { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }

        public long EPOId { get; private set; }
        public long EPOItemId { get; private set; }
        public string POSerialNumber { get; private set; }
        public double PlanPORemainingQuantity { get; private set; }
        public double TotalQty { get; private set; }
        public bool IsUsed { get; private set; }

        public GarmentSubconCuttingOut(Guid identity, string cutOutNo, string cuttingOutType, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, DateTimeOffset cuttingOutDate, string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, long epoId, long epoItemId, string poSerialNumber, bool isUsed) : base(identity)
        {
            Validator.ThrowIfNull(() => unitFromId);
            Validator.ThrowIfNull(() => rONo);

            //MarkTransient();

            Identity = identity;
            CutOutNo = cutOutNo;
            CuttingOutType = cuttingOutType;
            UnitFromId = unitFromId;
            UnitFromCode = unitFromCode;
            UnitFromName = unitFromName;
            CuttingOutDate = cuttingOutDate;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            EPOId = epoId;
            EPOItemId = epoItemId;
            POSerialNumber = poSerialNumber;
            IsUsed = isUsed;

            ReadModel = new GarmentCuttingOutReadModel(Identity)
            {
                CutOutNo = CutOutNo,
                CuttingOutType = CuttingOutType,
                UnitFromId = UnitFromId.Value,
                UnitFromCode = UnitFromCode,
                UnitFromName = UnitFromName,
                CuttingOutDate = CuttingOutDate,
                RONo = RONo,
                Article = Article,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                EPOId = EPOId,
                EPOItemId = EPOItemId,
                POSerialNumber = POSerialNumber,
                IsUsed=IsUsed
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCuttingOutPlaced(Identity));
        }

        public GarmentSubconCuttingOut(GarmentCuttingOutReadModel readModel) : base(readModel)
        {
            CutOutNo = readModel.CutOutNo;
            CuttingOutType = readModel.CuttingOutType;
            UnitFromId = new UnitDepartmentId(readModel.UnitFromId);
            UnitFromCode = readModel.UnitFromCode;
            UnitFromName = readModel.UnitFromName;
            CuttingOutDate = readModel.CuttingOutDate;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = ReadModel.ComodityCode;
            ComodityName = ReadModel.ComodityName;
            EPOId = readModel.EPOId;
            EPOItemId = readModel.EPOItemId;
            POSerialNumber = readModel.POSerialNumber;
            IsUsed = readModel.IsUsed;
        }

        public void SetIsUsed(bool isUsed)
        {
            if (IsUsed != isUsed)
            {
                IsUsed = isUsed;
                ReadModel.IsUsed = isUsed;

                MarkModified();
            }
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCuttingOut GetEntity()
        {
            return this;
        }
    }
}
