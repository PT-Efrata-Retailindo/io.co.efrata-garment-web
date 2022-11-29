using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetAllGarmentSampleAvalComponents
{
    public class GarmentSampleAvalComponentDto
    {
        public GarmentSampleAvalComponentDto(GarmentSampleAvalComponent garmentSampleAvalComponent)
        {
            Id = garmentSampleAvalComponent.Identity;
            SampleAvalComponentNo = garmentSampleAvalComponent.SampleAvalComponentNo;
            Unit = new UnitDepartment(garmentSampleAvalComponent.UnitId == null ? 0 : garmentSampleAvalComponent.UnitId.Value, garmentSampleAvalComponent.UnitCode, garmentSampleAvalComponent.UnitName);
            SampleAvalComponentType = garmentSampleAvalComponent.SampleAvalComponentType;
            RONo = garmentSampleAvalComponent.RONo;
            Article = garmentSampleAvalComponent.Article;
            Date = garmentSampleAvalComponent.Date;
            IsReceived = garmentSampleAvalComponent.IsReceived;
        }

        public Guid Id { get; set; }

        public string SampleAvalComponentNo { get; private set; }
        public UnitDepartment Unit { get; private set; }
        public string SampleAvalComponentType { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public double Quantities { get; set; }
        public DateTimeOffset Date { get; private set; }
        public bool IsReceived { get; private set; }
    }
}
