using System;
namespace Manufactures.Domain.Events.GarmentSubcon
{
    public class OnGarmentServiceSubconSewingPlaced : IGarmentServiceSubconSewingEvent
    {
        public OnGarmentServiceSubconSewingPlaced(Guid garmentServiceSubconSewingId)
        {
            GarmentServiceSubconSewingId = garmentServiceSubconSewingId;
        }

        public Guid GarmentServiceSubconSewingId { get; }
    }
}
