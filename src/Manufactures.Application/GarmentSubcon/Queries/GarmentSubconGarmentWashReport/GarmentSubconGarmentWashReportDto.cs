using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconGarmentWashReport
{
    public class GarmentSubconGarmentWashReportDto
    {
        public GarmentSubconGarmentWashReportDto()
        {
        }

        public Guid SSCSId { get; internal set; }
        public string SSCSNo { get; internal set; }
        public DateTimeOffset SSCSDate { get; internal set; }

        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }

        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }

        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }

        public string DesignColour { get; internal set; }

        public double Quantity { get; internal set; }

        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
    
        public GarmentSubconGarmentWashReportDto(GarmentSubconGarmentWashReportDto garmentSubconGarmentWashReportDto)
        {
            SSCSId = garmentSubconGarmentWashReportDto.SSCSId;
            SSCSNo = garmentSubconGarmentWashReportDto.SSCSNo;
            SSCSDate = garmentSubconGarmentWashReportDto.SSCSDate;
            BuyerId = garmentSubconGarmentWashReportDto.BuyerId;
            BuyerCode = garmentSubconGarmentWashReportDto.BuyerCode;
            BuyerName = garmentSubconGarmentWashReportDto.BuyerName;
            ComodityId = garmentSubconGarmentWashReportDto.ComodityId;
            ComodityCode = garmentSubconGarmentWashReportDto.ComodityCode;
            ComodityName = garmentSubconGarmentWashReportDto.ComodityName;
            UnitId = garmentSubconGarmentWashReportDto.UnitId;
            UnitCode = garmentSubconGarmentWashReportDto.UnitCode;
            UnitName = garmentSubconGarmentWashReportDto.UnitName;
            DesignColour = garmentSubconGarmentWashReportDto.DesignColour;
            Quantity = garmentSubconGarmentWashReportDto.Quantity;
            UomId = garmentSubconGarmentWashReportDto.UomId;
            UomUnit = garmentSubconGarmentWashReportDto.UomUnit;          
        }
    }
}
