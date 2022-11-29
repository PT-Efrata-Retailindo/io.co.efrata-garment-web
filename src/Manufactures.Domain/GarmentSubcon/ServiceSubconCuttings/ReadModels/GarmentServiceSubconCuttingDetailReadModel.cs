using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels
{
    public class GarmentServiceSubconCuttingDetailReadModel : ReadModelBase
    {
        public GarmentServiceSubconCuttingDetailReadModel(Guid identity) : base(identity)
        {
        }
        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public Guid ServiceSubconCuttingItemId { get; internal set; }

        public virtual GarmentServiceSubconCuttingItemReadModel GarmentServiceSubconCuttingItem { get; internal set; }
        public virtual List<GarmentServiceSubconCuttingSizeReadModel> GarmentServiceSubconCuttingSizes { get; internal set; }
    }
}
