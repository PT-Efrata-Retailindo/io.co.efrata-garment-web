using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconFabricWashes
{
    public class GarmentServiceSubconFabricWashListDto : BaseDto
    {
        public GarmentServiceSubconFabricWashListDto(GarmentServiceSubconFabricWash garmentServiceSubconFabricWashList)
        {
            Id = garmentServiceSubconFabricWashList.Identity;
            ServiceSubconFabricWashNo = garmentServiceSubconFabricWashList.ServiceSubconFabricWashNo;
            ServiceSubconFabricWashDate = garmentServiceSubconFabricWashList.ServiceSubconFabricWashDate;
            CreatedBy = garmentServiceSubconFabricWashList.AuditTrail.CreatedBy;
            IsUsed = garmentServiceSubconFabricWashList.IsUsed;
            Items = new List<GarmentServiceSubconFabricWashItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSubconFabricWashNo { get; set; }
        public DateTimeOffset ServiceSubconFabricWashDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSubconFabricWashItemDto> Items { get; set; }
    }
}
