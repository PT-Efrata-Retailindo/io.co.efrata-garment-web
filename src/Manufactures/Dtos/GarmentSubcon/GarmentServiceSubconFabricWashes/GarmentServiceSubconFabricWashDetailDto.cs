using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconFabricWashes
{
    public class GarmentServiceSubconFabricWashDetailDto
    {
        public GarmentServiceSubconFabricWashDetailDto(GarmentServiceSubconFabricWashDetail garmentServiceSubconFabricWashDetail)
        {
            Id = garmentServiceSubconFabricWashDetail.Identity;
            ServiceSubconFabricWashItemId = garmentServiceSubconFabricWashDetail.ServiceSubconFabricWashItemId;
            Product = new Product(garmentServiceSubconFabricWashDetail.ProductId.Value, garmentServiceSubconFabricWashDetail.ProductCode, garmentServiceSubconFabricWashDetail.ProductName, garmentServiceSubconFabricWashDetail.ProductRemark);
            DesignColor = garmentServiceSubconFabricWashDetail.DesignColor;
            Quantity = garmentServiceSubconFabricWashDetail.Quantity;
            Uom = new Uom(garmentServiceSubconFabricWashDetail.UomId.Value, garmentServiceSubconFabricWashDetail.UomUnit);
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconFabricWashItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public decimal Quantity { get; set; }
        public Uom Uom { get; set; }
    }
}
