using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels
{
    public class GarmentSubconCustomsOutItemReadModel : ReadModelBase
    {
        public GarmentSubconCustomsOutItemReadModel(Guid identity) : base(identity)
        {
        }
        public string SubconDLOutNo { get; internal set; }
        public Guid SubconDLOutId { get; internal set; }
        public double Quantity { get; internal set; }
        public Guid SubconCustomsOutId { get; internal set; }
        public virtual GarmentSubconCustomsOutReadModel GarmentSubconCustomsOut { get; internal set; }
    }
}
