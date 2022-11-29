using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects
{
    public class GarmentServiceSubconCuttingDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid ServiceSubconCuttingItemId { get; set; }

        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public double CuttingInQuantity { get; set; }
        public bool IsSave { get; set; }
        public List<GarmentServiceSubconCuttingSizeValueObject> Sizes { get; set; }
        public GarmentServiceSubconCuttingDetailValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
