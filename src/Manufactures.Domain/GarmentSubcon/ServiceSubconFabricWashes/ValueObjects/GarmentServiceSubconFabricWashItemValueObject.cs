using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ValueObjects
{
    public class GarmentServiceSubconFabricWashItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public string UnitExpenditureNo { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public UnitSender UnitSender { get; set; }
        public UnitRequest UnitRequest { get; set; }
        public List<GarmentServiceSubconFabricWashDetailValueObject> Details { get; set; }

        public GarmentServiceSubconFabricWashItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
