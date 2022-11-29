using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns
{
    public class GarmentFinishingInItem : AggregateRoot<GarmentFinishingInItem, GarmentFinishingInItemReadModel>
    {
        public Guid FinishingInId { get; private set; }
        public Guid SewingOutItemId { get; private set; }
        public Guid SewingOutDetailId { get; private set; }
        public Guid SubconCuttingId { get; private set; }
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

        public GarmentFinishingInItem(Guid identity, Guid finishingInId, Guid sewingOutItemId, Guid sewingOutDetailId, Guid subconCuttingId, SizeId sizeId, string sizeName, ProductId productId, string productCode, string productName, string designColor, double quantity, double remainingQuantity, UomId uomId, string uomUnit, string color, double basicPrice, double price) : base(identity)
        {
            FinishingInId = finishingInId;
            SewingOutItemId = sewingOutItemId;
            SewingOutDetailId = sewingOutDetailId;
            SubconCuttingId = subconCuttingId;
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
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentFinishingInItemReadModel(Identity)
            {
                FinishingInId = FinishingInId,
                SewingOutItemId = SewingOutItemId,
                SewingOutDetailId= SewingOutDetailId,
                SubconCuttingId = SubconCuttingId,
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

            ReadModel.AddDomainEvent(new OnGarmentFinishingInPlaced(Identity));
        }

        public GarmentFinishingInItem(GarmentFinishingInItemReadModel readModel) : base(readModel)
        {
            FinishingInId = readModel.FinishingInId;
            SewingOutItemId = readModel.SewingOutItemId;
            SewingOutDetailId = readModel.SewingOutDetailId;
            SubconCuttingId = readModel.SubconCuttingId;
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
            RemainingQuantity = readModel.RemainingQuantity;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentFinishingInItem GetEntity()
        {
            return this;
        }
    }
}
