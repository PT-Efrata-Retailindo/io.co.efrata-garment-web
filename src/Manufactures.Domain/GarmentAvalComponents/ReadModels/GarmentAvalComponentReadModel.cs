using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalComponents.ReadModels
{
    public class GarmentAvalComponentReadModel : ReadModelBase
    {
        public GarmentAvalComponentReadModel(Guid identity) : base(identity)
        {
        }

        public string AvalComponentNo { get; internal set; }
        public long UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string AvalComponentType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public long ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset Date { get; internal set; }
        public bool IsReceived { get; internal set; }

        public virtual ICollection<GarmentAvalComponentItemReadModel> Items { get; internal set; }
    }
}
