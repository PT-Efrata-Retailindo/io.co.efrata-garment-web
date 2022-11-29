using System;
using System.Collections.Generic;
using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.GarmentSewingIns.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels
{
    public class GarmentServiceSubconSewingReadModel : ReadModelBase
    {
        public GarmentServiceSubconSewingReadModel(Guid identity) : base(identity)
        {
        }

        public string ServiceSubconSewingNo { get; internal set; }
        public DateTimeOffset ServiceSubconSewingDate { get; internal set; }
        public bool IsUsed { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public int QtyPacking { get; internal set; }
        public string UomUnit { get; internal set; }
        public virtual List<GarmentServiceSubconSewingItemReadModel> GarmentServiceSubconSewingItem { get; internal set; }

    }
}
