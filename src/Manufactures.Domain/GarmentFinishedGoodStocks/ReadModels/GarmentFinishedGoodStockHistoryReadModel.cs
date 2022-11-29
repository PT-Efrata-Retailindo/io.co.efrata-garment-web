using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels
{
    public class GarmentFinishedGoodStockHistoryReadModel : ReadModelBase
    {
        public GarmentFinishedGoodStockHistoryReadModel(Guid identity) : base(identity)
        {
        }
        public Guid FinishedGoodStockId { get; internal set; }
        public Guid FinishingOutItemId { get; internal set; }
        public Guid FinishingOutDetailId { get; internal set; }
        public Guid ExpenditureGoodId { get; internal set; }
        public Guid ExpenditureGoodItemId { get; internal set; }
		public Guid AdjustmentId { get; internal set; }
		public Guid AdjustmentItemId { get; internal set; }
        public Guid ExpenditureGoodReturnId { get; internal set; }
        public Guid ExpenditureGoodReturnItemId { get; internal set; }
        public string StockType { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
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
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
		public string UId { get; set; }


	}
}
