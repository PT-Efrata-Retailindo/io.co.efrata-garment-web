using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconInvoicePackingListDto : BaseDto
    {
        public Guid Id { get; internal set; }
        public string InvoiceNo { get; internal set; }
        public string BCType { get; internal set; }
        public DateTimeOffset Date { get; internal set; }
        public Supplier Supplier { get; internal set; }
        public string ContractNo { get; internal set; }
        public double NW { get; internal set; }
        public double GW { get; internal set; }
        public string Remark { get; internal set; }
        public double CIF { get; internal set; }
        public List<string> DLNos { get; set; }
        public List<GarmentSubconInvoicePackingListItemDto> Items { get; set; }
        public GarmentSubconInvoicePackingListDto(SubconInvoicePackingList subconInvoicePackingList)
        {
            Id = subconInvoicePackingList.Identity;
            InvoiceNo = subconInvoicePackingList.InvoiceNo;
            BCType = subconInvoicePackingList.BCType;
            Date = subconInvoicePackingList.Date;
            Supplier = new Supplier(subconInvoicePackingList.SupplierId.Value, subconInvoicePackingList.SupplierCode, subconInvoicePackingList.SupplierName, subconInvoicePackingList.SupplierAddress);
            ContractNo = subconInvoicePackingList.ContractNo;
            NW = subconInvoicePackingList.NW;
            GW = subconInvoicePackingList.GW;
            Remark = subconInvoicePackingList.Remark;
            Items = new List<GarmentSubconInvoicePackingListItemDto>();
        }
    }
}
