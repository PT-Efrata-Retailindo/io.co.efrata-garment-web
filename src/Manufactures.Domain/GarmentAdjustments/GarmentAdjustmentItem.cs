using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAdjustments
{
    public class GarmentAdjustmentItem : AggregateRoot<GarmentAdjustmentItem, GarmentAdjustmentItemReadModel>
    {
        public Guid AdjustmentId { get; internal set; }
        public Guid SewingDOItemId { get; internal set; }
        public Guid SewingInItemId { get; internal set; }
        public Guid FinishingInItemId { get; internal set; }
		public Guid FinishedGoodStockId { get; internal set; }		
		public ProductId ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public SizeId SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public UomId UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
		public Guid Id { get; internal set; }

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

        public GarmentAdjustmentItem(Guid identity, Guid loadingId, Guid sewingDOItemId, Guid sewingInItemId, Guid finishingInItemId,Guid finishedGoodStockId, SizeId sizeId, string sizeName, ProductId productId, string productCode, string productName, string designColor, double quantity, double basicPrice, UomId uomId, string uomUnit, string color, double price) : base(identity)
        {
			Id = identity;
            AdjustmentId = loadingId;
            SewingDOItemId = sewingDOItemId;
            SewingInItemId = sewingInItemId;
            FinishingInItemId = finishingInItemId;
			FinishedGoodStockId = finishedGoodStockId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productCode;
            DesignColor = designColor;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentAdjustmentItemReadModel(Identity)
            {
				
                AdjustmentId = loadingId,
                SewingDOItemId = SewingDOItemId,
                SewingInItemId= SewingInItemId,
				FinishedGoodStockId= FinishedGoodStockId,
                FinishingInItemId= FinishingInItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Color = Color,
                BasicPrice = BasicPrice,
                Price = Price
            };

            ReadModel.AddDomainEvent(new OnGarmentAdjustmentPlaced(Identity));
        }

        public GarmentAdjustmentItem(GarmentAdjustmentItemReadModel readModel) : base(readModel)
        {
            AdjustmentId = readModel.AdjustmentId;
            SewingDOItemId = readModel.SewingDOItemId;
            SewingInItemId = readModel.SewingInItemId;
            FinishingInItemId = readModel.FinishingInItemId;
			FinishedGoodStockId = readModel.FinishedGoodStockId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductCode;
            DesignColor = readModel.DesignColor;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Color = readModel.Color;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentAdjustmentItem GetEntity()
        {
            return this;
        }
    }
}