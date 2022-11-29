using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconFabricWashes
{
    public class GarmentServiceSubconFabricWashItemDto : BaseDto
    {
        public GarmentServiceSubconFabricWashItemDto(GarmentServiceSubconFabricWashItem garmentServiceSubconFabricWashItem)
        {
            Id = garmentServiceSubconFabricWashItem.Identity;
            ServiceSubconFabricWashId = garmentServiceSubconFabricWashItem.ServiceSubconFabricWashId;
            UnitExpenditureNo = garmentServiceSubconFabricWashItem.UnitExpenditureNo;
            ExpenditureDate = garmentServiceSubconFabricWashItem.ExpenditureDate;
            UnitSender = new UnitSender(garmentServiceSubconFabricWashItem.UnitSenderId.Value, garmentServiceSubconFabricWashItem.UnitSenderCode, garmentServiceSubconFabricWashItem.UnitSenderName);
            UnitRequest = new UnitRequest(garmentServiceSubconFabricWashItem.UnitRequestId.Value, garmentServiceSubconFabricWashItem.UnitRequestCode, garmentServiceSubconFabricWashItem.UnitRequestName);
            Details = new List<GarmentServiceSubconFabricWashDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconFabricWashId { get; set; }
        public string UnitExpenditureNo { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public UnitSender UnitSender { get; set; }
        public UnitRequest UnitRequest { get; set; }
        public virtual List<GarmentServiceSubconFabricWashDetailDto> Details { get; internal set; }
    }
}
