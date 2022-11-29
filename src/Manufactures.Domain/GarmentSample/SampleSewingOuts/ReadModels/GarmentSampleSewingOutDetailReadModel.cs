using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels
{
    public class GarmentSampleSewingOutDetailReadModel : ReadModelBase
    {
        public GarmentSampleSewingOutDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid SampleSewingOutItemId { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }

        public virtual GarmentSampleSewingOutItemReadModel GarmentSampleSewingOutItemIdentity { get; internal set; }

    }
}
