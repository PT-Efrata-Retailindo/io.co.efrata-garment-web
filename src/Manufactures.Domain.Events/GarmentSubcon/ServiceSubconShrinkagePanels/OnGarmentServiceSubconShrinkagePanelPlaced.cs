using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon
{
    public class OnGarmentServiceSubconShrinkagePanelPlaced : IGarmentServiceSubconShrinkagePanelEvent
    {
        public OnGarmentServiceSubconShrinkagePanelPlaced(Guid garmentServiceSubconShrinkagePanelId)
        {
            GarmentServiceSubconShrinkagePanelId = garmentServiceSubconShrinkagePanelId;
        }

        public Guid GarmentServiceSubconShrinkagePanelId { get; }
    }
}
