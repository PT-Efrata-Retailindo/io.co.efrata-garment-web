using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentDeliveryReturns.ValueObjects
{
    public class Product : ValueObject
    {
        public Product()
        {

        }

        public Product(int productId, string name, string code)
        {
            Id = productId;
            Name = name;
            Code = code;
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
