using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels
{
    public class GarmentServiceSubconCuttingReadModel : ReadModelBase
    {
        public GarmentServiceSubconCuttingReadModel(Guid identity) : base(identity)
        {
        }
        public string SubconNo { get; internal set; }
        public string SubconType { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        
        public DateTimeOffset SubconDate { get; internal set; }

        public bool IsUsed { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public int QtyPacking { get; internal set; }
        public virtual List<GarmentServiceSubconCuttingItemReadModel> GarmentServiceSubconCuttingItem { get; internal set; }
    }
}
