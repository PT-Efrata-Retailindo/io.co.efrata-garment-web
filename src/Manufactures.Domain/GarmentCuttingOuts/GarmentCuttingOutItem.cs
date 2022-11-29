using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentCuttingOuts
{
    public class GarmentCuttingOutItem : AggregateRoot<GarmentCuttingOutItem, GarmentCuttingOutItemReadModel>
    {
        public Guid CutOutId { get; private set; }
        public Guid CuttingInId { get; private set; }
        public Guid CuttingInDetailId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public double TotalCuttingOut { get; private set; }
        public double TotalCuttingOutQuantity { get; private set; }
		public string UId { get; private set; }
		public GarmentCuttingOutItem(Guid identity, Guid cuttingInId, Guid cuttingInDetailId, Guid cutOutId, ProductId productId, string productCode, string productName, string designColor, double totalCuttingOut) : base(identity)
        {
            //MarkTransient();

            Identity = identity;
            CuttingInId = cuttingInId;
            CuttingInDetailId = cuttingInDetailId;
            CutOutId = cutOutId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            TotalCuttingOut = totalCuttingOut;

            ReadModel = new GarmentCuttingOutItemReadModel(identity)
            {
                CuttingInId = CuttingInId,
                CutOutId = CutOutId,
                CuttingInDetailId = CuttingInDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                TotalCuttingOut = TotalCuttingOut,
                GarmentCuttingOutDetail = new List<GarmentCuttingOutDetailReadModel>()
            };

            ReadModel.AddDomainEvent(new OnGarmentCuttingOutPlaced(Identity));
        }

        public GarmentCuttingOutItem(GarmentCuttingOutItemReadModel readModel) : base(readModel)
        {
            CuttingInId = readModel.CuttingInId;
            CutOutId = readModel.CutOutId;
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

        protected override GarmentCuttingOutItem GetEntity()
        {
            return this;
        }
    }
}
