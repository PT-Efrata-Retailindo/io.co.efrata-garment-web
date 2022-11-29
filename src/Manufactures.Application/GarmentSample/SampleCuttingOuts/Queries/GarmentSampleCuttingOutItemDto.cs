using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries
{
    public class GarmentSampleCuttingOutItemDto
    {
        public Guid Id { get; set; }
        public Guid CutOutId { get; set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double TotalCuttingOut { get; set; }
        public double TotalCuttingOutQuantity { get; set; }
        public List<GarmentSampleCuttingOutDetailDto> Details { get; set; }
    }
}
