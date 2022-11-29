using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class Product : ValueObject
    {
        public Product()
        {

        }

        public Product(int productId, string code, string name)
        {
            Id = productId;
            Code = code;
            Name = name;
        }

        public Product(int productId, string code, string name, string remark)
        {
            Id = productId;
            Code = code;
            Name = name;
            Remark = remark;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
        public string Remark { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
