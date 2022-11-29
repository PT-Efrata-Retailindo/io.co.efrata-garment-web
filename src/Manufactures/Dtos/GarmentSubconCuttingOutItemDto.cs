using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSubconCuttingOutItemDto : BaseDto
    {
        public GarmentSubconCuttingOutItemDto(GarmentSubconCuttingOutItem garmentCuttingOutItem)
        {
            Id = garmentCuttingOutItem.Identity;
            CutOutId = garmentCuttingOutItem.CutOutId;
            CuttingInId = garmentCuttingOutItem.CuttingInId;
            CuttingInDetailId = garmentCuttingOutItem.CuttingInDetailId;
            Product = new Product(garmentCuttingOutItem.ProductId.Value, garmentCuttingOutItem.ProductCode, garmentCuttingOutItem.ProductName);
            DesignColor = garmentCuttingOutItem.DesignColor;
            TotalCuttingOut = garmentCuttingOutItem.TotalCuttingOut;
            TotalCuttingOutQuantity = garmentCuttingOutItem.TotalCuttingOutQuantity;

            Details = new List<GarmentSubconCuttingOutDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid CutOutId { get; set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double TotalCuttingOut { get; set; }
        public double TotalCuttingOutQuantity { get; set; }

        
        public List<GarmentSubconCuttingOutDetailDto> Details { get; set; }
    }
}
