using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels
{
    public class GarmentServiceSubconFabricWashReadModel : ReadModelBase
    {
        public GarmentServiceSubconFabricWashReadModel(Guid identity) : base(identity)
        {
        }
        public string ServiceSubconFabricWashNo { get; internal set; }
        public DateTimeOffset ServiceSubconFabricWashDate { get; internal set; }
        public string Remark { get; internal set; }
        public bool IsUsed { get; internal set; }
        public int QtyPacking { get; internal set; }
        public string UomUnit { get; internal set; }

        public virtual List<GarmentServiceSubconFabricWashItemReadModel> GarmentServiceSubconFabricWashItem { get; internal set; }
    }
}
