using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconContractDto : BaseDto
    {
        public GarmentSubconContractDto(GarmentSubconContract garmentSubconContract)
        {
            Id = garmentSubconContract.Identity;
            ContractType = garmentSubconContract.ContractType;
            ContractNo = garmentSubconContract.ContractNo;
            AgreementNo = garmentSubconContract.AgreementNo;
            JobType = garmentSubconContract.JobType;
            BPJNo = garmentSubconContract.BPJNo;
            Supplier = new Supplier(garmentSubconContract.SupplierId.Value, garmentSubconContract.SupplierCode, garmentSubconContract.SupplierName);
            FinishedGoodType = garmentSubconContract.FinishedGoodType;
            Quantity = garmentSubconContract.Quantity;
            DueDate = garmentSubconContract.DueDate;
            ContractDate = garmentSubconContract.ContractDate;
            IsUsed = garmentSubconContract.IsUsed;
            Buyer = new Buyer(garmentSubconContract.BuyerId.Value, garmentSubconContract.BuyerCode, garmentSubconContract.BuyerName);
            SubconCategory = garmentSubconContract.SubconCategory;
            Uom = new Uom(garmentSubconContract.UomId.Value, garmentSubconContract.UomUnit);
            SKEPNo = garmentSubconContract.SKEPNo;
            AgreementDate = garmentSubconContract.AgreementDate;
            CIF = garmentSubconContract.CIF;
            Items = new List<GarmentSubconContractItemDto>();
        }
        public Guid Id { get; internal set; }
        public string ContractType { get;  set; }
        public string ContractNo { get;  set; }
        public string AgreementNo { get;  set; }
        public Supplier Supplier { get;  set; }
        public string JobType { get;  set; }
        public string BPJNo { get;  set; }
        public string FinishedGoodType { get;  set; }
        public double Quantity { get;  set; }
        public DateTimeOffset DueDate { get;  set; }
        public DateTimeOffset ContractDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }

        public string SubconCategory { get; set; }
        public Uom Uom { get; set; }
        public string SKEPNo { get; set; }
        public DateTimeOffset AgreementDate { get; set; }
        public int CIF { get; set; }
        public List<GarmentSubconContractItemDto> Items { get; set; }
    }

}
