using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts
{
    public class GarmentSubconCutting : AggregateRoot<GarmentSubconCutting, GarmentSubconCuttingReadModel>
    {
        public string RONo { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public double FinishingInQuantity { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public string DesignColor { get; private set; }
        public string Remark { get; private set; }
        public double BasicPrice { get; private set; }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetFinishingInQuantity(double FinishingInQuantity)
        {
            if (this.FinishingInQuantity != FinishingInQuantity)
            {
                this.FinishingInQuantity = FinishingInQuantity;
                ReadModel.FinishingInQuantity = FinishingInQuantity;
            }
        }

        public GarmentSubconCutting(Guid identity, string roNo, SizeId sizeId, string sizeName, double quantity, ProductId productId, string productCode,string productName, GarmentComodityId comodityId, string comodityCode, string comodityName, string designColor, string remark,double basicPrice) : base(identity)
        {
            RONo = roNo;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            ComodityId = comodityId;
            ComodityName = comodityName;
            ComodityCode = comodityCode;
            DesignColor = designColor;
            Remark = remark;
            BasicPrice = basicPrice;

            ReadModel = new GarmentSubconCuttingReadModel(Identity)
            {
                RONo=RONo,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                ProductId=ProductId.Value,
                ProductCode=ProductCode,
                ProductName=ProductName,
                ComodityId=ComodityId.Value,
                ComodityName=ComodityName,
                ComodityCode=ComodityCode,
                DesignColor=DesignColor,
                Remark=Remark,
                BasicPrice=BasicPrice
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCuttingPlaced(Identity));
        }

        public GarmentSubconCutting(GarmentSubconCuttingReadModel readModel) : base(readModel)
        {
            RONo = readModel.RONo;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            FinishingInQuantity = readModel.FinishingInQuantity;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityName = readModel.ComodityName;
            ComodityCode = readModel.ComodityCode;
            DesignColor = readModel.DesignColor;
            Remark = readModel.Remark;
            BasicPrice = readModel.BasicPrice;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCutting GetEntity()
        {
            return this;
        }
    }
}
