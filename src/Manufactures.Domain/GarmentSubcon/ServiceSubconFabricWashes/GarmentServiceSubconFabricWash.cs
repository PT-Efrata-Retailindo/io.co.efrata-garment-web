using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes
{
    public class GarmentServiceSubconFabricWash : AggregateRoot<GarmentServiceSubconFabricWash, GarmentServiceSubconFabricWashReadModel>
    {
        public string ServiceSubconFabricWashNo { get; private set; }
        public DateTimeOffset ServiceSubconFabricWashDate { get; private set; }
        public string Remark { get; private set; }
        public bool IsUsed { get; private set; }
        public int QtyPacking { get; private set; }
        public string UomUnit { get; private set; }

        public GarmentServiceSubconFabricWash(Guid identity, string serviceSubconFabricWashNo, DateTimeOffset serviceSubconFabricWashDate, string remark, bool isUsed, int qtyPacking, string uomUnit) : base(identity)
        {
            Identity = identity;
            ServiceSubconFabricWashNo = serviceSubconFabricWashNo;
            ServiceSubconFabricWashDate = serviceSubconFabricWashDate;
            IsUsed = isUsed;
            Remark = remark;
            QtyPacking = qtyPacking;
            UomUnit = uomUnit;

            ReadModel = new GarmentServiceSubconFabricWashReadModel(Identity)
            {
                ServiceSubconFabricWashNo = ServiceSubconFabricWashNo,
                ServiceSubconFabricWashDate = ServiceSubconFabricWashDate,
                Remark = Remark,
                IsUsed = IsUsed,
                QtyPacking = QtyPacking,
                UomUnit = UomUnit
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconFabricWashPlaced(Identity));
        }

        public GarmentServiceSubconFabricWash(GarmentServiceSubconFabricWashReadModel readModel) : base(readModel)
        {
            ServiceSubconFabricWashNo = readModel.ServiceSubconFabricWashNo;
            ServiceSubconFabricWashDate = readModel.ServiceSubconFabricWashDate;
            Remark = readModel.Remark;
            IsUsed = readModel.IsUsed;
            QtyPacking = readModel.QtyPacking;
            UomUnit = readModel.UomUnit;
        }

        public void SetServiceSubconFabricWashDate(DateTimeOffset ServiceSubconFabricWashDate)
        {
            if (this.ServiceSubconFabricWashDate != ServiceSubconFabricWashDate)
            {
                this.ServiceSubconFabricWashDate = ServiceSubconFabricWashDate;
                ReadModel.ServiceSubconFabricWashDate = ServiceSubconFabricWashDate;
            }
        }

        public void SetIsUsed(bool isUsed)
        {
            if (isUsed != IsUsed)
            {
                IsUsed = isUsed;
                ReadModel.IsUsed = isUsed;

                MarkModified();
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

        public void SetQtyPacking(int qtyPacking)
        {
            if (this.QtyPacking != qtyPacking)
            {
                this.QtyPacking = qtyPacking;
                ReadModel.QtyPacking = qtyPacking;
            }
        }

        public void SetUomUnit(string uomUnit)
        {
            if (this.UomUnit != uomUnit)
            {
                this.UomUnit = uomUnit;
                ReadModel.UomUnit = UomUnit;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconFabricWash GetEntity()
        {
            return this;
        }
    }
}
