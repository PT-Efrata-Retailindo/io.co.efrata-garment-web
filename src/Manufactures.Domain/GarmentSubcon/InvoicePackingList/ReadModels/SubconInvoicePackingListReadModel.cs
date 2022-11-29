using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels
{
    public class SubconInvoicePackingListReadModel : ReadModelBase
    {
        public SubconInvoicePackingListReadModel(Guid identity) : base(identity)
        {
        }

        public string InvoiceNo { get; internal set; }
        public string BCType { get; internal set; }
        public DateTimeOffset Date { get; internal set; }
        public int SupplierId { get; internal set; }
        public string SupplierCode { get; internal set; }
        public string SupplierName { get; internal set; }
        public string SupplierAddress { get; internal set; }
        public string ContractNo { get; internal set; }
        public double NW { get; internal set; }
        public double GW { get; internal set; }
        public string Remark { get; internal set; }
        public virtual List<SubconInvoicePackingListItemReadModel> SubconInvoicePackingListItem { get; internal set; }

    }
}
