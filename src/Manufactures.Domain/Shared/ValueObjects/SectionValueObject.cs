using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class SectionValueObject : ValueObject
    {
        public SectionValueObject()
        {

        }
        public SectionValueObject(int sectionId, string code)
        {
            Id = sectionId;
            Code = code;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
