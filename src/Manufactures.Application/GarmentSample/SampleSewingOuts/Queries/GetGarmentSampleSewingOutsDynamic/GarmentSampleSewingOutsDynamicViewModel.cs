using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsDynamic
{
    public class GarmentSampleSewingOutsDynamicViewModel
    {
        public int total { get; private set; }
        public List<dynamic> data { get; private set; }

        public GarmentSampleSewingOutsDynamicViewModel(int total, List<dynamic> data)
        {
            this.total = total;
            this.data = data;
        }
    }
}
