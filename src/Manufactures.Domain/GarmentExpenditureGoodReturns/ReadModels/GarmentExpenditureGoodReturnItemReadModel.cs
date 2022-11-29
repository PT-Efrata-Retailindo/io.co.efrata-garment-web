using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels
{
    public class GarmentExpenditureGoodReturnItemReadModel : ReadModelBase
    {
        public GarmentExpenditureGoodReturnItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid ReturId { get; internal set; }
        public Guid ExpenditureGoodId { get; internal set; }
        public Guid ExpenditureGoodItemId { get; internal set; }
        public Guid FinishedGoodStockId { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Description { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
		public string UId { get; set; }
		public virtual GarmentExpenditureGoodReturnReadModel GarmentExpenditureGoodReturn { get; internal set; }

    }
}
