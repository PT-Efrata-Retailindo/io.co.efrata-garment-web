using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class GarmentProductResult : BaseResult
    {
        public GarmentProductResult()
        {
            data = new List<GarmentProductViewModel>();
        }
        public IList<GarmentProductViewModel> data { get; set; }
    }

    public class SingleGarmentProductResult : BaseResult
    {
        public GarmentProductViewModel data { get; set; }
    }

    public class GarmentProductViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Uom UOM { get; set; }
        public string ProductType { get; set; }
        public string Composition { get; set; }
        public string Const { get; set; }
        public string Yarn { get; set; }
        public string Width { get; set; }
        public string Tags { get; set; }
        public string Remark { get; set; }
    }
}
