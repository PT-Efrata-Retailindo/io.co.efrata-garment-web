using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAdjustments.ReadModels
{
    public class GarmentAdjustmentReadModel : ReadModelBase
    {
        public GarmentAdjustmentReadModel(Guid identity) : base(identity)
        {

        }

        public string AdjustmentNo { get; internal set; }
        public string AdjustmentType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public DateTimeOffset AdjustmentDate { get; internal set; }
        public string AdjustmentDesc { get; internal set; }
		public string UId { get; set; }
		public virtual List<GarmentAdjustmentItemReadModel> Items { get; internal set; }

    }
}
