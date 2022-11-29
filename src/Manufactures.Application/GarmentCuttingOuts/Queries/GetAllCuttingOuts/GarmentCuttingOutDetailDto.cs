using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class GarmentCuttingOutDetailDto
    {
        public Guid Id { get; set; }
        public Guid CutOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double CuttingOutQuantity { get; set; }
        public Uom CuttingOutUom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
