using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.ValueObjects
{
    public class GarmentPreparingItem : ValueObject
    {
        public GarmentPreparingItem()
        {

        }

        public GarmentPreparingItem(string preparingItemId, ProductId productId, string designColor, double quantity)
        {
            Id = preparingItemId;
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