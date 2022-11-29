using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentCuttingOutItemDto : BaseDto
    {
        public GarmentCuttingOutItemDto(GarmentCuttingOutItem garmentCuttingOutItem)
        {
            Id = garmentCuttingOutItem.Identity;
            CutOutId = garmentCuttingOutItem.CutOutId;
            CuttingInId = garmentCuttingOutItem.CuttingInId;
            CuttingInDetailId = garmentCuttingOutItem.CuttingInDetailId;
            Product = new Product(garmentCuttingOutItem.ProductId.Value, garmentCuttingOutItem.ProductCode, garmentCuttingOutItem.ProductName);
            DesignColor = garmentCuttingOutItem.DesignColor;
            TotalCuttingOut = garmentCuttingOutItem.TotalCuttingOut;
            TotalCuttingOutQuantity = garmentCuttingOutItem.TotalCuttingOutQuantity;

            Details = new List<GarmentCuttingOutDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid CutOutId { get; set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double TotalCuttingOut { get; set; }
        public double TotalCuttingOutQuantity { get; set; }
        public List<GarmentCuttingOutDetailDto> Details { get; set; }
    }
}