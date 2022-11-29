using Infrastructure.Domain;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events.GarmentSample;

namespace Manufactures.Domain.GarmentSample.SampleCuttingOuts
{
    public class GarmentSampleCuttingOutItem : AggregateRoot<GarmentSampleCuttingOutItem, GarmentSampleCuttingOutItemReadModel>
    {
        public Guid CuttingOutId { get; private set; }
        public Guid CuttingInId { get; private set; }
        public Guid CuttingInDetailId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public double TotalCuttingOut { get; private set; }
        public double TotalCuttingOutQuantity { get; private set; }
        public string UId { get; private set; }
        public GarmentSampleCuttingOutItem(Guid identity, Guid cuttingInId, Guid cuttingInDetailId, Guid cutOutId, ProductId productId, string productCode, string productName, string designColor, double totalCuttingOut) : base(identity)
        {
            //MarkTransient();

            Identity = identity;
            CuttingInId = cuttingInId;
            CuttingInDetailId = cuttingInDetailId;
            CuttingOutId = cutOutId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            TotalCuttingOut = totalCuttingOut;

            ReadModel = new GarmentSampleCuttingOutItemReadModel(identity)
            {
                CuttingInId = CuttingInId,
                CuttingOutId = CuttingOutId,
                CuttingInDetailId = CuttingInDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                TotalCuttingOut = TotalCuttingOut,
                GarmentSampleCuttingOutDetail = new List<GarmentSampleCuttingOutDetailReadModel>()
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleCuttingOutPlaced(Identity));
        }

        public GarmentSampleCuttingOutItem(GarmentSampleCuttingOutItemReadModel readModel) : base(readModel)
        {
            CuttingInId = readModel.CuttingInId;
            CuttingOutId = readModel.CuttingOutId;
            CuttingInDetailId = readModel.CuttingInDetailId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            TotalCuttingOut = readModel.TotalCuttingOut;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSampleCuttingOutItem GetEntity()
        {
            return this;
        }
    }
}
