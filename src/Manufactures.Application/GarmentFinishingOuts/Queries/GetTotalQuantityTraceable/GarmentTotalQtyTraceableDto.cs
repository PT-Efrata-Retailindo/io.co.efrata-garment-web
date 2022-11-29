using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable
{
    public class GarmentTotalQtyTraceableDto
    {
        public GarmentTotalQtyTraceableDto()
        {
        }

        public string roJob { get; internal set; }
        public string finishingTo { get; internal set; }
        public string finishingInType { get; internal set; }
        public double totalQty { get; internal set; }
        public GarmentTotalQtyTraceableDto(GarmentTotalQtyTraceableDto totalQtyTraceableDto)
        {

            roJob = totalQtyTraceableDto.roJob;
            finishingTo = totalQtyTraceableDto.finishingTo;
            finishingInType = totalQtyTraceableDto.finishingInType;
            totalQty = totalQtyTraceableDto.totalQty;
        }
    }
}
