using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentComodityPrices.ReadModels
{
    public class GarmentComodityPriceReadModel : ReadModelBase
    {
        public GarmentComodityPriceReadModel(Guid identity) : base(identity)
        {
        }
        public bool IsValid { get; internal set; }
        public DateTimeOffset Date { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public decimal Price { get; internal set; }

    }
}
