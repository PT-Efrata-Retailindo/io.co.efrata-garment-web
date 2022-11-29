using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects
{
    public class GarmentSamplePreparingItem : ValueObject
    {
        public GarmentSamplePreparingItem()
        {

        }

        public GarmentSamplePreparingItem(string samplePreparingItemId, ProductId productId, string designColor, double quantity)
        {
            Id = samplePreparingItemId;
            ProductId = productId;
            DesignColor = designColor;
            Quantity = quantity;
        }

        public string Id { get; set; }
        public ProductId ProductId { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return ProductId;
            yield return DesignColor;
            yield return Quantity;
        }
    }
}
