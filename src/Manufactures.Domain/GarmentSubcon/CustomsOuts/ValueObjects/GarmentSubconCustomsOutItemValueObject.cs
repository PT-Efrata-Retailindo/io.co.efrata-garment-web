using System;
using System.Collections.Generic;
using System.Text;
using Moonlay.Domain;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.ValueObjects
{
    public class GarmentSubconCustomsOutItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public string SubconDLOutNo { get; set; }
        public Guid SubconDLOutId { get; set; }
        public Guid SubconCustomsOutId { get; set; }
        public double Quantity { get; set; }
        public double ContractQuantity { get; set; }
        public GarmentSubconCustomsOutItemValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
