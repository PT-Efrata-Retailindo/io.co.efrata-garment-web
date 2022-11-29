using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsByRONo
{
    public class GarmentSampleSewingOutsByRONoViewModel
    {
        public List<GarmentSampleSewingOutByRONoDto> data { get; private set; }

        public GarmentSampleSewingOutsByRONoViewModel(List<GarmentSampleSewingOutByRONoDto> data)
        {
            this.data = data;
        }
    }
}
