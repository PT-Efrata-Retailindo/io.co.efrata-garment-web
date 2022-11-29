using System;
using System.Collections.Generic;
using Moonlay.Domain;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects
{
    public class GarmentServiceSubconSewingItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public Buyer Buyer { get; set; }
        public UnitDepartment Unit { get; set; }
        public List<GarmentServiceSubconSewingDetailValueObject> Details { get; set; }

        public GarmentServiceSubconSewingItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
