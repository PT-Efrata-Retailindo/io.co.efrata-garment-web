using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class PEBResult : BaseResult
    {
        public PEBResult()
        {
            data = new List<PEBResultViewModel>();
        }
        public IList<PEBResultViewModel> data { get; set; }
    }

    public class SinglePEBResultResult : BaseResult
    {
        public PEBResultViewModel data { get; set; }
    }

    public class PEBResultViewModel
    {
        public string BCNo { get; set; }
        public string BonNo { get; set; }
        public DateTime BCDate { get; set; }
        public string BuyerName { get; set; }
        public string CurrencyCode { get; set; }
        public string UomPrice { get; set; }
        public string Country { get; set; }
        public double Nominal { get; set; }
        public double Quantity { get; set; }

    }
}
