using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport
{
    public class GarmentSubconDLORawMaterialReportDto
    {
        public GarmentSubconDLORawMaterialReportDto()
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
        public string UENNo { get; internal set; }
        public DateTimeOffset UENDate { get; internal set; }
        public string UnitSenderName { get; internal set; }
        public string UnitRequestName { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColour { get; internal set; }
        public decimal Quantity { get; internal set; }
        public string UomUnit { get; internal set; }
    
        public GarmentSubconDLORawMaterialReportDto(GarmentSubconDLORawMaterialReportDto garmentSubconDLORawMaterialReportDto)
        {
            DLType = garmentSubconDLORawMaterialReportDto.DLType;
            DLNo = garmentSubconDLORawMaterialReportDto.DLNo;
            DLDate = garmentSubconDLORawMaterialReportDto.DLDate;
            ContractNo = garmentSubconDLORawMaterialReportDto.ContractNo;
            ContractType = garmentSubconDLORawMaterialReportDto.ContractType;
            SubConCategory = garmentSubconDLORawMaterialReportDto.SubConCategory;
            SubConNo = garmentSubconDLORawMaterialReportDto.SubConNo;
            SubConDate = garmentSubconDLORawMaterialReportDto.SubConDate;
            UENNo = garmentSubconDLORawMaterialReportDto.UENNo;
            UENDate = garmentSubconDLORawMaterialReportDto.UENDate;
            UnitSenderName = garmentSubconDLORawMaterialReportDto.UnitSenderName;
            UnitSenderName = garmentSubconDLORawMaterialReportDto.UnitSenderName;
            ProductCode = garmentSubconDLORawMaterialReportDto.ProductCode;
            ProductName = garmentSubconDLORawMaterialReportDto.ProductName;
            DesignColour = garmentSubconDLORawMaterialReportDto.DesignColour;
            Quantity = garmentSubconDLORawMaterialReportDto.Quantity;
            UomUnit = garmentSubconDLORawMaterialReportDto.UomUnit;          
        }
    }
}
