using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSewingDOs
{
    public class GarmentSewingDOItem : AggregateRoot<GarmentSewingDOItem, GarmentSewingDOItemReadModel>
    {
        public Guid SewingDOId { get; private set; }
        public Guid CuttingOutDetailId { get; private set; }
        public Guid CuttingOutItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Color { get; private set; }
        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }

        public GarmentSewingDOItem(Guid identity, Guid sewingDOId, Guid cuttingOutDetailId, Guid cuttingOutItemId, ProductId productId, string productCode, string productName, string designColor, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, double remainingQuantity, double basicPrice, double price) : base(identity)
        {
            Identity = identity;
            SewingDOId = sewingDOId;
            CuttingOutDetailId = cuttingOutDetailId;
            CuttingOutItemId = cuttingOutItemId;
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

            ReadModel = new GarmentSewingDOItemReadModel(identity)
            {
                SewingDOId = SewingDOId,
                CuttingOutDetailId = CuttingOutDetailId,
                CuttingOutItemId = CuttingOutItemId,
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
                Price=price
            };

            ReadModel.AddDomainEvent(new OnGarmentSewingDOPlaced(Identity));
        }

        public GarmentSewingDOItem(GarmentSewingDOItemReadModel readModel) : base(readModel)
        {
            SewingDOId = readModel.SewingDOId;
            CuttingOutDetailId = readModel.CuttingOutDetailId;
            CuttingOutItemId = readModel.CuttingOutItemId;
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

        public void setRemainingQuantity(double newRemainingQuantity)
        {
            if (newRemainingQuantity != RemainingQuantity)
            {
                RemainingQuantity = newRemainingQuantity;
                ReadModel.RemainingQuantity = newRemainingQuantity;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSewingDOItem GetEntity()
        {
            return this;
        }
    }
}