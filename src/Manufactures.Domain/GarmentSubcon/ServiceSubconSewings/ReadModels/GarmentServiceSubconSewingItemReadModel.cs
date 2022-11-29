using System;
using System.Collections.Generic;
using Infrastructure.Domain.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels
{
    public class GarmentServiceSubconSewingItemReadModel : ReadModelBase
    {
        public GarmentServiceSubconSewingItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSubconSewingId { get; internal set; }

        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }

        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }

        public virtual GarmentServiceSubconSewingReadModel GarmentServiceSubconSewingIdentity { get; internal set; }
        public virtual List<GarmentServiceSubconSewingDetailReadModel> GarmentServiceSubconSewingDetail { get; internal set; }
    }
}
