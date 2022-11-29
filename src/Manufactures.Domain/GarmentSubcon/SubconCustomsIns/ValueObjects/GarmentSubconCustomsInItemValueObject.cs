using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ValueObjects
{
    public class GarmentSubconCustomsInItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SubconCustomsInId { get; set; }
        public Supplier Supplier { get; set; }
        public int DoId { get; set; }
        public string DoNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalQty { get; set; }
        public decimal RemainingQuantity { get; set; }

        public GarmentSubconCustomsInItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
