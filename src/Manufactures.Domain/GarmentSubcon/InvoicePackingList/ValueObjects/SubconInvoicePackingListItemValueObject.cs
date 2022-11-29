using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;

namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.ValueObjects
{
    public class SubconInvoicePackingListItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid InvoicePackingListId { get; set; }
        public string DLNo { get;  set; }
        public DateTimeOffset DLDate { get;  set; }

        public Product Product { get;  set; }

        public string DesignColor { get;  set; }
        public double Quantity { get;  set; }
        public Uom Uom { get;  set; }
        public double CIF { get;  set; }
        public double TotalPrice { get;  set; }

        public SubconInvoicePackingListItemValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
