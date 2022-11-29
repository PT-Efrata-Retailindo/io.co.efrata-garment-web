using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOSewingReport
{
    public class GarmentSubconDLOSewingReportDto
    {
        public GarmentSubconDLOSewingReportDto()
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
        public string RONo { get; internal set; }
        public string PONo { get; internal set; }
        public string UnitName { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComoditytName { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string Size { get; internal set; }
        public string Colour { get; internal set; }
        public double Quantity { get; internal set; }
        public string UomUnit { get; internal set; }

        public GarmentSubconDLOSewingReportDto(GarmentSubconDLOSewingReportDto garmentSubconDLOSewingReportDto)
        {
            DLType = garmentSubconDLOSewingReportDto.DLType;
            DLNo = garmentSubconDLOSewingReportDto.DLNo;
            DLDate = garmentSubconDLOSewingReportDto.DLDate;
            ContractNo = garmentSubconDLOSewingReportDto.ContractNo;
            ContractType = garmentSubconDLOSewingReportDto.ContractType;
            SubConCategory = garmentSubconDLOSewingReportDto.SubConCategory;
            SubConNo = garmentSubconDLOSewingReportDto.SubConNo;
            SubConDate = garmentSubconDLOSewingReportDto.SubConDate;
            RONo = garmentSubconDLOSewingReportDto.UnitName;
            PONo = garmentSubconDLOSewingReportDto.UnitName;
            UnitName = garmentSubconDLOSewingReportDto.UnitName;
            ComodityCode = garmentSubconDLOSewingReportDto.ComodityCode;
            ComoditytName = garmentSubconDLOSewingReportDto.ComoditytName;
            ProductCode = garmentSubconDLOSewingReportDto.ProductCode;
            ProductName = garmentSubconDLOSewingReportDto.ProductName;
            Size = garmentSubconDLOSewingReportDto.ProductName;
            Colour = garmentSubconDLOSewingReportDto.Colour;
            Quantity = garmentSubconDLOSewingReportDto.Quantity;
            UomUnit = garmentSubconDLOSewingReportDto.UomUnit;          
        }
    }
}
