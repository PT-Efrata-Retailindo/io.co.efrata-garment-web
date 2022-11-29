using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class GarmentCuttingOutItemDto
    {
        public Guid Id { get; set; }
        public Guid CutOutId { get; set; }
        public Guid CuttingInId { get; set; }
        public Guid CuttingInDetailId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public double TotalCuttingOut { get; set; }
        public double TotalCuttingOutQuantity { get; set; }
        public List<GarmentCuttingOutDetailDto> Details { get; set; }
    }
}
