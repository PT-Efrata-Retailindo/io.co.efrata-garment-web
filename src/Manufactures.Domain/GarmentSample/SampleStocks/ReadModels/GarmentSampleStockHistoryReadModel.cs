using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleStocks.ReadModels
{
    public class GarmentSampleStockHistoryReadModel : ReadModelBase
    {
        public GarmentSampleStockHistoryReadModel(Guid identity) : base(identity)
        {
        }
        public Guid ExpenditureGoodId { get; internal set; }
        public Guid ExpenditureGoodItemId { get; internal set; }
        public string StockType { get; internal set; }
        public string ArchiveType { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Description { get; internal set; }
    }
}
