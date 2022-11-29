using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSewingIns
{
    public class GarmentSewingInItem : AggregateRoot<GarmentSewingInItem, GarmentSewingInItemReadModel>
    {
        public Guid SewingInId { get; private set; }
        public Guid SewingOutItemId { get; private set; }
        public Guid SewingOutDetailId { get; private set; }
        public Guid LoadingItemId { get; private set; }
        public Guid FinishingOutItemId { get; private set; }
        public Guid FinishingOutDetailId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Color { get; private set; }
        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }

        public void SetRemainingQuantity(double RemainingQuantity)
        {
            if (this.RemainingQuantity != RemainingQuantity)
            {
                this.RemainingQuantity = RemainingQuantity;
                ReadModel.RemainingQuantity = RemainingQuantity;
            }
        }

        public GarmentSewingInItem(Guid identity, Guid sewingInId, Guid sewingOutItemId, Guid sewingOutDetailId , Guid loadingItemId, Guid finishingOutItemId, Guid finishingOutDetailId, ProductId productId, string productCode, string productName, string designColor, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, double remainingQuantity, double basicPrice, double price) : base(identity)
        {
            Identity = identity;
            SewingInId = sewingInId;
            SewingOutItemId = sewingOutItemId;
            SewingOutDetailId = sewingOutDetailId;
            LoadingItemId = loadingItemId;
            FinishingOutItemId = finishingOutItemId;
            FinishingOutDetailId = finishingOutDetailId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentSewingInItemReadModel(identity)
            {
                SewingInId = SewingInId,
                SewingOutItemId= SewingOutItemId,
                SewingOutDetailId= SewingOutDetailId,
                LoadingItemId = LoadingItemId,
                FinishingOutItemId= FinishingOutItemId,
                FinishingOutDetailId = FinishingOutDetailId,
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
                RemainingQuantity = RemainingQuantity,
                BasicPrice=BasicPrice,
                Price=Price
            };

            ReadModel.AddDomainEvent(new OnGarmentSewingInPlaced(Identity));
        }

        public GarmentSewingInItem(GarmentSewingInItemReadModel readModel) : base(readModel)
        {
            SewingInId = readModel.SewingInId;
            SewingOutItemId = readModel.SewingOutItemId;
            SewingOutDetailId = readModel.SewingOutDetailId;
            LoadingItemId = readModel.LoadingItemId;
            FinishingOutItemId = readModel.FinishingOutItemId;
            FinishingOutDetailId = readModel.FinishingOutDetailId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Color = readModel.Color;
            RemainingQuantity = readModel.RemainingQuantity;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSewingInItem GetEntity()
        {
            return this;
        }
    }
}