using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels
{
    public class GarmentServiceSubconShrinkagePanelItemReadModel : ReadModelBase
    {
        public GarmentServiceSubconShrinkagePanelItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSubconShrinkagePanelId { get; internal set; }
        public string UnitExpenditureNo { get; internal set; }
        public DateTimeOffset ExpenditureDate { get; internal set; }

        public int UnitSenderId { get; internal set; }
        public string UnitSenderCode { get; internal set; }
        public string UnitSenderName { get; internal set; }

        public int UnitRequestId { get; internal set; }
        public string UnitRequestCode { get; internal set; }
        public string UnitRequestName { get; internal set; }

        public virtual GarmentServiceSubconShrinkagePanelReadModel GarmentServiceSubconShrinkagePanelIdentity { get; internal set; }
        public virtual List<GarmentServiceSubconShrinkagePanelDetailReadModel> GarmentServiceSubconShrinkagePanelDetail { get; internal set; }
    }
}
