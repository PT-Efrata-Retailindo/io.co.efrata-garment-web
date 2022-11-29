using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts.ValueObjects
{
    public class GarmentSubconContractItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SubconContractId { get; set; }

        public Product Product { get; set; }
        public double Quantity { get; set; }

        public Uom Uom { get; set; }
        public int CIFItem { get; set; }

        public GarmentSubconContractItemValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
