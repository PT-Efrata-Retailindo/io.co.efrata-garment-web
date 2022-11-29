using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable
{
    public class GarmentTotalQtyTraceableListViewModel
    {
       public List<GarmentTotalQtyTraceableDto> garmentTotalQtyTraceables { get; set; }
       public int count { get; set; }
    }
}
