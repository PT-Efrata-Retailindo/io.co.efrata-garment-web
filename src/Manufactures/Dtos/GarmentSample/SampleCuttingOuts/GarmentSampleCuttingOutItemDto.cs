using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleCuttingOuts
{
    public class GarmentSampleCuttingOutItemDto : BaseDto
    {
        public GarmentSampleCuttingOutItemDto(GarmentSampleCuttingOutItem garmentCuttingOutItem)
        {
            Id = garmentCuttingOutItem.Identity;
            CuttingOutId = garmentCuttingOutItem.CuttingOutId;
            CuttingInId = garmentCuttingOutItem.CuttingInId;
            CuttingInDetailId = garmentCuttingOutItem.CuttingInDetailId;
            Product = new Product(garmentCuttingOutItem.ProductId.Value, garmentCuttingOutItem.ProductCode, garmentCuttingOutItem.ProductName);
            DesignColor = garmentCuttingOutItem.DesignColor;
            TotalCuttingOut = garmentCuttingOutItem.TotalCuttingOut;
            TotalCuttingOutQuantity = garmentCuttingOutItem.TotalCuttingOutQuantity;

            Details = new List<GarmentSampleCuttingOutDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid CuttingOutId { get; set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double TotalCuttingOut { get; set; }
        public double TotalCuttingOutQuantity { get; set; }
        public List<GarmentSampleCuttingOutDetailDto> Details { get; set; }
    }
}
