using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentAvalProductItemDto
    {
        public GarmentAvalProductItemDto(GarmentAvalProductItem garmentAvalProductItem)
        {
            Id = garmentAvalProductItem.Identity;

            LastModifiedDate = garmentAvalProductItem.AuditTrail.ModifiedDate ?? garmentAvalProductItem.AuditTrail.CreatedDate;
            LastModifiedBy = garmentAvalProductItem.AuditTrail.ModifiedBy ?? garmentAvalProductItem.AuditTrail.CreatedBy;

            APId = garmentAvalProductItem.APId;
            PreparingId = new GarmentPreparing(garmentAvalProductItem.PreparingId.Value, "", "");
            PreparingItemId = new GarmentPreparingItem(garmentAvalProductItem.PreparingItemId.Value, null, "", 0);
            Product = new Product(garmentAvalProductItem.ProductId.Value, garmentAvalProductItem.ProductName, garmentAvalProductItem.ProductCode);
            DesignColor = garmentAvalProductItem.DesignColor;
            Quantity = garmentAvalProductItem.Quantity;
            Uom = new Uom(garmentAvalProductItem.UomId.Value, garmentAvalProductItem.UomUnit);
            BasicPrice = garmentAvalProductItem.BasicPrice;
            IsReceived = garmentAvalProductItem.IsReceived;
        }

        public Guid Id { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }

        public Guid APId { get; set; }
        public GarmentPreparing PreparingId { get; set; }
        public GarmentPreparingItem PreparingItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public double BasicPrice { get; set; }
        public bool IsReceived { get; set; }
    }
}