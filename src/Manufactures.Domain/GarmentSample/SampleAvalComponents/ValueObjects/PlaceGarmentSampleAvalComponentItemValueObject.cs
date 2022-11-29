using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents.ValueObjects
{
    public class PlaceGarmentSampleAvalComponentItemValueObject : ValueObject
    {
        public bool IsDifferentSize { get; set; }
        public Guid SampleCuttingInDetailId { get; set; }
        public Guid SampleSewingOutItemId { get; set; }
        public Guid SampleSewingOutDetailId { get; set; }
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
