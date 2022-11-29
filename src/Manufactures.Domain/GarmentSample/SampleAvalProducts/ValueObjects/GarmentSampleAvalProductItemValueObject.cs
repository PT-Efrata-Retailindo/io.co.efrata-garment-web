using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects
{
    public class GarmentSampleAvalProductItemValueObject : ValueObject
    {
        public GarmentSampleAvalProductItemValueObject()
        {

        }

        public GarmentSampleAvalProductItemValueObject(Guid id, Guid apId, GarmentSamplePreparingId samplePreparingId, GarmentSamplePreparingItemId samplePreparingItemId, Product product, string designColor, double quantity, Uom uom)
        {

        }

        public Guid Identity { get; set; }
        public Guid APId { get; set; }
        public GarmentSamplePreparingId SamplePreparingId { get; set; }
        public GarmentSamplePreparingItemId SamplePreparingItemId { get; set; }
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
