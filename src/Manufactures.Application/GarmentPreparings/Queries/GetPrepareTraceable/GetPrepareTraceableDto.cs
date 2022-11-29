using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetPrepareTraceable
{
    public class GetPrepareTraceableDto
    {
        public GetPrepareTraceableDto()
        {
        }

        //public long UENId { get; internal set; }
        //public string UENNo { get; internal set; }
        public string RONo { get; internal set; }
        //public string Article { get; internal set; }
        public string ProductCode { get; internal set; }
        //public bool IsCuttingIn { get; internal set; }
        //public string CreatedBy { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public double Quantity { get; internal set; }
        //public long UENItemId { get; internal set; }
        public GetPrepareTraceableDto(GetPrepareTraceableDto getPrepareTraceableDto)
        {
            //UENId = getPrepareTraceableDto.UENId;
            //UENNo = getPrepareTraceableDto.UENNo;
            RONo = getPrepareTraceableDto.RONo;
            //Article = getPrepareTraceableDto.Article;
            //IsCuttingIn = getPrepareTraceableDto.IsCuttingIn;
            //CreatedBy = getPrepareTraceableDto.CreatedBy;
            RemainingQuantity = getPrepareTraceableDto.RemainingQuantity;
            Quantity = getPrepareTraceableDto.Quantity;
            ProductCode = getPrepareTraceableDto.ProductCode;
            //UENItemId = getPrepareTraceableDto.UENItemId;
        }
    }
}
