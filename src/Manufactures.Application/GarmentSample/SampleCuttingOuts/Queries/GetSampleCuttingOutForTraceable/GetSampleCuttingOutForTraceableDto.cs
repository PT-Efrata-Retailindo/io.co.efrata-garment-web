using System;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.GetSampleCuttingOutForTraceable
{
    public class GetSampleCuttingOutForTraceableDto
    {
        public GetSampleCuttingOutForTraceableDto()
        {
        }
        public string CutOutNo { get; internal set; }
        public string CuttingOutType { get; internal set; }

        public UnitDepartment UnitFrom { get; internal set; }
        public DateTimeOffset CuttingOutDate { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public UnitDepartment Unit { get; internal set; }
        public GarmentComodity Comodity { get; internal set; }

        public double TotalCuttingOutQuantity { get; internal set; }

        public GetSampleCuttingOutForTraceableDto(GetSampleCuttingOutForTraceableDto GetSampleCuttingOutForTraceableDto)
        {
            CutOutNo = GetSampleCuttingOutForTraceableDto.CutOutNo;
            CuttingOutType = GetSampleCuttingOutForTraceableDto.CuttingOutType;
            UnitFrom = GetSampleCuttingOutForTraceableDto.UnitFrom;
            CuttingOutDate = GetSampleCuttingOutForTraceableDto.CuttingOutDate;
            RONo = GetSampleCuttingOutForTraceableDto.RONo;
            Article = GetSampleCuttingOutForTraceableDto.Article;
            Unit = GetSampleCuttingOutForTraceableDto.Unit;
            Comodity = GetSampleCuttingOutForTraceableDto.Comodity;
            TotalCuttingOutQuantity = GetSampleCuttingOutForTraceableDto.TotalCuttingOutQuantity;

        }
    }
}
