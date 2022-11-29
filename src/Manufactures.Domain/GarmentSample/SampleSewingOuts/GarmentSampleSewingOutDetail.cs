using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingOuts
{
    public class GarmentSampleSewingOutDetail : AggregateRoot<GarmentSampleSewingOutDetail, GarmentSampleSewingOutDetailReadModel>
    {
        public Guid SampleSewingOutItemId { get; private set; }
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

        public GarmentSampleSewingOutDetail(Guid identity, Guid sewingOutItemId, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit) : base(identity)
        {
            //MarkTransient();

            SampleSewingOutItemId = sewingOutItemId;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;

            ReadModel = new GarmentSampleSewingOutDetailReadModel(Identity)
            {
                SampleSewingOutItemId = SampleSewingOutItemId,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleSewingOutPlaced(Identity));
        }

        public GarmentSampleSewingOutDetail(GarmentSampleSewingOutDetailReadModel readModel) : base(readModel)
        {
            SampleSewingOutItemId = readModel.SampleSewingOutItemId;
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

        protected override GarmentSampleSewingOutDetail GetEntity()
        {
            return this;
        }
    }
}
