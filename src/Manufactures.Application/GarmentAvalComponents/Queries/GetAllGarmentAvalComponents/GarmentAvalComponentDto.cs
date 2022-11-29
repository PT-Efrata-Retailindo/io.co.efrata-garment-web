using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents
{
    public class GarmentAvalComponentDto
    {
        public GarmentAvalComponentDto(GarmentAvalComponent garmentAvalComponent)
        {
            Id = garmentAvalComponent.Identity;
            AvalComponentNo = garmentAvalComponent.AvalComponentNo;
            Unit = new UnitDepartment(garmentAvalComponent.UnitId == null ? 0 : garmentAvalComponent.UnitId.Value, garmentAvalComponent.UnitCode, garmentAvalComponent.UnitName);
            AvalComponentType = garmentAvalComponent.AvalComponentType;
            RONo = garmentAvalComponent.RONo;
            Article = garmentAvalComponent.Article;
            Date = garmentAvalComponent.Date;
            IsReceived = garmentAvalComponent.IsReceived;
        }

        public Guid Id { get; set; }

        public string AvalComponentNo { get; private set; }
        public UnitDepartment Unit { get; private set; }
        public string AvalComponentType { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public double Quantities { get; set; }
        public DateTimeOffset Date { get; private set; }
        public bool IsReceived { get; private set; }
    }
}
