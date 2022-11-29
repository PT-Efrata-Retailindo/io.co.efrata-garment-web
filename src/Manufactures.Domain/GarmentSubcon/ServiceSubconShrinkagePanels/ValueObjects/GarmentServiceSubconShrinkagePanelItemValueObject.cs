using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ValueObjects
{
    public class GarmentServiceSubconShrinkagePanelItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public string UnitExpenditureNo { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public UnitSender UnitSender { get; set; }
        public UnitRequest UnitRequest { get; set; }
        public List<GarmentServiceSubconShrinkagePanelDetailValueObject> Details { get; set; }

        public GarmentServiceSubconShrinkagePanelItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
