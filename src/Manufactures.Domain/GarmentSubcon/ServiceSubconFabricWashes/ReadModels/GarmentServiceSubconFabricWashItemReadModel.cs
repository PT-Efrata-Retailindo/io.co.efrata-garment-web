using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels
{
    public class GarmentServiceSubconFabricWashItemReadModel : ReadModelBase
    {
        public GarmentServiceSubconFabricWashItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSubconFabricWashId { get; internal set; }
        public string UnitExpenditureNo { get; internal set; }
        public DateTimeOffset ExpenditureDate { get; internal set; }

        public int UnitSenderId { get; internal set; }
        public string UnitSenderCode { get; internal set; }
        public string UnitSenderName { get; internal set; }

        public int UnitRequestId { get; internal set; }
        public string UnitRequestCode { get; internal set; }
        public string UnitRequestName { get; internal set; }

        public virtual GarmentServiceSubconFabricWashReadModel GarmentServiceSubconFabricWashIdentity { get; internal set; }
        public virtual List<GarmentServiceSubconFabricWashDetailReadModel> GarmentServiceSubconFabricWashDetail { get; internal set; }
    }
}
