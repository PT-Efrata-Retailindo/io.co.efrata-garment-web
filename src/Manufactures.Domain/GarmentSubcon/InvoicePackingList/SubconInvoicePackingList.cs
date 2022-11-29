using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList
{
    public class SubconInvoicePackingList : AggregateRoot<SubconInvoicePackingList, SubconInvoicePackingListReadModel>
    {
        public string InvoiceNo { get; private set; }
        public string BCType { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public SupplierId SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public string SupplierAddress { get; private set; }
        public string ContractNo { get; private set; }
        public double NW { get; private set; }
        public double GW { get; private set; }
        public string Remark { get; private set; }

        public SubconInvoicePackingList(SubconInvoicePackingListReadModel readModel):base(readModel)
        {
            InvoiceNo = readModel.InvoiceNo;
            BCType = readModel.BCType;
            Date = readModel.Date;
            SupplierId = new SupplierId(readModel.SupplierId);
            SupplierCode = readModel.SupplierCode;
            SupplierName = readModel.SupplierName;
            SupplierAddress = readModel.SupplierAddress;
            ContractNo = readModel.ContractNo;
            NW = readModel.NW;
            GW = readModel.GW;
            Remark = readModel.Remark;
        }

        public SubconInvoicePackingList(Guid identity, string invoiceNo,string bcType, DateTimeOffset date, SupplierId supplierId, string supplierCode, string supplierName, string supplierAddress, string contractNo, double nw,double gw, string keterangan) : base(identity)
        {
            Identity = Identity;
            InvoiceNo = invoiceNo;
            BCType = bcType;
            Date = date;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            SupplierAddress = supplierAddress;
            ContractNo = contractNo;
            NW = nw;
            GW = gw;
            Remark= keterangan;

            ReadModel = new SubconInvoicePackingListReadModel(Identity)
            {
                InvoiceNo = InvoiceNo,
                BCType = BCType,
                Date = Date,
                SupplierId = SupplierId.Value,
                SupplierCode = SupplierCode,
                SupplierName = SupplierCode,
                SupplierAddress = SupplierAddress,
                ContractNo = ContractNo,
                NW = NW,
                GW = GW,
                Remark = Remark,
            };

            ReadModel.AddDomainEvent(new OnSubconPackingListPlaced(Identity));
        }

        protected override SubconInvoicePackingList GetEntity()
        {
            return this;
        }
        public void SetInvoiceNo(string InvoiceNp)
        {
            if (this.InvoiceNo != InvoiceNo)
            {
                this.InvoiceNo = InvoiceNo;
                ReadModel.InvoiceNo = InvoiceNo;
            }
        }
        public void SetBCType(string BCType)
        {
            if (this.BCType != BCType)
            {
                this.BCType = BCType;
                ReadModel.BCType = BCType;
            }
        }
        public void SetDate(DateTimeOffset Date)
        {
            if (this.Date != Date)
            {
                this.Date = Date;
                ReadModel.Date = Date;
            }
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
        public void SetSupplierAddress(string SupplierAddress)
        {
            if (this.SupplierAddress != SupplierAddress)
            {
                this.SupplierAddress = SupplierAddress;
                ReadModel.SupplierAddress = SupplierAddress;
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
        public void SetNW(double NW)
        {
            if (this.NW != NW)
            {
                this.NW = NW;
                ReadModel.NW = NW;
            }
        }

        public void SetGW(double GW)
        {
            if (this.GW != GW)
            {
                this.GW = GW;
                ReadModel.GW = GW;
            }
        }
        public void SetRemark(string Remark)
        {
            if (this.Remark != Remark)
            {
                this.Remark = Remark;
                ReadModel.Remark = Remark;
            }
        }

        public void Modify()
        {
            MarkModified();
        }



    }


}
