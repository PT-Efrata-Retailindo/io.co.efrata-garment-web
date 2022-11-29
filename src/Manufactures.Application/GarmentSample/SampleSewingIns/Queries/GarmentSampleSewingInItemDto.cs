using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleSewingIns.Queries
{
    public class GarmentSampleSewingInItemDto
    {
        public Guid Id { get; set; }
        public Guid SewingInId { get; set; }
        public Guid CuttingOutDetailId { get; set; }
        public Guid CuttingOutItemId { get; set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
