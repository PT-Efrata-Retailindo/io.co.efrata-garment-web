using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOCuttingSewingReport
{
    public class GarmentSubconDLOCuttingSewingReportDto
    {
        public GarmentSubconDLOCuttingSewingReportDto()
        {
        }

        //select a.DLType, a.DLNo, a.DLDate, a.ContractNo, a.ContractType, a.SubconCategory,
        //a.UENNo, a.PONo, b.ProductCode, b.ProductName, b.DesignColor, b.Quantity, bb.UomOutUnit
        //from GarmentSubconDeliveryLetterOuts a join
        //GarmentSubconDeliveryLetterOutItems b on a.[Identity]= b.SubconDeliveryLetterOutId
        //where a.UENId >0 and b.UENItemId >0 and a.Deleted= 0 and b.Deleted= 0 and a.ContractType= 'SUBCON GARMENT' and a.SubConCategory= 'SUBCON CUTTING SEWING'
        //order by a.UENId

        //select a.Id, a.UENNo, a.ExpenditureDate, a.UnitRequestName, a.UnitRequestName, b.FabricType, b.RONo, b.Quantity, b.UomUnit
        //from GarmentUnitExpenditureNotes a
        //join GarmentUnitExpenditureNoteItems b on a.Id= b.UENId
        //where a.IsDeleted= 0 and b.IsDeleted= 0 and a.Id in (195149,195091,195081)
        //order by a.Id
        
        public string DLType { get; internal set; }
        public string DLNo { get; internal set; }
        public DateTimeOffset DLDate { get; internal set; }
        public string ContractNo { get; internal set; }
        public string ContractType { get; internal set; }
        public string SubConCategory { get; internal set; }
        public string UENNo { get; internal set; }
        public DateTimeOffset UENDate { get; internal set; }
        public string RONo { get; internal set; }
        public string PONo { get; internal set; }
        public string UnitRequestName { get; internal set; }
        public string UnitSenderName { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string FabricType { get; internal set; }
        public string Colour { get; internal set; }        
        public double QtyUEN { get; internal set; }
        public string UomUEN { get; internal set; }
        public double QtyOut { get; internal set; }
        public string UomUnit { get; internal set; }

        public GarmentSubconDLOCuttingSewingReportDto(GarmentSubconDLOCuttingSewingReportDto garmentSubconDLOCuttingSewingReportDto)
        {
            DLType = garmentSubconDLOCuttingSewingReportDto.DLType;
            DLNo = garmentSubconDLOCuttingSewingReportDto.DLNo;
            DLDate = garmentSubconDLOCuttingSewingReportDto.DLDate;
            ContractNo = garmentSubconDLOCuttingSewingReportDto.ContractNo;
            ContractType = garmentSubconDLOCuttingSewingReportDto.ContractType;
            SubConCategory = garmentSubconDLOCuttingSewingReportDto.SubConCategory;
            UENNo = garmentSubconDLOCuttingSewingReportDto.UENNo;
            UENDate = garmentSubconDLOCuttingSewingReportDto.UENDate;
            RONo = garmentSubconDLOCuttingSewingReportDto.RONo;
            PONo = garmentSubconDLOCuttingSewingReportDto.PONo;
            UnitRequestName = garmentSubconDLOCuttingSewingReportDto.UnitRequestName;
            UnitSenderName = garmentSubconDLOCuttingSewingReportDto.UnitSenderName;
            ProductCode = garmentSubconDLOCuttingSewingReportDto.ProductCode;
            ProductName = garmentSubconDLOCuttingSewingReportDto.ProductName;
            FabricType = garmentSubconDLOCuttingSewingReportDto.FabricType;
            Colour = garmentSubconDLOCuttingSewingReportDto.Colour;
            QtyUEN = garmentSubconDLOCuttingSewingReportDto.QtyUEN;
            UomUEN = garmentSubconDLOCuttingSewingReportDto.UomUEN;
            QtyOut = garmentSubconDLOCuttingSewingReportDto.QtyOut;
            UomUnit = garmentSubconDLOCuttingSewingReportDto.UomUnit;
        }
    }
}
