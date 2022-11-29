using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleSewingIns.Queries
{
    public class SampleSewingInListViewModel
    {
        public List<GarmentSampleSewingInListDto> data { get; set; }
        public int total { get; set; }
        public double totalQty { get; internal set; }
    }
}
