using Moonlay.Domain;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.ValueObjects
{
    public class GarmentSewingOutDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SewingOutItemId { get;  set; }
        public SizeValueObject Size { get;  set; }
        public double Quantity { get;  set; }
        public Uom Uom { get;  set; }
        public GarmentSewingOutDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
