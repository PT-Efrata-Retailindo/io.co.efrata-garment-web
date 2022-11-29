using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels
{
    public class SubconInvoicePackingListItemReadModel : ReadModelBase
    {
        public SubconInvoicePackingListItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid InvoicePackingListId { get; internal set; }
        public string DLNo { get; internal set; }
        public DateTimeOffset DLDate { get; internal set; }

        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string ProductRemark { get; internal set; }

        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public double CIF { get; internal set; }
        public double TotalPrice { get; internal set; }

        public virtual SubconInvoicePackingListReadModel SubconInvoicePacking { get; internal set; }
    }
}
