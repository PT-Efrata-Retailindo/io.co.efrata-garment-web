using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetWIP
{
    public class GarmentWIPListViewModel
    {
        public List<GarmentWIPDto> garmentWIP { get; set; }
        public int count { get; set; }
    }
}
