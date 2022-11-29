using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingIns
{
    public class GarmentSampleFinishingInItem : AggregateRoot<GarmentSampleFinishingInItem, GarmentSampleFinishingInItemReadModel>
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
        public long DODetailId { get; private set; }
        public Guid Identity { get; private set; }

        public void SetRemainingQuantity(double RemainingQuantity)
        {
            if (this.RemainingQuantity != RemainingQuantity)
            {
                this.RemainingQuantity = RemainingQuantity;
                ReadModel.RemainingQuantity = RemainingQuantity;
            }
        }

        public GarmentSampleFinishingInItem(Guid identity, Guid finishingInId, Guid sewingOutItemId, Guid sewingOutDetailId, Guid subconCuttingId, SizeId sizeId, string sizeName, ProductId productId, string productCode, string productName, string designColor, double quantity, double remainingQuantity, UomId uomId, string uomUnit, string color, double basicPrice, double price,int doDetailId) : base(identity)
        {
            Identity = identity;
            FinishingInId = finishingInId;
            SewingOutItemId = sewingOutItemId;
            SewingOutDetailId = sewingOutDetailId;
            SubconCuttingId = subconCuttingId;
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
            DODetailId = doDetailId;

            ReadModel = new GarmentSampleFinishingInItemReadModel(Identity)
            {
                
                FinishingInId = FinishingInId,
                SewingOutItemId = SewingOutItemId,
                SewingOutDetailId = SewingOutDetailId,
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
                BasicPrice = BasicPrice,
                Price = Price,
                DODetailId = DODetailId
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleFinishingInPlaced(Identity));
        }

        public GarmentSampleFinishingInItem(GarmentSampleFinishingInItemReadModel readModel) : base(readModel)
        {
            Identity = readModel.Identity;
            FinishingInId = readModel.FinishingInId;
            SewingOutItemId = readModel.SewingOutItemId;
            SewingOutDetailId = readModel.SewingOutDetailId;
            SubconCuttingId = readModel.SubconCuttingId;
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
            DODetailId = readModel.DODetailId;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSampleFinishingInItem GetEntity()
        {
            return this;
        }
    }
}
