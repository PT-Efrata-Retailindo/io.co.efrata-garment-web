using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentPreparings.ReadModels
{
    public class GarmentPreparingReadModel : ReadModelBase
    {
        public GarmentPreparingReadModel(Guid identity) : base(identity)
        {

        }

        public int UENId {get; internal set;}
        public string UENNo { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public DateTimeOffset? ProcessDate { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public bool IsCuttingIn { get; internal set; }
		public string UId { get; internal set; }
		public virtual List<GarmentPreparingItemReadModel> GarmentPreparingItem { get; internal set; }

    }
}