using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes
{
    public class GarmentServiceSubconFabricWashItem : AggregateRoot<GarmentServiceSubconFabricWashItem, GarmentServiceSubconFabricWashItemReadModel>
    {
        public Guid ServiceSubconFabricWashId { get; private set; }
        public string UnitExpenditureNo { get; private set; }
        public DateTimeOffset ExpenditureDate { get; private set; }

        public UnitSenderId UnitSenderId { get; private set; }
        public string UnitSenderCode { get; private set; }
        public string UnitSenderName { get; private set; }

        public UnitRequestId UnitRequestId { get; private set; }
        public string UnitRequestCode { get; private set; }
        public string UnitRequestName { get; private set; }

        public GarmentServiceSubconFabricWashItem(Guid identity, Guid serviceSubconFabricWashId, string unitExpenditureNo, DateTimeOffset expenditureDate, UnitSenderId unitSenderId, string unitSenderCode, string unitSenderName, UnitRequestId unitRequestId, string unitRequestCode, string unitRequestName) : base(identity)
        {
            Identity = identity;
            ServiceSubconFabricWashId = serviceSubconFabricWashId;
            UnitExpenditureNo = unitExpenditureNo;
            ExpenditureDate = expenditureDate;
            UnitSenderId = unitSenderId;
            UnitSenderCode = unitSenderCode;
            UnitSenderName = unitSenderName;
            UnitRequestId = unitRequestId;
            UnitRequestCode = unitRequestCode;
            UnitRequestName = unitRequestName;

            ReadModel = new GarmentServiceSubconFabricWashItemReadModel(identity)
            {
                ServiceSubconFabricWashId = ServiceSubconFabricWashId,
                ExpenditureDate = ExpenditureDate,
                UnitExpenditureNo = UnitExpenditureNo,
                UnitSenderId = UnitSenderId.Value,
                UnitSenderCode = UnitSenderCode,
                UnitSenderName = UnitSenderName,
                UnitRequestId = UnitRequestId.Value,
                UnitRequestCode = UnitRequestCode,
                UnitRequestName = UnitRequestName
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconFabricWashPlaced(Identity));
        }

        public GarmentServiceSubconFabricWashItem(GarmentServiceSubconFabricWashItemReadModel readModel) : base(readModel)
        {
            ServiceSubconFabricWashId = readModel.ServiceSubconFabricWashId;
            UnitExpenditureNo = readModel.UnitExpenditureNo;
            ExpenditureDate = readModel.ExpenditureDate;
            UnitSenderId = new UnitSenderId(readModel.UnitSenderId);
            UnitSenderCode = readModel.UnitSenderCode;
            UnitSenderName = readModel.UnitSenderName;
            UnitRequestId = new UnitRequestId(readModel.UnitRequestId);
            UnitRequestCode = readModel.UnitRequestCode;
            UnitRequestName = readModel.UnitRequestName;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconFabricWashItem GetEntity()
        {
            return this;
        }
    }
}
