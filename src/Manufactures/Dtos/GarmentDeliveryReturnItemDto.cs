using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentDeliveryReturnItemDto
    {
        public GarmentDeliveryReturnItemDto(GarmentDeliveryReturnItem garmentDeliveryReturnItem)
        {
            Id = garmentDeliveryReturnItem.Identity;

            LastModifiedDate = garmentDeliveryReturnItem.AuditTrail.ModifiedDate ?? garmentDeliveryReturnItem.AuditTrail.CreatedDate;
            LastModifiedBy = garmentDeliveryReturnItem.AuditTrail.ModifiedBy ?? garmentDeliveryReturnItem.AuditTrail.CreatedBy;
            DRId = garmentDeliveryReturnItem.DRId;
            UnitDOItemId = garmentDeliveryReturnItem.UnitDOItemId;
            UENItemId = garmentDeliveryReturnItem.UENItemId;
            PreparingItemId = garmentDeliveryReturnItem.PreparingItemId;
            Product = new Product(garmentDeliveryReturnItem.ProductId.Value, garmentDeliveryReturnItem.ProductName, garmentDeliveryReturnItem.ProductCode);
            DesignColor = garmentDeliveryReturnItem.DesignColor;
            RONo = garmentDeliveryReturnItem.RONo;
            Quantity = garmentDeliveryReturnItem.Quantity;
            Uom = new Uom(garmentDeliveryReturnItem.UomId.Value, garmentDeliveryReturnItem.UomUnit);
        }

        public Guid Id { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }
        public Guid DRId { get; set; }
        public int UnitDOItemId { get; set; }
        public int UENItemId { get; set; }
        public string PreparingItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public string RONo { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
    }
}