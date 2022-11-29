using Infrastructure.Domain;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.GarmentAvalComponents
{
    public class GarmentAvalComponentItem : AggregateRoot<GarmentAvalComponentItem, GarmentAvalComponentItemReadModel>
    {
        public Guid AvalComponentId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public Guid SewingOutItemId { get; internal set; }
        public Guid SewingOutDetailId { get; internal set; }
        public ProductId ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public string Color { get; internal set; }
        public double Quantity { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public SizeId SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public decimal Price { get; internal set; }
        public decimal BasicPrice { get; internal set; }

        public GarmentAvalComponentItem(Guid Identity, Guid avalComponentId, Guid cuttingInDetailId, Guid sewingOutItemId, Guid sewingOutDetailId, ProductId productId, string productCode, string productName, string designColor, string color, double quantity, double remainingQuantity, SizeId sizeId, string sizeName, decimal price, decimal basicPrice) : base(Identity)
        {
            AvalComponentId = avalComponentId;
            CuttingInDetailId = cuttingInDetailId;
            SewingOutItemId = sewingOutItemId;
            SewingOutDetailId = sewingOutDetailId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            Color = color;
            Quantity = quantity;
            RemainingQuantity = remainingQuantity;
            SizeId = sizeId;
            SizeName = sizeName;
            Price = price;
            BasicPrice = basicPrice;

            ReadModel = new GarmentAvalComponentItemReadModel(Identity)
            {
                AvalComponentId = avalComponentId,
                CuttingInDetailId = cuttingInDetailId,
                SewingOutItemId = sewingOutItemId,
                SewingOutDetailId = sewingOutDetailId,
                ProductId = productId.Value,
                ProductCode = productCode,
                ProductName = productName,
                DesignColor = designColor,
                Color = color,
                Quantity = quantity,
                RemainingQuantity = remainingQuantity,
                SizeId = sizeId.Value,
                SizeName = sizeName,
                Price = price,
                BasicPrice=basicPrice
            };
        }

        public GarmentAvalComponentItem(GarmentAvalComponentItemReadModel readModel) : base(readModel)
        {
            AvalComponentId = readModel.AvalComponentId;
            CuttingInDetailId = readModel.CuttingInDetailId;
            SewingOutItemId = readModel.SewingOutItemId;
            SewingOutDetailId = readModel.SewingOutDetailId;
            ProductId = new ProductId((int)readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            Color = readModel.Color;
            Quantity = readModel.Quantity;
            RemainingQuantity = readModel.RemainingQuantity;
            SizeId = new SizeId((int)readModel.SizeId);
            SizeName = readModel.SizeName;
            Price = readModel.Price;
            BasicPrice = readModel.BasicPrice;
        }

        protected override GarmentAvalComponentItem GetEntity()
        {
            return this;
        }
    }
}
