using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentAvalProducts.ValueObjects
{
    public class GarmentAvalProductItemValueObject : ValueObject
    {
        public GarmentAvalProductItemValueObject()
        {

        }

        public GarmentAvalProductItemValueObject(Guid id, Guid apId, GarmentPreparingId preparingId, GarmentPreparingItemId preparingItemId, Product product, string designColor, double quantity, Uom uom)
        {

        }

        public Guid Identity { get; set; }
        public Guid APId { get; set; }
        public GarmentPreparingId PreparingId { get; set; }
        public GarmentPreparingItemId PreparingItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public double BasicPrice { get; set; }
        public double PreparingQuantity { get; set; }
        public bool IsReceived { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}