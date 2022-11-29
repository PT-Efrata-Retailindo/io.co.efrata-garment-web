using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconShrinkagePanels
{
    public class GarmentServiceSubconShrinkagePanelItemDto : BaseDto
    {
        public GarmentServiceSubconShrinkagePanelItemDto(GarmentServiceSubconShrinkagePanelItem garmentServiceSubconShrinkagePanelItem)
        {
            Id = garmentServiceSubconShrinkagePanelItem.Identity;
            ServiceSubconShrinkagePanelId = garmentServiceSubconShrinkagePanelItem.ServiceSubconShrinkagePanelId;
            UnitExpenditureNo = garmentServiceSubconShrinkagePanelItem.UnitExpenditureNo;
            ExpenditureDate = garmentServiceSubconShrinkagePanelItem.ExpenditureDate;
            UnitSender = new UnitSender(garmentServiceSubconShrinkagePanelItem.UnitSenderId.Value, garmentServiceSubconShrinkagePanelItem.UnitSenderCode, garmentServiceSubconShrinkagePanelItem.UnitSenderName);
            UnitRequest = new UnitRequest(garmentServiceSubconShrinkagePanelItem.UnitRequestId.Value, garmentServiceSubconShrinkagePanelItem.UnitRequestCode, garmentServiceSubconShrinkagePanelItem.UnitRequestName);
            Details = new List<GarmentServiceSubconShrinkagePanelDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconShrinkagePanelId { get; set; }
        public string UnitExpenditureNo { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public UnitSender UnitSender { get; set; }
        public UnitRequest UnitRequest { get; set; }
        public virtual List<GarmentServiceSubconShrinkagePanelDetailDto> Details { get; internal set; }
    }
}
