using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class GarmentPackingListResult : BaseResult
    {
        public GarmentPackingListResult()
        {
            data = new List<GarmentPackingList>();
        }
        public IList<GarmentPackingList> data { get; set; }
    }

    public class SingleGarmentPackingListResult : BaseResult
    {
        public GarmentPackingList data { get; set; }
    }

    public class GarmentPackingList
    {
        public int Id { get; set; }

        public string InvoiceNo { get; set; }
        public bool IsGarmentExpenditureGood { get; set; }
    }
}
