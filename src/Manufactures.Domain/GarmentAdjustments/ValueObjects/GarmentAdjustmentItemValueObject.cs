using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAdjustments.ValueObjects
{
    public class GarmentAdjustmentItemValueObject : ValueObject
    {
        public Guid AdjustmentId { get;  set; }
        public Guid SewingDOItemId { get;  set; }
        public Guid SewingInItemId { get; set; }
        public Guid FinishingInItemId { get; set; }
		public Guid FinishedGoodStockId { get; set; }
		public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get;  set; }
        public double BasicPrice { get;  set; }
        public double Price { get;  set; }
        public double RemainingQuantity { get; set; }
        public bool IsSave { get; set; }
		public string AdjustmentType { get; set; } 

		public GarmentAdjustmentItemValueObject()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
