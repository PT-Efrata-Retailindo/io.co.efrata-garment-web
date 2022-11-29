using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class UnitRequest : ValueObject
    {
        public UnitRequest()
        {

        }

        public UnitRequest(int unitRequestId, string code, string name)
        {
            Id = unitRequestId;
            Code = code;
            Name = name;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
