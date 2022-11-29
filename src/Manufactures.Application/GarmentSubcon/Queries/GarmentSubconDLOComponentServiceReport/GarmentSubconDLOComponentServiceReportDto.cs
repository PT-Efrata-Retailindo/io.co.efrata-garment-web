using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOComponentServiceReport
{
    public class GarmentSubconDLOComponentServiceReportDto
    {
        public GarmentSubconDLOComponentServiceReportDto()
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
        public string SubConType { get; internal set; }
        public string UnitName { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComoditytName { get; internal set; }
        public string RONo { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string Size { get; internal set; }
        public string Colour { get; internal set; }
        public double Quantity { get; internal set; }
        public string UomUnit { get; internal set; }

        public GarmentSubconDLOComponentServiceReportDto(GarmentSubconDLOComponentServiceReportDto garmentSubconDLOComponentServiceReportDto)
        {
            DLType = garmentSubconDLOComponentServiceReportDto.DLType;
            DLNo = garmentSubconDLOComponentServiceReportDto.DLNo;
            DLDate = garmentSubconDLOComponentServiceReportDto.DLDate;
            ContractNo = garmentSubconDLOComponentServiceReportDto.ContractNo;
            ContractType = garmentSubconDLOComponentServiceReportDto.ContractType;
            SubConCategory = garmentSubconDLOComponentServiceReportDto.SubConCategory;
            SubConNo = garmentSubconDLOComponentServiceReportDto.SubConNo;
            SubConDate = garmentSubconDLOComponentServiceReportDto.SubConDate;
            SubConType = garmentSubconDLOComponentServiceReportDto.SubConType;
            UnitName = garmentSubconDLOComponentServiceReportDto.UnitName;
            BuyerCode = garmentSubconDLOComponentServiceReportDto.BuyerCode;
            BuyerName = garmentSubconDLOComponentServiceReportDto.BuyerName;
            ComodityCode = garmentSubconDLOComponentServiceReportDto.ComodityCode;
            ComoditytName = garmentSubconDLOComponentServiceReportDto.ComoditytName;
            RONo = garmentSubconDLOComponentServiceReportDto.RONo;
            ProductCode = garmentSubconDLOComponentServiceReportDto.ProductCode;
            ProductName = garmentSubconDLOComponentServiceReportDto.ProductName;
            Colour = garmentSubconDLOComponentServiceReportDto.Colour;
            Size = garmentSubconDLOComponentServiceReportDto.Size;
            Quantity = garmentSubconDLOComponentServiceReportDto.Quantity;
            UomUnit = garmentSubconDLOComponentServiceReportDto.UomUnit;          
        }
    }
}
