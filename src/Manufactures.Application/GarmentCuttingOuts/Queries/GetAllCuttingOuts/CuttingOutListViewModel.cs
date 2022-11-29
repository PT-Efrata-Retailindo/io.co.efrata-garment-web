using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class CuttingOutListViewModel
    {
        public List<GarmentCuttingOutListDto> data { get; set; }
        public int total { get; set; }
        public double totalQty { get; internal set; }
    }
}
