using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentLoadings.ValueObjects
{
    public class GarmentLoadingItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid LoadingId { get;  set; }
        public Guid SewingDOItemId { get;  set; }
        public Product Product { get;  set; }
        public string DesignColor { get;  set; }
        public SizeValueObject Size { get;  set; }
        public double Quantity { get;  set; }
        public Uom Uom { get;  set; }
        public string Color { get;  set; }
        public double RemainingQuantity { get;  set; }
        public double BasicPrice { get; set; }
        public double SewingDORemainingQuantity { get; set; }
        public bool IsSave { get; set; }
        public double Price { get; set; }

        public GarmentLoadingItemValueObject()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
