using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels
{
    public class GarmentSampleExpenditureGoodItemReadModel : ReadModelBase
    {
        public GarmentSampleExpenditureGoodItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid ExpenditureGoodId { get; internal set; }
        public Guid FinishedGoodStockId { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public double ReturQuantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Description { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
        public virtual GarmentSampleExpenditureGoodReadModel GarmentSampleExpenditureGood { get; internal set; }
    }
}
