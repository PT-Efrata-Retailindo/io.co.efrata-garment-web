using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels
{
    public class GarmentSampleAvalProductItemReadModel : ReadModelBase
    {
        public GarmentSampleAvalProductItemReadModel(Guid identity) : base(identity)
        {

        }

        public Guid APId { get; internal set; }
        public string SamplePreparingId { get; internal set; }
        public string SamplePreparingItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public double BasicPrice { get; internal set; }
        public bool IsReceived { get; internal set; }
        public string UId { get; set; }
        public virtual GarmentSampleAvalProductReadModel GarmentSampleAvalProductIdentity { get; internal set; }
    }
}
