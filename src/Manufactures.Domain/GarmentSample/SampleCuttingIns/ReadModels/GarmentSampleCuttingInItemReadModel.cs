using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels
{
    public class GarmentSampleCuttingInItemReadModel : ReadModelBase
    {
        public GarmentSampleCuttingInItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutInId { get; internal set; }
        public Guid PreparingId { get; internal set; }
        public Guid SewingOutId { get; internal set; }
        public string SewingOutNo { get; internal set; }
        public int UENId { get; internal set; }
        public string UENNo { get; internal set; }
        public virtual GarmentSampleCuttingInReadModel GarmentSampleCuttingIn { get; internal set; }
        public virtual ICollection<GarmentSampleCuttingInDetailReadModel> Details { get; internal set; }
    }
}
