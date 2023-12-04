using Moonlay.Domain;
using System;
using System.Collections.Generic;
namespace Manufactures.Domain.GarmentDeliveryReturns.ValueObjects
{
    public class GarmentDeliveryReturnItemValueObject : ValueObject
    {
        public GarmentDeliveryReturnItemValueObject()
        {

        }

        public GarmentDeliveryReturnItemValueObject(Guid id, Guid drId, int unitDOItemId, int uenItemId, string preparingItemId, Product product, string designColor, string roNo, double quantity, Uom uom, Guid garmentDeliveryReturnId, double quantityUENItem, double remainingQuantityPreparingItem, string colour, string rack, string level, string box, string area, bool isSave)
        {
            Id = id;
            DRId = drId;
            UnitDOItemId = unitDOItemId;
            UENItemId = uenItemId;
            PreparingItemId = preparingItemId;
            Product = product;
            DesignColor = designColor;
            RONo = roNo;
            Quantity = quantity;
            Uom = uom;
            QuantityUENItem = quantityUENItem;
            RemainingQuantityPreparingItem = remainingQuantityPreparingItem;
            IsSave = isSave;
            Area = area;
            Box = box;
            Colour = colour;
            Rack = rack;
            Level = level;
        }

        public Guid Id { get; set; }
        public Guid DRId { get; set; }
        public int UnitDOItemId { get; set; }
        public int UENItemId { get; set; }
        public string PreparingItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public string RONo { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public double QuantityUENItem { get; set; }
        public double RemainingQuantityPreparingItem { get; set; }
        public bool IsSave { get; set; }
        public string Colour { get; set; }
        public string Rack { get; set; }
        public string Level { get; set; }
        public string Box { get; set; }
        public string Area { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}