using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconSewingDto
    {
        public GarmentServiceSubconSewingDto(GarmentServiceSubconSewing garmentServiceSubconSewingList)
        {
            Id = garmentServiceSubconSewingList.Identity;
            ServiceSubconSewingNo = garmentServiceSubconSewingList.ServiceSubconSewingNo;
            //
            ServiceSubconSewingDate = garmentServiceSubconSewingList.ServiceSubconSewingDate;
            IsUsed = garmentServiceSubconSewingList.IsUsed;
            Buyer = new Buyer(garmentServiceSubconSewingList.BuyerId.Value, garmentServiceSubconSewingList.BuyerCode, garmentServiceSubconSewingList.BuyerName);
            QtyPacking = garmentServiceSubconSewingList.QtyPacking;
            UomUnit = garmentServiceSubconSewingList.UomUnit;
            Items = new List<GarmentServiceSubconSewingItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSubconSewingNo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset ServiceSubconSewingDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }

        public virtual List<GarmentServiceSubconSewingItemDto> Items { get; internal set; }
    }
}
