using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport
{
    public class GarmentRealizationSubconReportListViewModel
    {
        public List<GarmentRealizationSubconReportDto> garmentRealizationSubconReportDtos { get; set; }
        public List<GarmentRealizationSubconReportDto> garmentRealizationSubconReportDtosOUT { get; set; }
        public int count { get; set; }
    }
}
