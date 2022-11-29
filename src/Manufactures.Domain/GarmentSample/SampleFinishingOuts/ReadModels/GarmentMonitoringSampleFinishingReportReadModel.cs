using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels
{
    public class GarmentMonitoringSampleFinishingReportReadModel : ReadModelBase
    {
        public GarmentMonitoringSampleFinishingReportReadModel(Guid identity) : base(identity)
        {
        }
        public string RoJob { get; internal set; }
        public string Article { get; internal set; }
        public double Stock { get; internal set; }
        public double SewingQtyPcs { get; internal set; }
        public double FinishingQtyPcs { get; internal set; }
        public double RemainQty { get; internal set; }
        public string UomUnit { get; internal set; }
    }
}
