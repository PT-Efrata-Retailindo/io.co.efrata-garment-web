using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels
{
    public class GarmentSampleFinishingInReadModel : ReadModelBase
    {
        public GarmentSampleFinishingInReadModel(Guid identity) : base(identity)
        {

        }
        public string FinishingInNo { get; internal set; }
        public string FinishingInType { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public int UnitFromId { get; internal set; }
        public string UnitFromCode { get; internal set; }
        public string UnitFromName { get; internal set; }
        public string Article { get; internal set; }
        public string RONo { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset FinishingInDate { get; internal set; }

        public long DOId { get; internal set; }
        public string DONo { get; internal set; }
        public string UId { get; internal set; }
        public string SubconType { get; internal set; }
        public virtual List<GarmentSampleFinishingInItemReadModel> Items { get; internal set; }

    }
}
