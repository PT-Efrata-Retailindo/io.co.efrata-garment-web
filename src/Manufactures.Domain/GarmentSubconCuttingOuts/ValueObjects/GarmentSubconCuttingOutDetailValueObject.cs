using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;
using Moonlay.Domain;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.ValueObjects
{
    public class GarmentSubconCuttingOutDetailValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid CutOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double CuttingOutQuantity { get; set; }
        public Uom CuttingOutUom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
        public string Remark { get; set; }

        public GarmentSubconCuttingOutDetailValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
