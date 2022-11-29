using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetAllGarmentSampleAvalComponents
{
    public class GarmentSampleAvalComponentsListViewModel
    {
        public List<GarmentSampleAvalComponentDto> GarmentSampleAvalComponents { get; set; }
        public int total { get; set; }
    }
}
