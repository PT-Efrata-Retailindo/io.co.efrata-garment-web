using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings
{
    public class GarmentServiceSubconCuttingSize : AggregateRoot<GarmentServiceSubconCuttingSize, GarmentServiceSubconCuttingSizeReadModel>
    {

        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Color { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public Guid ServiceSubconCuttingDetailId { get; private set; }
        public Guid CuttingInDetailId { get; private set; }
        public Guid CuttingInId { get; private set; }
        public GarmentServiceSubconCuttingSize(Guid identity,SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, Guid serviceSubconCuttingDetailId, Guid cuttingId, Guid cuttingInDetailId, ProductId productId, string productCode, string productName) : base(identity)
        {
            Identity = identity;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            CuttingInDetailId = cuttingInDetailId;
            ServiceSubconCuttingDetailId = serviceSubconCuttingDetailId;
            CuttingInId = cuttingId;
            ReadModel = new GarmentServiceSubconCuttingSizeReadModel(Identity)
            {
                ServiceSubconCuttingDetailId = ServiceSubconCuttingDetailId,
                Color = Color,
                CuttingInDetailId = CuttingInDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                CuttingInId = CuttingInId
            };

            ReadModel.AddDomainEvent(new OnServiceSubconCuttingPlaced(Identity));
        }

        public GarmentServiceSubconCuttingSize(GarmentServiceSubconCuttingSizeReadModel readModel) : base(readModel)
        {
            ServiceSubconCuttingDetailId = readModel.ServiceSubconCuttingDetailId;
            Color = readModel.Color;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            CuttingInDetailId = readModel.CuttingInDetailId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            CuttingInId = readModel.CuttingInId;
        }

        public void SetQuantity(double qty)
        {
            if (Quantity != qty)
            {
                Quantity = qty;
                ReadModel.Quantity = qty;

                MarkModified();
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconCuttingSize GetEntity()
        {
            return this;
        }
    }
}
