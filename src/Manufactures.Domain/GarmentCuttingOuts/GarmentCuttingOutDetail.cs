using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts
{
    public class GarmentCuttingOutDetail : AggregateRoot<GarmentCuttingOutDetail, GarmentCuttingOutDetailReadModel>
    {
        public Guid CutOutItemId { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double CuttingOutQuantity { get; private set; }
        public UomId CuttingOutUomId { get; private set; }
        public string CuttingOutUomUnit { get; private set; }
        public string Color { get; private set; }
        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }
		public string UId { get; private set; }

		public void SetCuttingOutQuantity(double CuttingOutQuantity)
        {
            if (this.CuttingOutQuantity != CuttingOutQuantity)
            {
                this.CuttingOutQuantity = CuttingOutQuantity;
                ReadModel.CuttingOutQuantity = CuttingOutQuantity;
            }
        }

        public void SetRemainingQuantity(double RemainingQuantity)
        {
            if (this.RemainingQuantity != RemainingQuantity)
            {
                this.RemainingQuantity = RemainingQuantity;
                ReadModel.RemainingQuantity = RemainingQuantity;
            }
        }

        public void SetColor(string Color)
        {
            if (this.Color != Color)
            {
                this.Color = Color;
                ReadModel.Color = Color;
            }
        }

        public void SetSizeId(SizeId SizeId)
        {
            if (this.SizeId != SizeId)
            {
                this.SizeId = SizeId;
                ReadModel.SizeId = SizeId.Value;
            }
        }

        public void SetSizeName(string SizeName)
        {
            if (this.SizeName != SizeName)
            {
                this.SizeName = SizeName;
                ReadModel.SizeName = SizeName;
            }
        }

        public void SetPrice(double Price)
        {
            if (this.Price != Price)
            {
                this.Price = Price;
                ReadModel.Price = Price;
            }
        }

        public GarmentCuttingOutDetail(Guid identity, Guid cutOutItemId, SizeId sizeId, string sizeName, string color, double remainingQuantity, double cuttingOutQuantity, UomId cuttingOutUomId, string cuttingOutUomUnit, double basicPrice, double price) : base(identity)
        {
            //MarkTransient();

            CutOutItemId = cutOutItemId;
            Color = color;
            SizeId = sizeId;
            SizeName = sizeName;
            RemainingQuantity = remainingQuantity;
            CuttingOutQuantity = cuttingOutQuantity;
            CuttingOutUomId = cuttingOutUomId;
            CuttingOutUomUnit = cuttingOutUomUnit;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentCuttingOutDetailReadModel(Identity)
            {
                CutOutItemId = CutOutItemId,
                Color = Color,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                RemainingQuantity = RemainingQuantity,
                CuttingOutQuantity = CuttingOutQuantity,
                CuttingOutUomId = CuttingOutUomId.Value,
                CuttingOutUomUnit = CuttingOutUomUnit,
                BasicPrice = BasicPrice,
                Price = Price
            };

            ReadModel.AddDomainEvent(new OnGarmentCuttingOutPlaced(Identity));
        }

        public GarmentCuttingOutDetail(GarmentCuttingOutDetailReadModel readModel) : base(readModel)
        {
            CutOutItemId = readModel.CutOutItemId;
            Color = readModel.Color;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            RemainingQuantity = readModel.RemainingQuantity;
            CuttingOutQuantity = readModel.CuttingOutQuantity;
            CuttingOutUomId = new UomId(readModel.CuttingOutUomId);
            CuttingOutUomUnit = readModel.CuttingOutUomUnit;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentCuttingOutDetail GetEntity()
        {
            return this;
        }
    }
}
