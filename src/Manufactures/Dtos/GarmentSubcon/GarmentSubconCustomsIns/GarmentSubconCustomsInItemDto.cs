using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentSubconCustomsIns
{
    public class GarmentSubconCustomsInItemDto : BaseDto
    {
        public GarmentSubconCustomsInItemDto(GarmentSubconCustomsInItem garmentSubconCustomsInItem)
        {
            Id = garmentSubconCustomsInItem.Identity;
            SubconCustomsInId = garmentSubconCustomsInItem.SubconCustomsInId;
            Supplier = new Supplier(garmentSubconCustomsInItem.SupplierId.Value, garmentSubconCustomsInItem.SupplierCode, garmentSubconCustomsInItem.SupplierName);
            DoId = garmentSubconCustomsInItem.DoId;
            DoNo = garmentSubconCustomsInItem.DoNo;
            Quantity = garmentSubconCustomsInItem.Quantity;
        }

        public Guid Id { get; set; }
        public Guid SubconCustomsInId { get; set; }
        public Supplier Supplier { get; set; }
        public int DoId { get; set; }
        public string DoNo { get; set; }
        public decimal Quantity { get; set; }
    }
}
