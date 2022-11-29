using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries
{
    public class GarmentSampleCuttingOutListDto
    {
        public Guid Id { get; set; }
        public string CutOutNo { get; set; }
        public string CuttingOutType { get; set; }

        public UnitDepartment UnitFrom { get; set; }
        public DateTimeOffset CuttingOutDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public GarmentComodity Comodity { get; set; }

        public double TotalRemainingQuantity { get; set; }
        public double TotalCuttingOutQuantity { get; set; }
        public List<string> Products { get; set; }
        public List<GarmentSampleCuttingOutItemDto> Items { get; set; }
    }
}
