using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentAvalComponents.ValueObjects
{
    public class PlaceGarmentAvalComponentItemValueObject : ValueObject
    {
        public bool IsDifferentSize { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Guid SewingOutItemId { get; set; }
        public Guid SewingOutDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public string Color { get; set; }
        public double Quantity { get; set; }
        public double SourceQuantity { get; set; }
        public SizeValueObject Size { get; set; }
        public bool IsSave { get; set; }
        public decimal Price { get; set; }
        public decimal BasicPrice { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
