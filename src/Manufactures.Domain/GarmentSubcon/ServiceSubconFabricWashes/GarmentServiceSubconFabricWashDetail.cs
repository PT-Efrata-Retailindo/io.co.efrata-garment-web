using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.Events.GarmentSubcon;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes
{
    public class GarmentServiceSubconFabricWashDetail : AggregateRoot<GarmentServiceSubconFabricWashDetail, GarmentServiceSubconFabricWashDetailReadModel>
    {
        public Guid ServiceSubconFabricWashItemId { get; private set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string ProductRemark { get; private set; }

        public string DesignColor { get; private set; }
        public decimal Quantity { get; private set; }

        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }

        public GarmentServiceSubconFabricWashDetail(Guid identity, Guid serviceSubconFabricWashItemId, ProductId productId, string productCode, string productName, string productRemark, string designColor, decimal quantity, UomId uomId, string uomUnit) : base(identity)
        {
            ServiceSubconFabricWashItemId = serviceSubconFabricWashItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ProductRemark = productRemark;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;

            ReadModel = new GarmentServiceSubconFabricWashDetailReadModel(identity)
            {
                ServiceSubconFabricWashItemId = ServiceSubconFabricWashItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                ProductRemark = ProductRemark,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
            };
            ReadModel.AddDomainEvent(new OnGarmentServiceSubconFabricWashPlaced(Identity));
        }

        public GarmentServiceSubconFabricWashDetail(GarmentServiceSubconFabricWashDetailReadModel readModel) : base(readModel)
        {
            ServiceSubconFabricWashItemId = readModel.ServiceSubconFabricWashItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            ProductRemark = readModel.ProductRemark;
            DesignColor = readModel.DesignColor;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconFabricWashDetail GetEntity()
        {
            return this;
        }

        public void SetQuantity(decimal Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
    }
}
