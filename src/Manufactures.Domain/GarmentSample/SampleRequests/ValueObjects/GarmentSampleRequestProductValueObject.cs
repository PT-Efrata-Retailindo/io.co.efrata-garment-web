using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.ValueObjects
{
    public class GarmentSampleRequestProductValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SampleRequestId { get;  set; }
        public string Style { get;  set; }
        public string Color { get;  set; }

        public string Fabric { get; set; }

        public SizeValueObject Size { get;  set; }

        public string SizeDescription { get;  set; }
        public double Quantity { get;  set; }
        public int Index { get; set; }

        public GarmentSampleRequestProductValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
