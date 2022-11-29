using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.SampleCuttingIns.ValueObjects
{
    public class GarmentSampleCuttingInDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid PreparingItemId { get; set; }
        public Product Product { get; set; }

        public string DesignColor { get; set; }
        public string FabricType { get; set; }

        public double PreparingRemainingQuantity { get; set; }
        public double PreparingQuantity { get; set; }
        public Uom PreparingUom { get; set; }

        public int CuttingInQuantity { get; set; }
        public Uom CuttingInUom { get; set; }

        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }

        public bool IsSave { get; set; }
        public double Price { get; set; }
        public double FC { get; set; }

        public GarmentSampleCuttingInDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
