using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentCuttingInDetailDto : BaseDto
    {
        public GarmentCuttingInDetailDto(GarmentCuttingInDetail garmentCuttingInDetail)
        {
            Id = garmentCuttingInDetail.Identity;
            CutInItemId = garmentCuttingInDetail.CutInItemId;
            PreparingItemId = garmentCuttingInDetail.PreparingItemId;
            Product = new Product(garmentCuttingInDetail.ProductId.Value, garmentCuttingInDetail.ProductCode, garmentCuttingInDetail.ProductName);
            DesignColor = garmentCuttingInDetail.DesignColor;
            FabricType = garmentCuttingInDetail.FabricType;
            PreparingQuantity = garmentCuttingInDetail.PreparingQuantity;
            PreparingUom = new Uom(garmentCuttingInDetail.PreparingUomId.Value, garmentCuttingInDetail.PreparingUomUnit);
            CuttingInQuantity = garmentCuttingInDetail.CuttingInQuantity;
            CuttingInUom = new Uom(garmentCuttingInDetail.CuttingInUomId.Value, garmentCuttingInDetail.CuttingInUomUnit);
            RemainingQuantity = garmentCuttingInDetail.RemainingQuantity;
            BasicPrice = garmentCuttingInDetail.BasicPrice;
            Price = garmentCuttingInDetail.Price;
            FC = garmentCuttingInDetail.FC;
            Color = garmentCuttingInDetail.Color;
        }

        public Guid Id { get; set; }
        public Guid CutInItemId { get; set; }
        public Guid PreparingItemId { get; set; }
        public Product Product { get; set; }

        public string DesignColor { get; set; }
        public string FabricType { get; set; }

        public double PreparingRemainingQuantity { get; set; }
        public double PreparingQuantity { get; set; }
        public Uom PreparingUom { get; set; }

        public double CuttingInQuantity { get; set; }
        public Uom CuttingInUom { get; set; }

        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
        public double FC { get; set; }
        public string Color { get; set; }
    }
}
