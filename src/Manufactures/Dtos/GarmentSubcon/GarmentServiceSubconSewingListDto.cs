using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconSewingListDto : BaseDto
    {
        public GarmentServiceSubconSewingListDto(GarmentServiceSubconSewing garmentServiceSubconSewingList)
        {
            Id = garmentServiceSubconSewingList.Identity;
            ServiceSubconSewingNo = garmentServiceSubconSewingList.ServiceSubconSewingNo;
           // Unit = new UnitDepartment(garmentServiceSubconSewingList.UnitId.Value, garmentServiceSubconSewingList.UnitCode, garmentServiceSubconSewingList.UnitName);
            ServiceSubconSewingDate = garmentServiceSubconSewingList.ServiceSubconSewingDate;
            CreatedBy = garmentServiceSubconSewingList.AuditTrail.CreatedBy;
            IsUsed = garmentServiceSubconSewingList.IsUsed;
            Items = new List<GarmentServiceSubconSewingItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSubconSewingNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string SewingTo { get; set; }
        public DateTimeOffset ServiceSubconSewingDate { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSubconSewingItemDto> Items { get; set; }
    }
}
