using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport
{
    public class GarmentSubconContactReportListViewModel
    {


        public List<GarmentSubconContactReportDto> garmentSubconContactReportDto { get; set; }
        public int count { get; set; }

        //public GarmentSubconContactReportListViewModel()
        //{
        //}

        //public string ContractType { get; set; }
        //public string ContractNo { get; set; }
        //public string AgreementNo { get; set; }
        //public int SupplierId { get; set; }
        //public string SupplierCode { get; set; }
        //public string SupplierName { get; set; }
        //public string JobType { get; set; }
        //public string BPJNo { get; set; }
        //public string FinishedGoodType { get; set; }
        //public double Quantity { get; set; }
        //public DateTimeOffset DueDate { get; set; }
        //public DateTimeOffset ContractDate { get; set; }
        //public bool IsUsed { get; set; }
        //public int BuyerId { get; set; }
        //public string BuyerCode { get; set; }
        //public string BuyerName { get; set; }
        //public string SubconCategory { get; set; }
        //public int UomId { get; set; }
        //public string UomUnit { get; set; }
        //public string SKEPNo { get; set; }
        //public DateTimeOffset AgreementDate { get; set; }

        //public int count { get; set; }

        //public GarmentSubconContactReportListViewModel(GarmentSubconContactReportListViewModel garmentSubconContactReportListViewModel) 
        //{
        //    ContractType = garmentSubconContactReportListViewModel.ContractType;
        //    ContractNo = garmentSubconContactReportListViewModel.ContractNo;
        //    AgreementNo = garmentSubconContactReportListViewModel.AgreementNo;
        //    SupplierId = garmentSubconContactReportListViewModel.SupplierId;
        //    SupplierCode = garmentSubconContactReportListViewModel.SupplierCode;
        //    SupplierName = garmentSubconContactReportListViewModel.SupplierName;
        //    JobType = garmentSubconContactReportListViewModel.JobType;
        //    BPJNo = garmentSubconContactReportListViewModel.BPJNo;
        //    FinishedGoodType = garmentSubconContactReportListViewModel.FinishedGoodType;
        //    Quantity = garmentSubconContactReportListViewModel.Quantity;
        //    DueDate = garmentSubconContactReportListViewModel.DueDate;
        //    ContractDate = garmentSubconContactReportListViewModel.ContractDate;
        //    IsUsed = garmentSubconContactReportListViewModel.IsUsed;
        //    BuyerId = garmentSubconContactReportListViewModel.BuyerId;
        //    BuyerName = garmentSubconContactReportListViewModel.BuyerName;
        //    BuyerCode = garmentSubconContactReportListViewModel.BuyerCode;
        //    SubconCategory = garmentSubconContactReportListViewModel.SubconCategory;
        //    UomId = garmentSubconContactReportListViewModel.UomId;
        //    UomUnit = garmentSubconContactReportListViewModel.UomUnit;
        //    SKEPNo = garmentSubconContactReportListViewModel.SKEPNo;
        //    AgreementDate = garmentSubconContactReportListViewModel.AgreementDate;
        //}

        //public static implicit operator GarmentSubconContactReportListViewModel(List<GarmentSubconContactReportListViewModel> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
