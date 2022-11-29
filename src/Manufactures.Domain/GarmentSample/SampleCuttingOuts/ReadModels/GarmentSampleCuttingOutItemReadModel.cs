using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels
{
    public class GarmentSampleCuttingOutItemReadModel : ReadModelBase
    {
        public GarmentSampleCuttingOutItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CuttingOutId { get; internal set; }
        public Guid CuttingInId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public double TotalCuttingOut { get; internal set; }
        public string UId { get; private set; }

        public virtual ICollection<GarmentSampleCuttingOutDetailReadModel> GarmentSampleCuttingOutDetail { get; internal set; }
        public virtual GarmentSampleCuttingOutReadModel GarmentSampleCuttingOutIdentity { get; internal set; }
    }
}
