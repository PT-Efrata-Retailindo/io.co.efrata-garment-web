using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;
namespace Manufactures.Domain.GarmentDeliveryReturns
{
    public class GarmentDeliveryReturnItem : AggregateRoot<GarmentDeliveryReturnItem, GarmentDeliveryReturnItemReadModel>
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

        public GarmentDeliveryReturnItem(Guid identity, Guid drId, int unitDOItemId, int uenItemId, string preparingItemId, ProductId productId, string productCode, string productName, string designColor, string roNo, double quantity, UomId uomId, string uomUnit) : base(identity)
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

            ReadModel = new GarmentDeliveryReturnItemReadModel(identity)
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
            };
            ReadModel.AddDomainEvent(new OnGarmentDeliveryReturnPlaced(this.Identity));
        }
        public GarmentDeliveryReturnItem(GarmentDeliveryReturnItemReadModel readModel) : base(readModel)
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
        protected override GarmentDeliveryReturnItem GetEntity()
        {
            return this;
        }
    }
}