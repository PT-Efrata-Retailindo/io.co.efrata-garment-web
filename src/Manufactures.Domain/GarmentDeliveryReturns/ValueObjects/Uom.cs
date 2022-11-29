using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentDeliveryReturns.ValueObjects
{
    public class Uom : ValueObject
    {
        public Uom()
        {

        }
        public Uom(int uomId, string uomUnit)
        {
            Id = uomId;
            Unit = uomUnit;
        }

        public int Id { get; set; }
        public string Unit { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
