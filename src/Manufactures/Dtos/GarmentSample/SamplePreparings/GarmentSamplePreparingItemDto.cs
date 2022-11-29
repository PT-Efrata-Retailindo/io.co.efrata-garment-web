using Manufactures.Domain.GarmentSample.SamplePreparings;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects;

namespace Manufactures.Dtos.GarmentSample.SamplePreparings
{
    public class GarmentSamplePreparingItemDto
    {
        public GarmentSamplePreparingItemDto(GarmentSamplePreparingItem garmentSamplePreparingItem)
        {
            Id = garmentSamplePreparingItem.Identity;
            LastModifiedDate = garmentSamplePreparingItem.AuditTrail.ModifiedDate ?? garmentSamplePreparingItem.AuditTrail.CreatedDate;
            LastModifiedBy = garmentSamplePreparingItem.AuditTrail.ModifiedBy ?? garmentSamplePreparingItem.AuditTrail.CreatedBy;
            UENItemId = garmentSamplePreparingItem.UENItemId;
            Product = new Product(garmentSamplePreparingItem.ProductId.Value, garmentSamplePreparingItem.ProductName, garmentSamplePreparingItem.ProductCode);
            DesignColor = garmentSamplePreparingItem.DesignColor;
            Quantity = (decimal)garmentSamplePreparingItem.Quantity;
            Uom = new Uom(garmentSamplePreparingItem.UomId.Value, garmentSamplePreparingItem.UomUnit);
            FabricType = garmentSamplePreparingItem.FabricType;
            RemainingQuantity = (decimal)garmentSamplePreparingItem.RemainingQuantity;
            BasicPrice = (decimal)garmentSamplePreparingItem.BasicPrice;
            GarmentSamplePreparingId = garmentSamplePreparingItem.GarmentSamplePreparingId;
            ROSource = garmentSamplePreparingItem.ROSource;
        }

        public Guid Id { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
        public int UENItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public decimal Quantity { get; set; }
        public Uom Uom { get; set; }
        public string FabricType { get; set; }
        public string ROSource { get; set; }
        public decimal RemainingQuantity { get; set; }
        public decimal BasicPrice { get; set; }
        public Guid GarmentSamplePreparingId { get; set; }
    }
}
