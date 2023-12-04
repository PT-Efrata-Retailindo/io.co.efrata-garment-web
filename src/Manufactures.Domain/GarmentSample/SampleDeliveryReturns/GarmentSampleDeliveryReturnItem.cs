using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleDeliveryReturns
{
    public class GarmentSampleDeliveryReturnItem : AggregateRoot<GarmentSampleDeliveryReturnItem, GarmentSampleDeliveryReturnItemReadModel>
    {
        public Guid DRId { get; private set; }
        public int UnitDOItemId { get; private set; }
        public int UENItemId { get; private set; }
        public string PreparingItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public string RONo { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Colour { get; private set; }
        public string Rack { get; private set; }
        public string Level { get; private set; }
        public string Box { get; private set; }
        public string Area { get; private set; }

        public GarmentSampleDeliveryReturnItem(Guid identity, Guid drId, int unitDOItemId, int uenItemId, string preparingItemId, ProductId productId, string productCode, string productName, string designColor, string roNo, double quantity, UomId uomId, string uomUnit, string colour, string rack, string level, string box, string area) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            DRId = drId;
            UnitDOItemId = unitDOItemId;
            UENItemId = uenItemId;
            PreparingItemId = preparingItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            RONo = roNo;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Area = area;
            Box = box;
            Colour = colour;
            Rack = rack;
            Level = level;

            ReadModel = new GarmentSampleDeliveryReturnItemReadModel(identity)
            {
                DRId = DRId,
                UnitDOItemId = UnitDOItemId,
                UENItemId = UENItemId,
                PreparingItemId = PreparingItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                RONo = RONo,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Area = Area,
                Box = Box,
                Colour = Colour,
                Rack = Rack,
                Level = Level,
            };
            ReadModel.AddDomainEvent(new OnGarmentSampleDeliveryReturnPlaced(this.Identity));
        }
        public GarmentSampleDeliveryReturnItem(GarmentSampleDeliveryReturnItemReadModel readModel) : base(readModel)
        {
            DRId = ReadModel.DRId;
            UnitDOItemId = ReadModel.UnitDOItemId;
            UENItemId = ReadModel.UENItemId;
            PreparingItemId = ReadModel.PreparingItemId;
            ProductId = new ProductId(ReadModel.ProductId);
            ProductCode = ReadModel.ProductCode;
            ProductName = ReadModel.ProductName;
            DesignColor = ReadModel.DesignColor;
            RONo = ReadModel.RONo;
            Quantity = ReadModel.Quantity;
            UomId = new UomId(ReadModel.UomId);
            UomUnit = ReadModel.UomUnit;
            Area = ReadModel.Area;
            Box = ReadModel.Box;
            Colour = ReadModel.Colour;
            Rack = ReadModel.Rack;
            Level = ReadModel.Level;
        }

        public void setUnitDOItemId(int newUnitDOItemId)
        {
            if (newUnitDOItemId != UnitDOItemId)
            {
                UnitDOItemId = newUnitDOItemId;
                ReadModel.UnitDOItemId = newUnitDOItemId;
            }
        }
        public void setUENItemId(int newUENItemId)
        {
            if (newUENItemId != UENItemId)
            {
                UENItemId = newUENItemId;
                ReadModel.UENItemId = newUENItemId;
            }
        }
        public void setPreparingItemId(string newPreparingItemId)
        {
            if (newPreparingItemId != PreparingItemId)
            {
                PreparingItemId = newPreparingItemId;
                ReadModel.PreparingItemId = newPreparingItemId;
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
        public void setRONo(string newRONo)
        {
            Validator.ThrowIfNull(() => newRONo);

            if (newRONo != RONo)
            {
                RONo = newRONo;
                ReadModel.RONo = newRONo;
            }
        }
        public void setQuantity(double newQuantity)
        {
            if (newQuantity != Quantity)
            {
                Quantity = newQuantity;
                ReadModel.Quantity = newQuantity;
            }
        }
        public void setUomId(UomId newUom)
        {
            Validator.ThrowIfNull(() => newUom);

            if (newUom != UomId)
            {
                UomId = newUom;
                ReadModel.UomId = newUom.Value;
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
        public void SetModified()
        {
            MarkModified();
        }
        public void SetDeleted()
        {
            MarkRemoved();
        }
        protected override GarmentSampleDeliveryReturnItem GetEntity()
        {
            return this;
        }
    }
}
