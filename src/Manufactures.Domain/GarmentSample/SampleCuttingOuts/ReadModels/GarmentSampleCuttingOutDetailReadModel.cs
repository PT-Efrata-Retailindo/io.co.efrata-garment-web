using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels
{
    public class GarmentSampleCuttingOutDetailReadModel : ReadModelBase
    {
        public GarmentSampleCuttingOutDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CuttingOutItemId { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double CuttingOutQuantity { get; internal set; }
        public int CuttingOutUomId { get; internal set; }
        public string CuttingOutUomUnit { get; internal set; }
        public string Color { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
        public string Remark { get; internal set; }
        public virtual GarmentSampleCuttingOutItemReadModel GarmentSampleCuttingOutItemIdentity { get; internal set; }

    }
}
