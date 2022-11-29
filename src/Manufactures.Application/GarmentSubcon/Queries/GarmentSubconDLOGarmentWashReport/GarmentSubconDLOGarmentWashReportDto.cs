using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOGarmentWashReport
{
    public class GarmentSubconDLOGarmentWashReportDto
    {
        public GarmentSubconDLOGarmentWashReportDto()
        {
        }

        public string DLType { get; internal set; }
        public string DLNo { get; internal set; }
        public DateTimeOffset DLDate { get; internal set; }
        public string ContractNo { get; internal set; }
        public string ContractType { get; internal set; }
        public string SubConCategory { get; internal set; }
        public string SubConNo { get; internal set; }
        public DateTimeOffset SubConDate { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComoditytName { get; internal set; }
        public string RONo { get; internal set; }
        public string UnitName { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string Colour { get; internal set; }
        public double Quantity { get; internal set; }
        public string UomUnit { get; internal set; }

        public GarmentSubconDLOGarmentWashReportDto(GarmentSubconDLOGarmentWashReportDto garmentSubconDLOGarmentWashReportDto)
        {
            DLType = garmentSubconDLOGarmentWashReportDto.DLType;
            DLNo = garmentSubconDLOGarmentWashReportDto.DLNo;
            DLDate = garmentSubconDLOGarmentWashReportDto.DLDate;
            ContractNo = garmentSubconDLOGarmentWashReportDto.ContractNo;
            ContractType = garmentSubconDLOGarmentWashReportDto.ContractType;
            SubConCategory = garmentSubconDLOGarmentWashReportDto.SubConCategory;
            SubConNo = garmentSubconDLOGarmentWashReportDto.SubConNo;
            SubConDate = garmentSubconDLOGarmentWashReportDto.SubConDate;
            BuyerCode = garmentSubconDLOGarmentWashReportDto.BuyerCode;
            BuyerName = garmentSubconDLOGarmentWashReportDto.BuyerName;
            ComodityCode = garmentSubconDLOGarmentWashReportDto.ComodityCode;
            ComoditytName = garmentSubconDLOGarmentWashReportDto.ComoditytName;
            UnitName = garmentSubconDLOGarmentWashReportDto.UnitName;
            RONo = garmentSubconDLOGarmentWashReportDto.UnitName;
            ProductCode = garmentSubconDLOGarmentWashReportDto.ProductCode;
            ProductName = garmentSubconDLOGarmentWashReportDto.ProductName;
            Colour = garmentSubconDLOGarmentWashReportDto.Colour;
            Quantity = garmentSubconDLOGarmentWashReportDto.Quantity;
            UomUnit = garmentSubconDLOGarmentWashReportDto.UomUnit;          
        }
    }
}
