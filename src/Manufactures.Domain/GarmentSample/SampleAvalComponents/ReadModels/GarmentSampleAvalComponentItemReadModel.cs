using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels
{
    public class GarmentSampleAvalComponentItemReadModel : ReadModelBase
    {
        public GarmentSampleAvalComponentItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid SampleAvalComponentId { get; internal set; }
        public Guid SampleCuttingInDetailId { get; internal set; }
        public Guid SampleSewingOutItemId { get; internal set; }
        public Guid SampleSewingOutDetailId { get; internal set; }
        public long ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public string Color { get; internal set; }
        public double Quantity { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public long SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public decimal Price { get; internal set; }
        public decimal BasicPrice { get; internal set; }

        public string UId { get; internal set; }
        public virtual GarmentSampleAvalComponentReadModel GarmentSampleAvalComponent { get; internal set; }
    }
}
