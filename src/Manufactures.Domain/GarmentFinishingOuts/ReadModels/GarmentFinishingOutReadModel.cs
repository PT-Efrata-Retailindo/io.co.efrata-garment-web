using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts.ReadModels
{
    public class GarmentFinishingOutReadModel : ReadModelBase
    {
        public GarmentFinishingOutReadModel(Guid identity) : base(identity)
        {
        }
        public string FinishingOutNo { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string FinishingTo { get; internal set; }
        public int UnitToId { get; internal set; }
        public string UnitToCode { get; internal set; }
        public string UnitToName { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset FinishingOutDate { get; internal set; }
        public bool IsDifferentSize { get; internal set; }
		public string UId { get; internal set; }
		public virtual List<GarmentFinishingOutItemReadModel> GarmentFinishingOutItem { get; internal set; }

    }
}