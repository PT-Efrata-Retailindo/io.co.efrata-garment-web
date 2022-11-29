using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns
{
    public class GarmentSubconCustomsInItem : AggregateRoot<GarmentSubconCustomsInItem, GarmentSubconCustomsInItemReadModel>
    {
        public Guid SubconCustomsInId { get; private set; }
        public SupplierId SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public int DoId { get; private set; }
        public string DoNo { get; private set; }
        public decimal Quantity { get; private set; }

        public GarmentSubconCustomsInItem(Guid identity, Guid subconCustomsInId, SupplierId supplierId, string supplierCode, string supplierName, int doId, string doNo, decimal quantity) : base(identity)
        {
            Identity = identity;
            SubconCustomsInId = subconCustomsInId;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            DoId = doId;
            DoNo = doNo;
            Quantity = quantity;

            ReadModel = new GarmentSubconCustomsInItemReadModel(Identity)
            {
                SubconCustomsInId = SubconCustomsInId,
                SupplierId = SupplierId.Value,
                SupplierCode = SupplierCode,
                SupplierName = SupplierName,
                DoId = DoId,
                DoNo = DoNo,
                Quantity = Quantity,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCustomsInPlaced(Identity));
        }

        public GarmentSubconCustomsInItem(GarmentSubconCustomsInItemReadModel readModel) : base(readModel)
        {
            SubconCustomsInId = readModel.SubconCustomsInId;
            SupplierId = new SupplierId(readModel.SupplierId);
            SupplierCode = readModel.SupplierCode;
            SupplierName = readModel.SupplierName;
            DoId = readModel.DoId;
            DoNo = readModel.DoNo;
            Quantity = readModel.Quantity;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCustomsInItem GetEntity()
        {
            return this;
        }
    }
}
