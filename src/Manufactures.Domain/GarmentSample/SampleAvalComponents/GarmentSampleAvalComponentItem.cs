using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents
{
    public class GarmentSampleAvalComponentItem : AggregateRoot<GarmentSampleAvalComponentItem, GarmentSampleAvalComponentItemReadModel>
    {
        public Guid SampleAvalComponentId { get; internal set; }
        public Guid SampleCuttingInDetailId { get; internal set; }
        public Guid SampleSewingOutItemId { get; internal set; }
        public Guid SampleSewingOutDetailId { get; internal set; }
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

        public GarmentSampleAvalComponentItem(Guid Identity, Guid sampleAvalComponentId, Guid sampleCuttingInDetailId, Guid sampleSewingOutItemId, Guid sampleSewingOutDetailId, ProductId productId, string productCode, string productName, string designColor, string color, double quantity, double remainingQuantity, SizeId sizeId, string sizeName, decimal price, decimal basicPrice) : base(Identity)
        {
            SampleAvalComponentId = sampleAvalComponentId;
            SampleCuttingInDetailId = sampleCuttingInDetailId;
            SampleSewingOutItemId = sampleSewingOutItemId;
            SampleSewingOutDetailId = sampleSewingOutDetailId;
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

            ReadModel = new GarmentSampleAvalComponentItemReadModel(Identity)
            {
                SampleAvalComponentId = sampleAvalComponentId,
                SampleCuttingInDetailId = sampleCuttingInDetailId,
                SampleSewingOutItemId = sampleSewingOutItemId,
                SampleSewingOutDetailId = sampleSewingOutDetailId,
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
                BasicPrice = basicPrice
            };
        }

        public GarmentSampleAvalComponentItem(GarmentSampleAvalComponentItemReadModel readModel) : base(readModel)
        {
            SampleAvalComponentId = readModel.SampleAvalComponentId;
            SampleCuttingInDetailId = readModel.SampleCuttingInDetailId;
            SampleSewingOutItemId = readModel.SampleSewingOutItemId;
            SampleSewingOutDetailId = readModel.SampleSewingOutDetailId;
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

        protected override GarmentSampleAvalComponentItem GetEntity()
        {
            return this;
        }
    }
}
