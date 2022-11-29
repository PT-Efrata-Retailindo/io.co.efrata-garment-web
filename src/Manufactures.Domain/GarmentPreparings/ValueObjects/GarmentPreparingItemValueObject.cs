using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentPreparings.ValueObjects
{
    public class GarmentPreparingItemValueObject : ValueObject
    {
        public GarmentPreparingItemValueObject()
        {

        }


        public GarmentPreparingItemValueObject(Guid id, int uenItemId, Product product, string designColor, double quantity, Uom uom, string fabricType, double remainingQuantity, double basicPrice, Guid garmentPreparingId, string roSource)
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
            GarmentPreparingId = garmentPreparingId;
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
        public Guid GarmentPreparingId { get; set; }
        public string ROSource { get; set; }
        public string CustomsCategory { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}