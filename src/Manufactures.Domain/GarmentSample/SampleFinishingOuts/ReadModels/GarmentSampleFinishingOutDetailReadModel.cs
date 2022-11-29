using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels
{
    public class GarmentSampleFinishingOutDetailReadModel : ReadModelBase
    {
        public GarmentSampleFinishingOutDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid FinishingOutItemId { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }

        public virtual GarmentSampleFinishingOutItemReadModel GarmentSampleFinishingOutItemIdentity { get; internal set; }
    }
}
