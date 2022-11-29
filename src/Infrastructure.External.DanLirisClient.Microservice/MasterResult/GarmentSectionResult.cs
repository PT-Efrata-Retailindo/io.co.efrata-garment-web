using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class GarmentSectionResult : BaseResult
    {
        public IList<GarmentSectionViewModel> data { get; set; }
        public GarmentSectionResult()
        {
            data = new List<GarmentSectionViewModel>();
        }


        public class GarmentSectionViewModel
        {
            public int Id { get; set; }

            public string Name { get; set; }

        }
    }
}
