using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetGarmentSampleAvalComponents
{
    public class GarmentSampleAvalComponentViewModel
    {
        public Guid Id { get; set; }
        public string SampleAvalComponentNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string SampleAvalComponentType { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool IsReceived { get; set; }

        public List<GarmentSampleAvalComponentItemDto> Items { get; set; }

        public GarmentSampleAvalComponentViewModel(GarmentSampleAvalComponent garmentSampleAvalComponent)
        {
            Id = garmentSampleAvalComponent.Identity;
            SampleAvalComponentNo = garmentSampleAvalComponent.SampleAvalComponentNo;
            Unit = new UnitDepartment(garmentSampleAvalComponent.UnitId.Value, garmentSampleAvalComponent.UnitCode, garmentSampleAvalComponent.UnitName);
            SampleAvalComponentType = garmentSampleAvalComponent.SampleAvalComponentType;
            RONo = garmentSampleAvalComponent.RONo;
            Article = garmentSampleAvalComponent.Article;
            Comodity = garmentSampleAvalComponent.ComodityId.Value > 1 ? new GarmentComodity(garmentSampleAvalComponent.ComodityId.Value, garmentSampleAvalComponent.ComodityCode, garmentSampleAvalComponent.ComodityName) : null;
            Date = garmentSampleAvalComponent.Date;
            IsReceived = garmentSampleAvalComponent.IsReceived;

            Items = new List<GarmentSampleAvalComponentItemDto>();
        }
    }
}
