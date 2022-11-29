using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.ReadModels
{
    public class GarmentSewingOutReadModel : ReadModelBase
    {
        public GarmentSewingOutReadModel(Guid identity) : base(identity)
        {
        }
        public string SewingOutNo { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string SewingTo { get; internal set; }
        public int UnitToId { get; internal set; }
        public string UnitToCode { get; internal set; }
        public string UnitToName { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset SewingOutDate { get; internal set; }
        public bool IsDifferentSize { get; internal set; }
		public string UId { get; private set; }
		public virtual List<GarmentSewingOutItemReadModel> GarmentSewingOutItem { get; internal set; }

    }
}
