using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.Events.GarmentSubcon.InvoicePackingList;

namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList
{
    public class SubconInvoicePackingListItem : AggregateRoot<SubconInvoicePackingListItem, SubconInvoicePackingListItemReadModel>
    {
        public Guid InvoicePackingListId { get; private set; }
        public string DLNo { get; private set; }
        public DateTimeOffset DLDate { get; private set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string ProductRemark { get; private set; }

        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public double CIF { get; private set; }
        public double TotalPrice { get; private set; }

        public SubconInvoicePackingListItem(Guid identity, Guid invoicePackingListId, string dlNo, DateTimeOffset dlDate, ProductId productId, string productCode, string productName, string productRemark, string designColor, double quantity, UomId uomId, string uomUnit, double cif, double totalPrice) : base(identity)
        {
            Identity = identity;
            InvoicePackingListId = invoicePackingListId;
            DLNo = dlNo;
            DLDate = dlDate;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ProductRemark = productRemark;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            CIF = cif;
            TotalPrice = totalPrice;

            ReadModel = new SubconInvoicePackingListItemReadModel(Identity)
            {
                InvoicePackingListId = InvoicePackingListId,
                DLNo = DLNo,
                DLDate = DLDate,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                ProductRemark = ProductRemark,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                CIF = CIF,
                TotalPrice = TotalPrice,
            };

            ReadModel.AddDomainEvent(new OnSubconPackingListPlaced(Identity));

        }

        public SubconInvoicePackingListItem(SubconInvoicePackingListItemReadModel readModel) : base(readModel)
        {
            InvoicePackingListId = readModel.InvoicePackingListId;
            DLNo = readModel.DLNo;
            DLDate = readModel.DLDate;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            ProductRemark = readModel.ProductRemark;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
            UomUnit = readModel.UomUnit;
            UomId = new UomId(readModel.UomId);
            CIF = readModel.CIF;
            TotalPrice = readModel.TotalPrice;
        }

        public void SetDLNo(string DLNo)
        {
            if (this.DLNo != DLNo)
            {
                this.DLNo = DLNo;
                ReadModel.DLNo = DLNo;
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
        public void SetCIFItem(int CIF)
        {
            if (this.CIF != CIF)
            {
                this.CIF = CIF;
                ReadModel.CIF = CIF;
            }
        }

        public void SetTotalPrice(int TotalPrice)
        {
            if (this.TotalPrice != TotalPrice)
            {
                this.TotalPrice = TotalPrice;
                ReadModel.TotalPrice = TotalPrice;
            }
        }
        public void Modify()
        {
            MarkModified();
        }
        protected override SubconInvoicePackingListItem GetEntity()
        {
            return this;
        }

    }
}
