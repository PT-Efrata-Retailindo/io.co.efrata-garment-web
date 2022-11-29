using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels
{
    public class GarmentSampleCuttingInReadModel : ReadModelBase
    {
        public GarmentSampleCuttingInReadModel(Guid identity) : base(identity)
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
        public virtual ICollection<GarmentSampleCuttingInItemReadModel> Items { get; internal set; }
    }
}
