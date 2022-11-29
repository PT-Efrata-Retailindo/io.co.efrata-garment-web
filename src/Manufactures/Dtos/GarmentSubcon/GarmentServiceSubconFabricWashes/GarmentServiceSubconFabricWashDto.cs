using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconFabricWashes
{
    public class GarmentServiceSubconFabricWashDto
    {
        public GarmentServiceSubconFabricWashDto(GarmentServiceSubconFabricWash garmentServiceSubconFabricWashList)
        {
            Id = garmentServiceSubconFabricWashList.Identity;
            ServiceSubconFabricWashNo = garmentServiceSubconFabricWashList.ServiceSubconFabricWashNo;
            ServiceSubconFabricWashDate = garmentServiceSubconFabricWashList.ServiceSubconFabricWashDate;
            Remark = garmentServiceSubconFabricWashList.Remark;
            IsUsed = garmentServiceSubconFabricWashList.IsUsed;
            QtyPacking = garmentServiceSubconFabricWashList.QtyPacking;
            UomUnit = garmentServiceSubconFabricWashList.UomUnit;
            Items = new List<GarmentServiceSubconFabricWashItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSubconFabricWashNo { get; set; }
        public DateTimeOffset ServiceSubconFabricWashDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }

        public virtual List<GarmentServiceSubconFabricWashItemDto> Items { get; internal set; }
    }
}
