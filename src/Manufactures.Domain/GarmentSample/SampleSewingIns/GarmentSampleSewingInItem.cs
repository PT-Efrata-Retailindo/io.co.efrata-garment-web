using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingIns
{
    public class GarmentSampleSewingInItem : AggregateRoot<GarmentSampleSewingInItem, GarmentSampleSewingInItemReadModel>
    {
        public Guid SewingInId { get; private set; }
        public Guid CuttingOutItemId { get; private set; }
        public Guid CuttingOutDetailId { get; private set; }
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

        public GarmentSampleSewingInItem(Guid identity, Guid sewingInId, Guid cuttingOutItemId, Guid cuttingOutDetailId, Guid finishingOutItemId, Guid finishingOutDetailId, ProductId productId, string productCode, string productName, string designColor, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, double remainingQuantity, double basicPrice, double price) : base(identity)
        {
            Identity = identity;
            SewingInId = sewingInId;
            CuttingOutItemId = cuttingOutItemId;
            CuttingOutDetailId = cuttingOutDetailId;
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

            ReadModel = new GarmentSampleSewingInItemReadModel(identity)
            {
                SewingInId = SewingInId,
                CuttingOutItemId = CuttingOutItemId,
                CuttingOutDetailId = CuttingOutDetailId,
                FinishingOutItemId = FinishingOutItemId,
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
                BasicPrice = BasicPrice,
                Price = Price
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleSewingInPlaced(Identity));
        }

        public GarmentSampleSewingInItem(GarmentSampleSewingInItemReadModel readModel) : base(readModel)
        {
            SewingInId = readModel.SewingInId;
            CuttingOutItemId = readModel.CuttingOutItemId;
            CuttingOutDetailId = readModel.CuttingOutDetailId;
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

        protected override GarmentSampleSewingInItem GetEntity()
        {
            return this;
        }
    }
}
