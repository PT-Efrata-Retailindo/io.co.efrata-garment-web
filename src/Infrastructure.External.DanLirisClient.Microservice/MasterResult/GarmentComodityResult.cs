using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class GarmentComodityResult : BaseResult
    {
        public GarmentComodityResult()
        {
            data = new List<GarmentComodity>();
        }
        public IList<GarmentComodity> data { get; set; }
    }

    public class SingleGarmentComodityResult : BaseResult
    {
        public GarmentComodity data { get; set; }
    }

    public class GarmentComodity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}