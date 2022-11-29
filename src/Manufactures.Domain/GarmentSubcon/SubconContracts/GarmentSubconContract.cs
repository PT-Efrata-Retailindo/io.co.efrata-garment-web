using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts
{
    public class GarmentSubconContract : AggregateRoot<GarmentSubconContract, GarmentSubconContractReadModel>
    {
        public string ContractType { get; private set; }
        public string ContractNo { get; private set; }
        public string AgreementNo { get; private set; }
        public SupplierId SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public string JobType { get; private set; }
        public string BPJNo { get; private set; }
        public string FinishedGoodType { get; private set; }
        public double Quantity { get; private set; }
        public DateTimeOffset DueDate { get; private set; }
        public DateTimeOffset ContractDate { get; private set; }
        public bool IsUsed { get; private set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }

        public string SubconCategory { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string SKEPNo { get; private set; }
        public DateTimeOffset AgreementDate { get; private set; }
        public int CIF { get; private set; }

        public GarmentSubconContract(GarmentSubconContractReadModel readModel) : base(readModel)
        {
            ContractType = readModel.ContractType;
            ContractNo = readModel.ContractNo;
            AgreementNo = readModel.AgreementNo;
            SupplierId = new SupplierId(readModel.SupplierId);
            SupplierCode = readModel.SupplierCode;
            SupplierName = readModel.SupplierName;
            JobType = readModel.JobType;
            BPJNo = readModel.BPJNo;
            FinishedGoodType = readModel.FinishedGoodType;
            Quantity = readModel.Quantity;
            DueDate = readModel.DueDate;
            ContractDate = readModel.ContractDate;
            IsUsed = readModel.IsUsed;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
            SubconCategory = readModel.SubconCategory;
            UomId = new UomId( readModel.UomId);
            UomUnit = readModel.UomUnit;
            SKEPNo = readModel.SKEPNo;
            AgreementDate = readModel.AgreementDate;
            CIF = readModel.CIF;
        }

        public GarmentSubconContract(Guid identity, string contractType, string contractNo, string agreementNo, SupplierId supplierId, string supplierCode, string supplierName, string jobType, string bPJNo, string finishedGoodType, double quantity, DateTimeOffset dueDate, DateTimeOffset contractDate, bool isUsed, BuyerId buyerId, string buyerCode, string buyerName, string subconCategory, UomId uomId, string uomUnit, string sKEPNo, DateTimeOffset agreementDate, int cif) : base(identity)
        {
            Identity = identity;
            ContractType = contractType;
            ContractNo = contractNo;
            AgreementNo = agreementNo;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            JobType = jobType;
            BPJNo = bPJNo;
            FinishedGoodType = finishedGoodType;
            Quantity = quantity;
            DueDate = dueDate;
            ContractDate = contractDate;
            IsUsed = isUsed;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;
            SubconCategory = subconCategory;
            UomId = uomId;
            UomUnit = uomUnit;
            SKEPNo = sKEPNo;
            AgreementDate = agreementDate;
            CIF = cif;

            ReadModel = new GarmentSubconContractReadModel(Identity)
            {
                ContractType = ContractType,
                ContractNo = ContractNo,
                AgreementNo = AgreementNo,
                SupplierId = SupplierId.Value,
                SupplierCode = SupplierCode,
                SupplierName = SupplierName,
                JobType = JobType,
                BPJNo = BPJNo,
                FinishedGoodType = FinishedGoodType,
                Quantity = Quantity,
                DueDate = DueDate,
                ContractDate = ContractDate,
                IsUsed = IsUsed,
                BuyerId = BuyerId.Value,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName,
                SubconCategory = subconCategory,
                UomId = uomId.Value,
                UomUnit = uomUnit,
                SKEPNo = sKEPNo,
                AgreementDate = agreementDate,
                CIF = cif,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconContractPlaced(Identity));
        }

        protected override GarmentSubconContract GetEntity()
        {
            return this;
        }

        public void SetSupplierId(SupplierId SupplierId)
        {
            if (this.SupplierId != SupplierId)
            {
                this.SupplierId = SupplierId;
                ReadModel.SupplierId = SupplierId.Value;
            }
        }
        public void SetSupplierCode(string SupplierCode)
        {
            if (this.SupplierCode != SupplierCode)
            {
                this.SupplierCode = SupplierCode;
                ReadModel.SupplierCode = SupplierCode;
            }
        }
        public void SetSupplierName(string SupplierName)
        {
            if (this.SupplierName != SupplierName)
            {
                this.SupplierName = SupplierName;
                ReadModel.SupplierName = SupplierName;
            }
        }
        public void SetJobType(string JobType)
        {
            if (this.JobType != JobType)
            {
                this.JobType = JobType;
                ReadModel.JobType = JobType;
            }
        }
        public void SetContractNo(string ContractNo)
        {
            if (this.ContractNo != ContractNo)
            {
                this.ContractNo = ContractNo;
                ReadModel.ContractNo = ContractNo;
            }
        }
        public void SetContractType(string ContractType)
        {
            if (this.ContractType != ContractType)
            {
                this.ContractType = ContractType;
                ReadModel.ContractType = ContractType;
            }
        }
        public void SetAgreementNo(string AgreementNo)
        {
            if (this.AgreementNo != AgreementNo)
            {
                this.AgreementNo = AgreementNo;
                ReadModel.AgreementNo = AgreementNo;
            }
        }
        public void SetDueDate(DateTimeOffset DueDate)
        {
            if (this.DueDate != DueDate)
            {
                this.DueDate = DueDate;
                ReadModel.DueDate = DueDate;
            }
        }
        public void SetBPJNo(string BPJNo)
        {
            if (this.BPJNo != BPJNo)
            {
                this.BPJNo = BPJNo;
                ReadModel.BPJNo = BPJNo;
            }
        }
        public void SetFinishedGoodType(string FinishedGoodType)
        {
            if (this.FinishedGoodType != FinishedGoodType)
            {
                this.FinishedGoodType = FinishedGoodType;
                ReadModel.FinishedGoodType = FinishedGoodType;
            }
        }
        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
        public void SetContractDate(DateTimeOffset ContractDate)
        {
            if (this.ContractDate != ContractDate)
            {
                this.ContractDate = ContractDate;
                ReadModel.ContractDate = ContractDate;
            }
        }
        public void SetIsUsed(bool Isused)
        {
            if (this.IsUsed != Isused)
            {
                this.IsUsed = Isused;
                ReadModel.IsUsed = Isused;
            }
        }
        public void SetBuyerId(BuyerId SupplierId)
        {
            if (this.BuyerId != BuyerId)
            {
                this.BuyerId = BuyerId;
                ReadModel.BuyerId = BuyerId.Value;
            }
        }
        public void SetBuyerCode(string BuyerCode)
        {
            if (this.BuyerCode != BuyerCode)
            {
                this.BuyerCode = BuyerCode;
                ReadModel.BuyerCode = BuyerCode;
            }
        }
        public void SetBuyerName(string BuyerName)
        {
            if (this.BuyerName != BuyerName)
            {
                this.BuyerName = BuyerName;
                ReadModel.BuyerName = BuyerName;
            }
        }

        public void SetSKEPNo(string SKEPNo)
        {
            if (this.SKEPNo != SKEPNo)
            {
                this.SKEPNo = SKEPNo;
                ReadModel.SKEPNo = SKEPNo;
            }
        }

        public void SetSubconCategory(string SubconCategory)
        {
            if (this.SubconCategory != SubconCategory)
            {
                this.SubconCategory = SubconCategory;
                ReadModel.SubconCategory = SubconCategory;
            }
        }

        public void SetUomUnit(string UomUnit)
        {
            if (this.UomUnit != UomUnit)
            {
                this.UomUnit = UomUnit;
                ReadModel.UomUnit = UomUnit;
            }
        }

        public void SetUomId(UomId UomId)
        {
            if (this.UomId != UomId)
            {
                this.UomId = UomId;
                ReadModel.UomId = UomId.Value;
            }
        }

        public void SetAgreementDate(DateTimeOffset AgreementDate)
        {
            if (this.AgreementDate != AgreementDate)
            {
                this.AgreementDate = AgreementDate;
                ReadModel.AgreementDate = AgreementDate;
            }
        }

        public void SetCIF(int CIF)
        {
            if (this.CIF != CIF)
            {
                this.CIF = CIF;
                ReadModel.CIF = CIF;
            }
        }

        public void Modify()
        {
            MarkModified();
        }
    }
}
