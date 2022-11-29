using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport
{
    public class GarmentRealizationSubconReportDto
    {
        public GarmentRealizationSubconReportDto()
        {
        }

        public string bcNoOut { get; internal set; }
        public DateTimeOffset bcDateOut { get; internal set; }
        public double quantityOut { get; internal set; }
        public string uomOut { get; internal set; }
        public string jobType { get; internal set; }
        public string subconNo { get; internal set; }
        public string bpjNo { get; internal set; }
        public DateTimeOffset dueDate { get; internal set; }
        //public DateTimeOffset bcDateIn { get; internal set; }
        //public string quantityIn { get; internal set; }
        //public string uomIn { get; internal set; }

        public GarmentRealizationSubconReportDto(GarmentRealizationSubconReportDto garmentRealizationSubconReportDto)
        {
            bcNoOut = garmentRealizationSubconReportDto.bcNoOut;
            bcDateOut = garmentRealizationSubconReportDto.bcDateOut;
            quantityOut = garmentRealizationSubconReportDto.quantityOut;
            uomOut = garmentRealizationSubconReportDto.uomOut;
            jobType = garmentRealizationSubconReportDto.jobType;
            subconNo = garmentRealizationSubconReportDto.subconNo;
            bpjNo = garmentRealizationSubconReportDto.bpjNo;
            dueDate = garmentRealizationSubconReportDto.dueDate;
        }
    }
}
