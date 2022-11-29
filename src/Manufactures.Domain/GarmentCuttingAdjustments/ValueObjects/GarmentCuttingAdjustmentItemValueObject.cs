using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingAdjustments.ValueObjects
{
    public class GarmentCuttingAdjustmentItemValueObject : ValueObject
    {
        public Guid AdjustmentCuttingId { get; set; }
        public Guid CutInDetailId { get; set; }
        public Guid PreparingItemId { get; set; }
        public decimal FC { get; set; }
        public decimal ActualFC { get; set; }
        public decimal Quantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public bool IsSave { get; set; }

        public GarmentCuttingAdjustmentItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
