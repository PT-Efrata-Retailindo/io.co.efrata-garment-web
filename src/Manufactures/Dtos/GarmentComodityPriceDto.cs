using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentComodityPriceDto : BaseDto
    {
        public GarmentComodityPriceDto(GarmentComodityPrice garmentComodityPrice)
        {
            Id = garmentComodityPrice.Identity;
            Comodity = new GarmentComodity(garmentComodityPrice.ComodityId.Value, garmentComodityPrice.ComodityCode, garmentComodityPrice.ComodityName);
            IsValid = garmentComodityPrice.IsValid;
            Unit = new UnitDepartment(garmentComodityPrice.UnitId.Value, garmentComodityPrice.UnitCode, garmentComodityPrice.UnitName);
            Date = garmentComodityPrice.Date;
            Price = garmentComodityPrice.Price;
        }
        public Guid Id { get; set; }
        public bool IsValid { get; set; }
        public DateTimeOffset Date { get; set; }
        public UnitDepartment Unit { get; set; }
        public GarmentComodity Comodity { get; set; }
        public decimal Price { get; set; }
    }
}
