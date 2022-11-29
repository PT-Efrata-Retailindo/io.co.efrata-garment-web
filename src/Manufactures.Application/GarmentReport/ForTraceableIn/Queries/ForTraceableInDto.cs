using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentReport.ForTraceableIn.Queries
{
    public class ForTraceableInDto
    {
        public ForTraceableInDto()
        {

        }
        public string RoJob { get; internal set; }
        public string CutOutType { get; internal set; }
        public double CutOutQuantity { get; internal set; }
        public string FinishingInType { get; internal set; }
        public string FinishingTo { get; internal set; }
        public double FinishingOutQuantity { get; internal set; }
        public string ExpenditureType { get; internal set; }
        public string Invoice { get; internal set; }
        public double ExpenditureQuantity { get; internal set; }



        public ForTraceableInDto(ForTraceableInDto forTraceableInDto)
        {
            RoJob = forTraceableInDto.RoJob;
            CutOutType = forTraceableInDto.CutOutType;
            CutOutQuantity = forTraceableInDto.CutOutQuantity;
        }

    }
}
