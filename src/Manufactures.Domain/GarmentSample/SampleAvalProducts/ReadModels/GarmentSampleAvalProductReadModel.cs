using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels
{
    public class GarmentSampleAvalProductReadModel : ReadModelBase
    {
        public GarmentSampleAvalProductReadModel(Guid identity) : base(identity)
        {

        }

        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public DateTimeOffset? AvalDate { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string UId { get; set; }
        public virtual List<GarmentSampleAvalProductItemReadModel> GarmentSampleAvalProductItem { get; internal set; }
    }
}
