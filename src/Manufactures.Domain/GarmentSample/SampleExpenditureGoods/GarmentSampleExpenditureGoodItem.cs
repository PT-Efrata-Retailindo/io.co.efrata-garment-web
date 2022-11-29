using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleExpenditureGoods
{
    public class GarmentSampleExpenditureGoodItem : AggregateRoot<GarmentSampleExpenditureGoodItem, GarmentSampleExpenditureGoodItemReadModel>
    {
        public Guid ExpenditureGoodId { get; private set; }
        public Guid FinishedGoodStockId { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public double ReturQuantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Description { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }

        public GarmentSampleExpenditureGoodItem(Guid identity, Guid expenditureGoodId, Guid finishedGoodStockId, SizeId sizeId, string sizeName, double quantity, double returQuantity, UomId uomId, string uomUnit, string description, double basicPrice, double price) : base(identity)
        {
            ExpenditureGoodId = expenditureGoodId;
            FinishedGoodStockId = finishedGoodStockId;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            ReturQuantity = returQuantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Description = description;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentSampleExpenditureGoodItemReadModel(Identity)
            {
                ExpenditureGoodId = ExpenditureGoodId,
                FinishedGoodStockId = FinishedGoodStockId,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                ReturQuantity = ReturQuantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Description = Description,
                BasicPrice = BasicPrice,
                Price = Price
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleExpenditureGoodPlaced(Identity));
        }

        public GarmentSampleExpenditureGoodItem(GarmentSampleExpenditureGoodItemReadModel readModel) : base(readModel)
        {
            ExpenditureGoodId = readModel.ExpenditureGoodId;
            FinishedGoodStockId = readModel.FinishedGoodStockId;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            ReturQuantity = readModel.ReturQuantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Description = readModel.Description;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void SetReturQuantity(double ReturQuantity)
        {
            if (this.ReturQuantity != ReturQuantity)
            {
                this.ReturQuantity = ReturQuantity;
                ReadModel.ReturQuantity = ReturQuantity;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSampleExpenditureGoodItem GetEntity()
        {
            return this;
        }
    }
}
