using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests
{
    public class GarmentSampleRequestProduct : AggregateRoot<GarmentSampleRequestProduct, GarmentSampleRequestProductReadModel>
    {
        public int Index { get; private set; }
        public Guid SampleRequestId { get; private set; }
        public string Style { get; private set; }
        public string Color { get; private set; }

        public string Fabric { get; private set; }

        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }

        public string SizeDescription { get; private set; }
        public double Quantity { get; private set; }

        public GarmentSampleRequestProduct(Guid identity, Guid sampleRequestId, string style, string color, string fabric, SizeId sizeId, string sizeName, string sizeDescription, double quantity, int index) : base(identity)
        {
            Identity = identity;
            SampleRequestId = sampleRequestId;
            Style = style;
            Color = color;
            Fabric = fabric;
            SizeId = sizeId;
            SizeName = sizeName;
            SizeDescription = sizeDescription;
            Quantity = quantity;
            Index = index;

            ReadModel = new GarmentSampleRequestProductReadModel(Identity)
            {
                SampleRequestId=SampleRequestId,
                Style=Style,
                Color=Color,
                Fabric = Fabric,
                SizeId= SizeId.Value,
                SizeName=SizeName,
                SizeDescription=SizeDescription,
                Quantity=Quantity,
                Index=Index
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleRequestPlaced(Identity));
        }

        public GarmentSampleRequestProduct(GarmentSampleRequestProductReadModel readModel) : base(readModel)
        {
            SampleRequestId = readModel.SampleRequestId;
            Style = readModel.Style;
            Color = readModel.Color;
            Fabric = readModel.Fabric;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            SizeDescription = readModel.SizeDescription;
            Quantity = readModel.Quantity;
            Index = readModel.Index;
        }


        protected override GarmentSampleRequestProduct GetEntity()
        {
            return this;
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
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
        public void SetFabric(string Fabric)
        {
            if (this.Fabric != Fabric)
            {
                this.Fabric = Fabric;
                ReadModel.Fabric = Fabric;
            }
        }

        public void SetStyle(string Style)
        {
            if (this.Style != Style)
            {
                this.Style = Style;
                ReadModel.Style = Style;
            }
        }

        public void SetSizeDescription(string SizeDescription)
        {
            if (this.SizeDescription != SizeDescription)
            {
                this.SizeDescription = SizeDescription;
                ReadModel.SizeDescription = SizeDescription;
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

        public void Modify()
        {
            MarkModified();
        }
    }
}
