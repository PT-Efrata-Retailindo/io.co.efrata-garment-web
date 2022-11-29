using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels
{
    public class GarmentServiceSubconCuttingItemReadModel : ReadModelBase
    {
        public GarmentServiceSubconCuttingItemReadModel(Guid identity) : base(identity)
        {
        }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }

        public Guid ServiceSubconCuttingId { get; internal set; }
        
        public virtual GarmentServiceSubconCuttingReadModel GarmentServiceSubconCutting { get; internal set; }
        public virtual List<GarmentServiceSubconCuttingDetailReadModel> GarmentServiceSubconCuttingDetail { get; internal set; }
    }
}
