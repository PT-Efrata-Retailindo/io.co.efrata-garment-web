using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentCuttingIns.ReadModels
{
    public class GarmentCuttingInItemReadModel : ReadModelBase
    {
        public GarmentCuttingInItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutInId { get; internal set; }
        public Guid PreparingId { get; internal set; }
        public Guid SewingOutId { get; internal set; }
        public string SewingOutNo { get; internal set; }
        public int UENId { get; internal set; }
        public string UENNo { get; internal set; }
		public string UId { get; private set; }
		public virtual GarmentCuttingInReadModel GarmentCuttingIn { get; internal set; }
        public virtual ICollection<GarmentCuttingInDetailReadModel> Details { get; internal set; }
    }
}
