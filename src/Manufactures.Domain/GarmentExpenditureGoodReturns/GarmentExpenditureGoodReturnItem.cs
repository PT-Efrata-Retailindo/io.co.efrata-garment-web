using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoodReturns
{
    public class GarmentExpenditureGoodReturnItem : AggregateRoot<GarmentExpenditureGoodReturnItem, GarmentExpenditureGoodReturnItemReadModel>
    {
        public Guid ReturId { get; private set; }
        public Guid ExpenditureGoodId { get; private set; }
        public Guid ExpenditureGoodItemId { get; private set; }
        public Guid FinishedGoodStockId { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Description { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }

        public GarmentExpenditureGoodReturnItem(Guid identity, Guid returId, Guid expenditureGoodId, Guid expenditureGoodItemId, Guid finishedGoodStockId, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string description, double basicPrice, double price) : base(identity)
        {
            ReturId = returId;
            ExpenditureGoodId = expenditureGoodId;
            ExpenditureGoodItemId = expenditureGoodItemId;
            FinishedGoodStockId = finishedGoodStockId;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Description = description;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentExpenditureGoodReturnItemReadModel(Identity)
            {
                ReturId= ReturId,
                ExpenditureGoodId = ExpenditureGoodId,
                ExpenditureGoodItemId= ExpenditureGoodItemId,
                FinishedGoodStockId = FinishedGoodStockId,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Description = Description,
                BasicPrice = BasicPrice,
                Price = Price
            };

            ReadModel.AddDomainEvent(new OnGarmentExpenditureGoodReturnPlaced(Identity));
        }

        public GarmentExpenditureGoodReturnItem(GarmentExpenditureGoodReturnItemReadModel readModel) : base(readModel)
        {
            ReturId = readModel.ReturId;
            ExpenditureGoodId = readModel.ExpenditureGoodId;
            ExpenditureGoodItemId = readModel.ExpenditureGoodItemId;
            FinishedGoodStockId = readModel.FinishedGoodStockId;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Description = readModel.Description;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentExpenditureGoodReturnItem GetEntity()
        {
            return this;
        }
    }
}
