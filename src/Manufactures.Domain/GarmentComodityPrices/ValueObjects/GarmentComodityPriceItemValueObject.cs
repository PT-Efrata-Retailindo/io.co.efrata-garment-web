using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentComodityPrices.ValueObjects
{
    public class GarmentComodityPriceItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public bool IsValid { get;  set; }
        public DateTimeOffset Date { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public decimal Price { get;  set; }
        public decimal NewPrice { get; set; }

        public GarmentComodityPriceItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
