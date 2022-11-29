using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts
{
    public class GarmentSampleFinishingOutDetail : AggregateRoot<GarmentSampleFinishingOutDetail, GarmentSampleFinishingOutDetailReadModel>
    {
        public Guid FinishingOutItemId { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
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

        public GarmentSampleFinishingOutDetail(Guid identity, Guid sewingOutItemId, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit) : base(identity)
        {
            //MarkTransient();

            FinishingOutItemId = sewingOutItemId;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;

            ReadModel = new GarmentSampleFinishingOutDetailReadModel(Identity)
            {
                FinishingOutItemId = FinishingOutItemId,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleFinishingOutPlaced(Identity));
        }

        public GarmentSampleFinishingOutDetail(GarmentSampleFinishingOutDetailReadModel readModel) : base(readModel)
        {
            FinishingOutItemId = readModel.FinishingOutItemId;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSampleFinishingOutDetail GetEntity()
        {
            return this;
        }
    }
}