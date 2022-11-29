using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleAvalProducts
{
    public class GarmentSampleAvalProductItemDto
    {
        public GarmentSampleAvalProductItemDto(GarmentSampleAvalProductItem garmentSampleAvalProductItem)
        {
            Id = garmentSampleAvalProductItem.Identity;

            LastModifiedDate = garmentSampleAvalProductItem.AuditTrail.ModifiedDate ?? garmentSampleAvalProductItem.AuditTrail.CreatedDate;
            LastModifiedBy = garmentSampleAvalProductItem.AuditTrail.ModifiedBy ?? garmentSampleAvalProductItem.AuditTrail.CreatedBy;

            APId = garmentSampleAvalProductItem.APId;
            SamplePreparingId = new GarmentSamplePreparing(garmentSampleAvalProductItem.SamplePreparingId.Value, "", "");
            SamplePreparingItemId = new GarmentSamplePreparingItem(garmentSampleAvalProductItem.SamplePreparingItemId.Value, null, "", 0);
            Product = new Product(garmentSampleAvalProductItem.ProductId.Value, garmentSampleAvalProductItem.ProductName, garmentSampleAvalProductItem.ProductCode);
            DesignColor = garmentSampleAvalProductItem.DesignColor;
            Quantity = garmentSampleAvalProductItem.Quantity;
            Uom = new Uom(garmentSampleAvalProductItem.UomId.Value, garmentSampleAvalProductItem.UomUnit);
            BasicPrice = garmentSampleAvalProductItem.BasicPrice;
            IsReceived = garmentSampleAvalProductItem.IsReceived;
        }

        public Guid Id { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }

        public Guid APId { get; set; }
        public GarmentSamplePreparing SamplePreparingId { get; set; }
        public GarmentSamplePreparingItem SamplePreparingItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public double BasicPrice { get; set; }
        public bool IsReceived { get; set; }
    }
}
