using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetCuttingOutForTraceable
{
    public class GetCuttingOutForTraceableDto
    {
        public GetCuttingOutForTraceableDto()
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

        public GetCuttingOutForTraceableDto(GetCuttingOutForTraceableDto getCuttingOutForTraceableDto)
        {
            CutOutNo = getCuttingOutForTraceableDto.CutOutNo;
            CuttingOutType = getCuttingOutForTraceableDto.CuttingOutType;
            UnitFrom = getCuttingOutForTraceableDto.UnitFrom;
            CuttingOutDate = getCuttingOutForTraceableDto.CuttingOutDate;
            RONo = getCuttingOutForTraceableDto.RONo;
            Article = getCuttingOutForTraceableDto.Article;
            Unit = getCuttingOutForTraceableDto.Unit;
            Comodity = getCuttingOutForTraceableDto.Comodity;
            TotalCuttingOutQuantity = getCuttingOutForTraceableDto.TotalCuttingOutQuantity;
            
        }
    }
}
