using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class UnitDepartment : ValueObject
    {
        public UnitDepartment()
        {

        }

        public UnitDepartment(int departmentId, string code, string name)
        {
            Id = departmentId;
            Code = code;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
