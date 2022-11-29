using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns
{
    public class GarmentCuttingInDetail : AggregateRoot<GarmentCuttingInDetail, GarmentCuttingInDetailReadModel>
    {
        public Guid CutInItemId { get; private set; }
        public Guid PreparingItemId { get; private set; }

        public Guid SewingOutItemId { get; internal set; }
        public Guid SewingOutDetailId { get; internal set; }

        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }

        public string DesignColor { get; private set; }
        public string FabricType { get; private set; }

        public double PreparingQuantity { get; private set; }
        public UomId PreparingUomId { get; private set; }
        public string PreparingUomUnit { get; private set; }

        public int CuttingInQuantity { get; private set; }
        public UomId CuttingInUomId { get; private set; }
        public string CuttingInUomUnit { get; private set; }

        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }
        public double FC { get; private set; }
        public string Color { get; private set; }
		
		public void SetCuttingInQuantity(int CuttingInQuantity)
        {
            if (this.CuttingInQuantity != CuttingInQuantity)
            {
                this.CuttingInQuantity = CuttingInQuantity;
                ReadModel.CuttingInQuantity = CuttingInQuantity;
            }
        }

        public void SetPreparingQuantity(double PreparingQuantity)
        {
            if (this.PreparingQuantity != PreparingQuantity)
            {
                this.PreparingQuantity = PreparingQuantity;
                ReadModel.PreparingQuantity = PreparingQuantity;
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

        public void SetPrice(double Price)
        {
            if (this.Price != Price)
            {
                this.Price = Price;
                ReadModel.Price = Price;
            }
        }

        public void SetFC(double FC)
        {
            if (this.FC != FC)
            {
                this.FC = FC;
                ReadModel.FC = FC;
            }
        }

        public GarmentCuttingInDetail(Guid identity, Guid cutInItemId, Guid preparingItemId, Guid sewingOutItemId, Guid sewingOutDetailId, ProductId productId, string productCode, string productName, string designColor, string fabricType, double preparingQuantity, UomId preparingUomId, string preparingUomUnit, int cuttingInQuantity, UomId cuttingInUomId, string cuttingInUomUnit, double remainingQuantity, double basicPrice, double price,double fc, string color) : base(identity)
        {
            //MarkTransient();

            CutInItemId = cutInItemId;
            PreparingItemId = preparingItemId;
            SewingOutItemId = sewingOutItemId;
            SewingOutDetailId = sewingOutDetailId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            FabricType = fabricType;
            PreparingQuantity = preparingQuantity;
            PreparingUomId = preparingUomId;
            PreparingUomUnit = preparingUomUnit;
            CuttingInQuantity = cuttingInQuantity;
            CuttingInUomId = cuttingInUomId;
            CuttingInUomUnit = cuttingInUomUnit;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            Price = price;
            FC = fc;
            Color = color;

            ReadModel = new GarmentCuttingInDetailReadModel(Identity)
            {
                CutInItemId = CutInItemId,
                PreparingItemId = PreparingItemId,
                SewingOutItemId = SewingOutItemId,
                SewingOutDetailId = SewingOutDetailId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                FabricType = FabricType,
                PreparingQuantity = PreparingQuantity,
                PreparingUomId = PreparingUomId.Value,
                PreparingUomUnit = PreparingUomUnit,
                CuttingInQuantity = CuttingInQuantity,
                CuttingInUomId = CuttingInUomId.Value,
                CuttingInUomUnit = CuttingInUomUnit,
                RemainingQuantity = RemainingQuantity,
                BasicPrice = BasicPrice,
                Price=Price,
                FC=FC,
                Color= Color
            };

            ReadModel.AddDomainEvent(new OnGarmentCuttingInPlaced(Identity));
        }

        public GarmentCuttingInDetail(GarmentCuttingInDetailReadModel readModel) : base(readModel)
        {
            CutInItemId = readModel.CutInItemId;
            PreparingItemId = readModel.PreparingItemId;
            SewingOutItemId = readModel.SewingOutItemId;
            SewingOutDetailId = readModel.SewingOutDetailId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            FabricType = readModel.FabricType;
            PreparingQuantity = readModel.PreparingQuantity;
            PreparingUomId = new UomId(readModel.PreparingUomId);
            PreparingUomUnit = readModel.PreparingUomUnit;
            CuttingInQuantity = readModel.CuttingInQuantity;
            CuttingInUomId = new UomId(readModel.CuttingInUomId);
            CuttingInUomUnit = readModel.CuttingInUomUnit;
            RemainingQuantity = readModel.RemainingQuantity;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
            FC = readModel.FC;
            Color = readModel.Color;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentCuttingInDetail GetEntity()
        {
            return this;
        }
    }
}
