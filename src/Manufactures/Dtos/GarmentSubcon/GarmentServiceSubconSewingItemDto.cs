using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconSewingItemDto : BaseDto
    {
        public GarmentServiceSubconSewingItemDto(GarmentServiceSubconSewingItem garmentServiceSubconSewingItem)
        {
            Id = garmentServiceSubconSewingItem.Identity;
            ServiceSubconSewingId = garmentServiceSubconSewingItem.ServiceSubconSewingId;
            RONo = garmentServiceSubconSewingItem.RONo;
            Article = garmentServiceSubconSewingItem.Article;
            Comodity = new GarmentComodity(garmentServiceSubconSewingItem.ComodityId.Value, garmentServiceSubconSewingItem.ComodityCode, garmentServiceSubconSewingItem.ComodityName);
            Buyer = new Buyer(garmentServiceSubconSewingItem.BuyerId.Value, garmentServiceSubconSewingItem.BuyerCode, garmentServiceSubconSewingItem.BuyerName);
            Unit = new UnitDepartment(garmentServiceSubconSewingItem.UnitId.Value, garmentServiceSubconSewingItem.UnitCode, garmentServiceSubconSewingItem.UnitName);

            Details = new List<GarmentServiceSubconSewingDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid ServiceSubconSewingId { get; set; }
        public Buyer Buyer { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public UnitDepartment Unit { get; set; }
        public virtual List<GarmentServiceSubconSewingDetailDto> Details { get; internal set; }
    }
}
