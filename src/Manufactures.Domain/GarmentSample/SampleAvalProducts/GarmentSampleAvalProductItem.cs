using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts
{
    public class GarmentSampleAvalProductItem : AggregateRoot<GarmentSampleAvalProductItem, GarmentSampleAvalProductItemReadModel>
    {
        public Guid APId { get; private set; }
        public GarmentSamplePreparingId SamplePreparingId { get; private set; }
        public GarmentSamplePreparingItemId SamplePreparingItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public double BasicPrice { get; private set; }
        public bool IsReceived { get; private set; }

        public GarmentSampleAvalProductItem(Guid identity, Guid apId, GarmentSamplePreparingId samplePreparingId, GarmentSamplePreparingItemId samplePreparingItemId, ProductId productId, string productCode, string productName, string designColor, double quantity, UomId uomId, string uomUnit, double basicPrice, bool isReceived) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            APId = apId;
            SamplePreparingId = samplePreparingId;
            SamplePreparingItemId = samplePreparingItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            BasicPrice = basicPrice;
            IsReceived = isReceived;

            ReadModel = new GarmentSampleAvalProductItemReadModel(Identity)
            {
                APId = APId,
                SamplePreparingId = SamplePreparingId.Value,
                SamplePreparingItemId = SamplePreparingItemId.Value,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                BasicPrice = BasicPrice,
                IsReceived = IsReceived
            };
            ReadModel.AddDomainEvent(new OnGarmentSampleAvalProductPlaced(this.Identity));
        }

        public GarmentSampleAvalProductItem(GarmentSampleAvalProductItemReadModel readModel) : base(readModel)
        {
            APId = ReadModel.APId;
            SamplePreparingId = new GarmentSamplePreparingId(ReadModel.SamplePreparingId);
            SamplePreparingItemId = new GarmentSamplePreparingItemId(ReadModel.SamplePreparingItemId);
            ProductId = new ProductId(ReadModel.ProductId);
            ProductCode = ReadModel.ProductCode;
            ProductName = ReadModel.ProductName;
            DesignColor = ReadModel.DesignColor;
            Quantity = ReadModel.Quantity;
            UomId = new UomId(ReadModel.UomId);
            UomUnit = ReadModel.UomUnit;
            BasicPrice = ReadModel.BasicPrice;
            IsReceived = ReadModel.IsReceived;
        }

        public void setSamplePreparingId(GarmentSamplePreparingId newSamplePreparingId)
        {
            Validator.ThrowIfNull(() => newSamplePreparingId);

            if (newSamplePreparingId != SamplePreparingId)
            {
                SamplePreparingId = newSamplePreparingId;
                ReadModel.SamplePreparingId = newSamplePreparingId.Value;
            }
        }

        public void setSamplePreparingItemId(GarmentSamplePreparingItemId newSamplePreparingItemId)
        {
            Validator.ThrowIfNull(() => newSamplePreparingItemId);

            if (newSamplePreparingItemId != SamplePreparingItemId)
            {
                SamplePreparingItemId = newSamplePreparingItemId;
                ReadModel.SamplePreparingItemId = newSamplePreparingItemId.Value;
            }
        }

        public void setProductId(ProductId newProductId)
        {
            Validator.ThrowIfNull(() => newProductId);

            if (newProductId != ProductId)
            {
                ProductId = newProductId;
                ReadModel.ProductId = newProductId.Value;
            }
        }

        public void setProductCode(string newProductCode)
        {
            Validator.ThrowIfNullOrEmpty(() => newProductCode);

            if (newProductCode != ProductCode)
            {
                ProductCode = newProductCode;
                ReadModel.ProductCode = newProductCode;
            }
        }

        public void setProductName(string newProductName)
        {
            Validator.ThrowIfNullOrEmpty(() => newProductName);

            if (newProductName != ProductName)
            {
                ProductName = newProductName;
                ReadModel.ProductName = newProductName;
            }
        }

        public void setDesignColor(string newDesignColor)
        {
            Validator.ThrowIfNullOrEmpty(() => newDesignColor);

            if (newDesignColor != DesignColor)
            {
                DesignColor = newDesignColor;
                ReadModel.DesignColor = newDesignColor;
            }
        }

        public void setQuantity(double newQuantity)
        {
            Validator.ThrowWhenTrue(() => newQuantity < 0);


            if (newQuantity != Quantity)
            {
                Quantity = newQuantity;
                ReadModel.Quantity = newQuantity;
            }
        }

        public void setUomId(UomId newUomId)
        {
            Validator.ThrowIfNull(() => newUomId);

            if (newUomId != UomId)
            {
                UomId = newUomId;
                ReadModel.UomId = newUomId.Value;
            }
        }

        public void setUomUnit(string newUomUnit)
        {
            Validator.ThrowIfNullOrEmpty(() => newUomUnit);

            if (newUomUnit != UomUnit)
            {
                UomUnit = newUomUnit;
                ReadModel.UomUnit = newUomUnit;
            }
        }

        public void SetIsReceived(bool IsReceived)
        {
            if (this.IsReceived != IsReceived)
            {
                this.IsReceived = IsReceived;
                ReadModel.IsReceived = IsReceived;
            }
        }

        public void SetDeleted()
        {
            MarkModified();
        }

        protected override GarmentSampleAvalProductItem GetEntity()
        {
            return this;
        }
    }
}
