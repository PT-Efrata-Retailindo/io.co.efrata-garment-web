using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels
{
    public class GarmentSampleAvalComponentReadModel : ReadModelBase
    {
        public GarmentSampleAvalComponentReadModel(Guid identity) : base(identity)
        {
        }

        public string SampleAvalComponentNo { get; internal set; }
        public long UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string SampleAvalComponentType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public long ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset Date { get; internal set; }
        public bool IsReceived { get; internal set; }

        public virtual ICollection<GarmentSampleAvalComponentItemReadModel> Items { get; internal set; }
    }
}
