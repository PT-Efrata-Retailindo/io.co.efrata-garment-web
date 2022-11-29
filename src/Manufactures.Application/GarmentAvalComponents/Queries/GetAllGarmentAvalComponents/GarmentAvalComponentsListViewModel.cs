using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents
{
    public class GarmentAvalComponentsListViewModel
    {
        public List<GarmentAvalComponentDto> GarmentAvalComponents { get; set; }
        public int total { get; set; }
    }
}
