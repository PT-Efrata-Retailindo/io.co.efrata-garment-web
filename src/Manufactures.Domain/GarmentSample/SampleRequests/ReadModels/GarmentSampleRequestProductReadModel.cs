using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.ReadModels
{
    public class GarmentSampleRequestProductReadModel : ReadModelBase
    {
        public GarmentSampleRequestProductReadModel(Guid identity) : base(identity)
        {
        }
        public int Index { get; internal set; }
        public Guid SampleRequestId { get; internal set; }
        public string Style { get; internal set; }
        public string Color { get; internal set; }

        public string Fabric { get; internal set; }

        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }

        public string SizeDescription { get; internal set; }
        public double Quantity { get; internal set; }

        public virtual GarmentSampleRequestReadModel GarmentSampleRequest { get; internal set; }
    }
}
