using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport
{
    public class GarmentSubconContactReportDto
    {
        public GarmentSubconContactReportDto()
        {
        }

        public string ContractType { get; internal set; }
        public string ContractNo { get; internal set; }
        public string AgreementNo { get; internal set; }
        public int SupplierId { get; internal set; }
        public string SupplierCode { get; internal set; }
        public string SupplierName { get; internal set; }
        public string JobType { get; internal set; }
        public string BPJNo { get; internal set; }
        public string FinishedGoodType { get; internal set; }
        public double Quantity { get; internal set; }
        public DateTimeOffset DueDate { get; internal set; }
        public DateTimeOffset ContractDate { get; internal set; }
        public bool IsUsed { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public string SubconCategory { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string SKEPNo { get; internal set; }
        public DateTimeOffset AgreementDate { get; internal set; }

        //public int count { get; set; }

        public GarmentSubconContactReportDto(GarmentSubconContactReportDto garmentSubconContactReportDto)
        {
            ContractType = garmentSubconContactReportDto.ContractType;
            ContractNo = garmentSubconContactReportDto.ContractNo;
            AgreementNo = garmentSubconContactReportDto.AgreementNo;
            SupplierId = garmentSubconContactReportDto.SupplierId;
            SupplierCode = garmentSubconContactReportDto.SupplierCode;
            SupplierName = garmentSubconContactReportDto.SupplierName;
            JobType = garmentSubconContactReportDto.JobType;
            BPJNo = garmentSubconContactReportDto.BPJNo;
            FinishedGoodType = garmentSubconContactReportDto.FinishedGoodType;
            Quantity = garmentSubconContactReportDto.Quantity;
            DueDate = garmentSubconContactReportDto.DueDate;
            ContractDate = garmentSubconContactReportDto.ContractDate;
            IsUsed = garmentSubconContactReportDto.IsUsed;
            BuyerId = garmentSubconContactReportDto.BuyerId;
            BuyerName = garmentSubconContactReportDto.BuyerName;
            BuyerCode = garmentSubconContactReportDto.BuyerCode;
            SubconCategory = garmentSubconContactReportDto.SubconCategory;
            UomId = garmentSubconContactReportDto.UomId;
            UomUnit = garmentSubconContactReportDto.UomUnit;
            SKEPNo = garmentSubconContactReportDto.SKEPNo;
            AgreementDate = garmentSubconContactReportDto.AgreementDate;
        }
    }
}
