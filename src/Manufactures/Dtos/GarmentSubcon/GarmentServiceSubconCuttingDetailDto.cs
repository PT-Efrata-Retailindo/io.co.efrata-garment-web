using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingDetailDto : BaseDto
    {
        public GarmentServiceSubconCuttingDetailDto(GarmentServiceSubconCuttingDetail garmentServiceSubconCuttingDetail)
        {
            Id = garmentServiceSubconCuttingDetail.Identity;
            ServiceSubconCuttingItemId = garmentServiceSubconCuttingDetail.ServiceSubconCuttingItemId;
            DesignColor = garmentServiceSubconCuttingDetail.DesignColor;
            Quantity = garmentServiceSubconCuttingDetail.Quantity;
            Sizes= new List<GarmentServiceSubconCuttingSizeDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconCuttingItemId { get; set; }

        public string DesignColor { get; set; }

        public double Quantity { get; set; }
        public List<GarmentServiceSubconCuttingSizeDto> Sizes { get; set; }
    }
}