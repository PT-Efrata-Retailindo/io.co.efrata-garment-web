using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class UomResult : BaseResult
    {
        public UomResult()
        {
            data = new List<Uom>();
        }
        public IList<Uom> data { get; set; }
    }

    public class SingleUomResult : BaseResult
    {
        public Uom data { get; set; }
    }

    public class Uom
    {
        public int Id { get; set; }
        public string Unit { get; set; }
    }
}
