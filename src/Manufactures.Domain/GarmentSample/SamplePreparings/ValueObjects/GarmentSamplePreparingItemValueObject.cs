using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects
{
    public class GarmentSamplePreparingItemValueObject : ValueObject
    {
        public GarmentSamplePreparingItemValueObject()
        {

        }


        public GarmentSamplePreparingItemValueObject(Guid id, int uenItemId, Product product, string designColor, double quantity, Uom uom, string fabricType, double remainingQuantity, double basicPrice, Guid garmentSamplePreparingId, string roSource)
        {
            Identity = id;
            UENItemId = uenItemId;
            Product = product;
            DesignColor = designColor;
            Quantity = quantity;
            Uom = uom;
            FabricType = fabricType;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            GarmentSamplePreparingId = garmentSamplePreparingId;
            ROSource = roSource;
        }

        public Guid Identity { get; set; }
        public int UENItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string FabricType { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public Guid GarmentSamplePreparingId { get; set; }
        public string ROSource { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
