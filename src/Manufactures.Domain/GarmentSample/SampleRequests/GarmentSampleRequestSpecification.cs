using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSample.SampleRequests
{
    public class GarmentSampleRequestSpecification : AggregateRoot<GarmentSampleRequestSpecification, GarmentSampleRequestSpecificationReadModel>
    {
        public int Index { get; private set; }
        public Guid SampleRequestId { get; private set; }
        public string Inventory { get; private set; }
        public string SpecificationDetail { get; private set; }
        public double Quantity { get; private set; }
        public string Remark { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }

        public GarmentSampleRequestSpecification(Guid identity, Guid sampleRequestId, string inventory, string specificationDetail, double quantity, string remark, UomId uomId, string uomUnit, int index) : base(identity)
        {
            Identity = identity;
            SampleRequestId = sampleRequestId;
            Inventory = inventory;
            SpecificationDetail = specificationDetail;
            Quantity = quantity;
            Remark = remark;
            UomId = uomId;
            UomUnit = uomUnit;
            Index = index;

            ReadModel = new GarmentSampleRequestSpecificationReadModel(Identity)
            {
                SampleRequestId=SampleRequestId,
                Inventory=Inventory,
                SpecificationDetail=SpecificationDetail,
                Quantity=Quantity,
                Remark=Remark,
                UomId= UomId.Value,
                UomUnit= UomUnit,
                Index=Index

            };
            ReadModel.AddDomainEvent(new OnGarmentSampleRequestPlaced(Identity));
        }

        public GarmentSampleRequestSpecification(GarmentSampleRequestSpecificationReadModel readModel) : base(readModel)
        {
            SampleRequestId = readModel.SampleRequestId;
            Inventory = readModel.Inventory;
            SpecificationDetail = readModel.SpecificationDetail;
            Quantity = readModel.Quantity;
            Remark = readModel.Remark;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Index = readModel.Index;
        }

        protected override GarmentSampleRequestSpecification GetEntity()
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

        public void SetRemark(string remark)
        {
            if (this.Remark != remark)
            {
                this.Remark = remark;
                ReadModel.Remark = remark;
            }
        }
        public void SetInventory(string Inventory)
        {
            if (this.Inventory != Inventory)
            {
                this.Inventory = Inventory;
                ReadModel.Inventory = Inventory;
            }
        }
        public void SetSpecificationDetail(string SpecificationDetail)
        {
            if (this.SpecificationDetail != SpecificationDetail)
            {
                this.SpecificationDetail = SpecificationDetail;
                ReadModel.SpecificationDetail = SpecificationDetail;
            }
        }
        public void SetUomId(UomId UomId)
        {
            if (this.UomId != UomId)
            {
                this.UomId = UomId;
                ReadModel.UomId = UomId.Value;
            }
        }
        public void SetUomUnit(string UomUnit)
        {
            if (this.UomUnit != UomUnit)
            {
                this.UomUnit = UomUnit;
                ReadModel.UomUnit = UomUnit;
            }
        }

        public void Modify()
        {
            MarkModified();
        }
    }
}
