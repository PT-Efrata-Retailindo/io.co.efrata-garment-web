using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries
{
    public class SampleCuttingOutListViewModel
    {
        public List<GarmentSampleCuttingOutListDto> data { get; set; }
        public int total { get; set; }
        public double totalQty { get; internal set; }
    }
}
