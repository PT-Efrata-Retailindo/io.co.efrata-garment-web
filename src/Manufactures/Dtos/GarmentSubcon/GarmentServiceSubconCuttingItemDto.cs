using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingItemDto : BaseDto
    {
        public GarmentServiceSubconCuttingItemDto(GarmentServiceSubconCuttingItem garmentServiceSubconCuttingItem)
        {
            Id = garmentServiceSubconCuttingItem.Identity;
            ServiceSubconCuttingId = garmentServiceSubconCuttingItem.ServiceSubconCuttingId;
            RONo = garmentServiceSubconCuttingItem.RONo;
            Article = garmentServiceSubconCuttingItem.Article;
            Comodity = new GarmentComodity(garmentServiceSubconCuttingItem.ComodityId.Value, garmentServiceSubconCuttingItem.ComodityCode, garmentServiceSubconCuttingItem.ComodityName);
            Details = new List<GarmentServiceSubconCuttingDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconCuttingId { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public List<GarmentServiceSubconCuttingDetailDto> Details { get; set; }
    }
}