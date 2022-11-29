using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.Events.GarmentSubcon;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels
{
    public class GarmentServiceSubconShrinkagePanelDetail : AggregateRoot<GarmentServiceSubconShrinkagePanelDetail, GarmentServiceSubconShrinkagePanelDetailReadModel>
    {
        public Guid ServiceSubconShrinkagePanelItemId { get; private set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string ProductRemark { get; private set; }

        public string DesignColor { get; private set; }
        public decimal Quantity { get; private set; }

        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }

        public GarmentServiceSubconShrinkagePanelDetail(Guid identity, Guid serviceSubconShrinkagePanelItemId, ProductId productId, string productCode, string productName, string productRemark, string designColor, decimal quantity, UomId uomId, string uomUnit) : base(identity)
        {
            ServiceSubconShrinkagePanelItemId = serviceSubconShrinkagePanelItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ProductRemark = productRemark;
            DesignColor = designColor;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;

            ReadModel = new GarmentServiceSubconShrinkagePanelDetailReadModel(identity)
            {
                ServiceSubconShrinkagePanelItemId = ServiceSubconShrinkagePanelItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                ProductRemark = ProductRemark,
                DesignColor = DesignColor,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
            };
            ReadModel.AddDomainEvent(new OnGarmentServiceSubconShrinkagePanelPlaced(Identity));
        }

        public GarmentServiceSubconShrinkagePanelDetail(GarmentServiceSubconShrinkagePanelDetailReadModel readModel) : base(readModel)
        {
            ServiceSubconShrinkagePanelItemId = readModel.ServiceSubconShrinkagePanelItemId;
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

        protected override GarmentServiceSubconShrinkagePanelDetail GetEntity()
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
