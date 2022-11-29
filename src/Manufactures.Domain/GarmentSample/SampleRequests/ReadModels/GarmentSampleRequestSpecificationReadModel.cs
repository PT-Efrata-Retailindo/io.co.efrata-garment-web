using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.ReadModels
{
    public class GarmentSampleRequestSpecificationReadModel : ReadModelBase
    {
        public GarmentSampleRequestSpecificationReadModel(Guid identity) : base(identity)
        {
        }
        public int Index { get; internal set; }
        public Guid SampleRequestId { get; internal set; }
        public string Inventory { get; internal set; }
        public string SpecificationDetail { get; internal set; }
        public double Quantity { get; internal set; }
        public string Remark { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }

        public virtual GarmentSampleRequestReadModel GarmentSampleRequest { get; internal set; }
    }
}

