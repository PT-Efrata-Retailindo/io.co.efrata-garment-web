using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class ProductResult : BaseResult
    {
        public ProductResult()
        {
            data = new List<Product>();
        }
        public IList<Product> data { get; set; }
    }

    public class SingleProductResult : BaseResult
    {
        public Product data { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
        public string Composition { get; set; }
    }
}
