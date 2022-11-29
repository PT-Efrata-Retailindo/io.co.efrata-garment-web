using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSample.SampleCuttingIns
{
    public class GarmentSampleCuttingInDetailDto : BaseDto
    {
        public GarmentSampleCuttingInDetailDto(GarmentSampleCuttingInDetail garmentSampleCuttingInDetail)
        {
            Id = garmentSampleCuttingInDetail.Identity;
            CutInItemId = garmentSampleCuttingInDetail.CutInItemId;
            PreparingItemId = garmentSampleCuttingInDetail.PreparingItemId;
            Product = new Product(garmentSampleCuttingInDetail.ProductId.Value, garmentSampleCuttingInDetail.ProductCode, garmentSampleCuttingInDetail.ProductName);
            DesignColor = garmentSampleCuttingInDetail.DesignColor;
            FabricType = garmentSampleCuttingInDetail.FabricType;
            PreparingQuantity = garmentSampleCuttingInDetail.PreparingQuantity;
            PreparingUom = new Uom(garmentSampleCuttingInDetail.PreparingUomId.Value, garmentSampleCuttingInDetail.PreparingUomUnit);
            CuttingInQuantity = garmentSampleCuttingInDetail.CuttingInQuantity;
            CuttingInUom = new Uom(garmentSampleCuttingInDetail.CuttingInUomId.Value, garmentSampleCuttingInDetail.CuttingInUomUnit);
            RemainingQuantity = garmentSampleCuttingInDetail.RemainingQuantity;
            BasicPrice = garmentSampleCuttingInDetail.BasicPrice;
            Price = garmentSampleCuttingInDetail.Price;
            FC = garmentSampleCuttingInDetail.FC;
            Color = garmentSampleCuttingInDetail.Color;
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
