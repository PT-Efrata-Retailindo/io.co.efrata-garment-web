using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleSewingOuts
{
    public class GarmentSampleSewingOutDto : BaseDto
    {
        public GarmentSampleSewingOutDto(GarmentSampleSewingOut garmentSewingOutList)
        {
            Id = garmentSewingOutList.Identity;
            SewingOutNo = garmentSewingOutList.SewingOutNo;
            UnitTo = new UnitDepartment(garmentSewingOutList.UnitToId.Value, garmentSewingOutList.UnitToCode, garmentSewingOutList.UnitToName);
            Unit = new UnitDepartment(garmentSewingOutList.UnitId.Value, garmentSewingOutList.UnitCode, garmentSewingOutList.UnitName);
            RONo = garmentSewingOutList.RONo;
            Article = garmentSewingOutList.Article;
            SewingOutDate = garmentSewingOutList.SewingOutDate;
            SewingTo = garmentSewingOutList.SewingTo;
            Buyer = new Buyer(garmentSewingOutList.BuyerId.Value, garmentSewingOutList.BuyerCode, garmentSewingOutList.BuyerName);
            Comodity = new GarmentComodity(garmentSewingOutList.ComodityId.Value, garmentSewingOutList.ComodityCode, garmentSewingOutList.ComodityName);
            IsDifferentSize = garmentSewingOutList.IsDifferentSize;

            Items = new List<GarmentSampleSewingOutItemDto>();
        }

        public Guid Id { get; internal set; }
        public string SewingOutNo { get; set; }
        public Buyer Buyer { get; set; }
        public UnitDepartment Unit { get; set; }
        public string SewingTo { get; set; }
        public UnitDepartment UnitTo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset SewingOutDate { get; set; }
        public bool IsDifferentSize { get; set; }

        public virtual List<GarmentSampleSewingOutItemDto> Items { get; internal set; }
    }
}
