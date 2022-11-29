using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingSizeDto : BaseDto
    {
        public GarmentServiceSubconCuttingSizeDto(GarmentServiceSubconCuttingSize garmentServiceSubconCuttingSize)
        {
            Id = garmentServiceSubconCuttingSize.Identity;
            ServiceSubconCuttingDetailId = garmentServiceSubconCuttingSize.ServiceSubconCuttingDetailId;
            CuttingInId = garmentServiceSubconCuttingSize.CuttingInId;
            CuttingInDetailId = garmentServiceSubconCuttingSize.CuttingInDetailId;
            Product = new Product(garmentServiceSubconCuttingSize.ProductId.Value, garmentServiceSubconCuttingSize.ProductCode, garmentServiceSubconCuttingSize.ProductName);
            Color = garmentServiceSubconCuttingSize.Color;
            Quantity = garmentServiceSubconCuttingSize.Quantity;
            Size = new SizeValueObject(garmentServiceSubconCuttingSize.SizeId.Value, garmentServiceSubconCuttingSize.SizeName);
            Uom = new Uom(garmentServiceSubconCuttingSize.UomId.Value, garmentServiceSubconCuttingSize.UomUnit);
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconCuttingDetailId { get; set; }
        public SizeValueObject Size { get; internal set; }
        public double Quantity { get; internal set; }
        public Uom Uom { get; internal set; }
        public string Color { get; internal set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
    }
}