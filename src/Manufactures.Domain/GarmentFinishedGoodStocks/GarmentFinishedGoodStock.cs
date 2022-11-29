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
    public class GarmentFinishedGoodStock : AggregateRoot<GarmentFinishedGoodStock, GarmentFinishedGoodStockReadModel>
    {

        public string FinishedGoodStockNo { get; private set; }
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
		public double ComodityPrice { get; private set; }


		public GarmentFinishedGoodStock(Guid identity, string finishedGoodStockNo, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, GarmentComodityId comodityId, string comodityCode, string comodityName, SizeId sizeId, string sizeName, UomId uomId, string uomUnit, double quantity, double basicPrice, double price) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => rONo);

            //MarkTransient();

            Identity = identity;
            FinishedGoodStockNo = finishedGoodStockNo;
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
			 
			ReadModel = new GarmentFinishedGoodStockReadModel(Identity)
            {
                FinishedGoodStockNo = FinishedGoodStockNo,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                SizeId = SizeId.Value,
                SizeName= SizeName,
                Quantity= Quantity,
                UomId= UomId.Value,
                UomUnit= UomUnit,
                BasicPrice= BasicPrice,
                Price= Price 
				
            };

            ReadModel.AddDomainEvent(new OnGarmentFinishedGoodStockPlaced(Identity));
        }

        public GarmentFinishedGoodStock(GarmentFinishedGoodStockReadModel readModel) : base(readModel)
        {
            FinishedGoodStockNo = readModel.FinishedGoodStockNo;
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

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
		 
		public void SetPrice(double Price)
        {
            if (this.Price != Price)
            {
                this.Price = Price;
                ReadModel.Price = Price;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentFinishedGoodStock GetEntity()
        {
            return this;
        }
    }
}
