using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleDeliveryReturns
{
    public class GarmentSampleDeliveryReturnItemDto
    {
        public GarmentSampleDeliveryReturnItemDto(GarmentSampleDeliveryReturnItem garmentSampleDeliveryReturnItem)
        {
            Id = garmentSampleDeliveryReturnItem.Identity;

            LastModifiedDate = garmentSampleDeliveryReturnItem.AuditTrail.ModifiedDate ?? garmentSampleDeliveryReturnItem.AuditTrail.CreatedDate;
            LastModifiedBy = garmentSampleDeliveryReturnItem.AuditTrail.ModifiedBy ?? garmentSampleDeliveryReturnItem.AuditTrail.CreatedBy;
            DRId = garmentSampleDeliveryReturnItem.DRId;
            UnitDOItemId = garmentSampleDeliveryReturnItem.UnitDOItemId;
            UENItemId = garmentSampleDeliveryReturnItem.UENItemId;
            PreparingItemId = garmentSampleDeliveryReturnItem.PreparingItemId;
            Product = new Product(garmentSampleDeliveryReturnItem.ProductId.Value, garmentSampleDeliveryReturnItem.ProductName, garmentSampleDeliveryReturnItem.ProductCode);
            DesignColor = garmentSampleDeliveryReturnItem.DesignColor;
            RONo = garmentSampleDeliveryReturnItem.RONo;
            Quantity = garmentSampleDeliveryReturnItem.Quantity;
            Uom = new Uom(garmentSampleDeliveryReturnItem.UomId.Value, garmentSampleDeliveryReturnItem.UomUnit);
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
