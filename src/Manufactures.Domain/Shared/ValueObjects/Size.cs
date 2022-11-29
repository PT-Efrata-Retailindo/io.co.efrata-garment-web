using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class SizeValueObject : ValueObject
    {
        public SizeValueObject()
        {

        }
        public SizeValueObject(int sizeId, string sizeName)
        {
            Id = sizeId;
            Size = sizeName;
        }

        public int Id { get; set; }
        public string Size { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
