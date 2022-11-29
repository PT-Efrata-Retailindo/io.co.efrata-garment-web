using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubconCuttings.Queries.GetAllGarmentSubconCuttings
{
    public class GetGarmentSubconCuttingListViewModel
    {
        public List<GarmentSubconCuttingDto> data { get; set; }
        public int total { get; set; }
    }
}
