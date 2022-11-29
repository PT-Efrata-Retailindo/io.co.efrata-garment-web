using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentCuttingIns.ReadModels
{
    public class GarmentCuttingInReadModel : ReadModelBase
    {
        public GarmentCuttingInReadModel(Guid identity) : base(identity)
        {
        }

        public string CutInNo { get; internal set; }
        public string CuttingType { get; internal set; }
        public string CuttingFrom { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public DateTimeOffset CuttingInDate { get; internal set; }
        public double FC { get; internal set; }
		public string UId { get; private set; }
		public virtual ICollection<GarmentCuttingInItemReadModel> Items { get; internal set; }
    }
}
