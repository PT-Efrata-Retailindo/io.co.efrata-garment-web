using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentAvalComponents.Queries.GetGarmentAvalComponents
{
    public class GarmentAvalComponentViewModel
    {
        public Guid Id { get; set; }
        public string AvalComponentNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string AvalComponentType { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool IsReceived { get; set; }

        public List<GarmentAvalComponentItemDto> Items { get; set; }

        public GarmentAvalComponentViewModel(GarmentAvalComponent garmentAvalComponent)
        {
            Id = garmentAvalComponent.Identity;
            AvalComponentNo = garmentAvalComponent.AvalComponentNo;
            Unit = new UnitDepartment(garmentAvalComponent.UnitId.Value, garmentAvalComponent.UnitCode, garmentAvalComponent.UnitName);
            AvalComponentType = garmentAvalComponent.AvalComponentType;
            RONo = garmentAvalComponent.RONo;
            Article = garmentAvalComponent.Article;
            Comodity = garmentAvalComponent.ComodityId.Value > 1 ? new GarmentComodity(garmentAvalComponent.ComodityId.Value, garmentAvalComponent.ComodityCode, garmentAvalComponent.ComodityName) : null;
            Date = garmentAvalComponent.Date;
            IsReceived = garmentAvalComponent.IsReceived;

            Items = new List<GarmentAvalComponentItemDto>();
        }
    }
}