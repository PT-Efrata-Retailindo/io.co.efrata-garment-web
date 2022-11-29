using Moonlay.Domain;
using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.ValueObjects
{
    public class GarmentSubconCuttingOutItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid CutOutId { get; set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double TotalCuttingOut { get; set; }
        public bool IsSave { get; set; }
        public List<GarmentSubconCuttingOutDetailValueObject> Details { get; set; }

        public double TotalRemainingQuantityCuttingInItem { get; set; }
        public double TotalCuttingOutQuantity { get; set; }


        public GarmentSubconCuttingOutItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
