using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishedGoodStocks
{
    public class GarmentFinishedGoodStockHistory : AggregateRoot<GarmentFinishedGoodStockHistory, GarmentFinishedGoodStockHistoryReadModel>
    {

        public Guid FinishedGoodStockId { get; private set; }
        public Guid FinishingOutItemId { get; private set; }
        public Guid FinishingOutDetailId { get; private set; }
        public Guid ExpenditureGoodId { get; private set; }
        public Guid ExpenditureGoodItemId { get; private set; }
		public Guid AdjustmentId { get; private set; }
		public Guid AdjustmentItemId { get; private set; }
        public Guid ExpenditureGoodReturnId { get; private set; }
        public Guid ExpenditureGoodReturnItemId { get; private set; }
        public string StockType { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }

        public GarmentFinishedGoodStockHistory(Guid identity, Guid finishedGoodStockId, Guid finishingOutItemId,Guid finishingOutDetailId, Guid expenditureGoodId, Guid expenditureGoodItemId,Guid adjustmentId,Guid adjustmentItemId, Guid expenditureGoodReturnId, Guid expenditureGoodReturnItemId, string stockType, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, GarmentComodityId comodityId, string comodityCode, string comodityName, SizeId sizeId, string sizeName, UomId uomId, string uomUnit, double quantity, double basicPrice, double price) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => rONo);

            //MarkTransient();

            Identity = identity;
            FinishedGoodStockId = finishedGoodStockId;
            FinishingOutItemId = finishingOutItemId;
            FinishingOutDetailId = finishingOutDetailId;
            ExpenditureGoodId = expenditureGoodId;
            ExpenditureGoodItemId = expenditureGoodItemId;
            ExpenditureGoodReturnId = expenditureGoodReturnId;
            ExpenditureGoodReturnItemId = expenditureGoodReturnItemId;
			AdjustmentId = adjustmentId;
			AdjustmentItemId = adjustmentItemId;
            StockType = stockType;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentFinishedGoodStockHistoryReadModel(Identity)
            {
                FinishedGoodStockId = FinishedGoodStockId,
                FinishingOutItemId= FinishingOutItemId,
                FinishingOutDetailId= FinishingOutDetailId,
                ExpenditureGoodId= ExpenditureGoodId,
                ExpenditureGoodItemId= ExpenditureGoodItemId,
                ExpenditureGoodReturnId= ExpenditureGoodReturnId,
                ExpenditureGoodReturnItemId= ExpenditureGoodReturnItemId,
                AdjustmentId =AdjustmentId,
				AdjustmentItemId=AdjustmentItemId,
                StockType=StockType,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                BasicPrice = BasicPrice,
                Price = Price
            };

            ReadModel.AddDomainEvent(new OnGarmentFinishedGoodStockHistoryPlaced(Identity));
        }

        public GarmentFinishedGoodStockHistory(GarmentFinishedGoodStockHistoryReadModel readModel) : base(readModel)
        {
            FinishedGoodStockId = readModel.FinishedGoodStockId;
            FinishingOutItemId = readModel.FinishingOutItemId;
            FinishingOutDetailId = readModel.FinishingOutDetailId;
            ExpenditureGoodId = readModel.ExpenditureGoodId;
            ExpenditureGoodItemId = readModel.ExpenditureGoodItemId;
			AdjustmentItemId = readModel.AdjustmentItemId;
			AdjustmentId = readModel.AdjustmentId;
            ExpenditureGoodReturnId = readModel.ExpenditureGoodReturnId;
            ExpenditureGoodReturnItemId = readModel.ExpenditureGoodReturnItemId;
            StockType = readModel.StockType;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomUnit = readModel.UomUnit;
            UomId = new UomId(readModel.UomId);
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentFinishedGoodStockHistory GetEntity()
        {
            return this;
        }
    }
}
