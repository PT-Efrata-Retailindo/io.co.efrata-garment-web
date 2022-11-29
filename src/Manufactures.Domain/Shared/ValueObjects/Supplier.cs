using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class Supplier : ValueObject
    {
        public Supplier()
        {

        }

        public Supplier(int supplierId, string code, string name)
        {
            Id = supplierId;
            Code = code;
            Name = name;
        }

        public Supplier(int supplierId, string code, string name,string address)
        {
            Id = supplierId;
            Code = code;
            Name = name;
            Address = address;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}

