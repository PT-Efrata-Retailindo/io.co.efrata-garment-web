using System;
using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings
{
    public class GarmentServiceSubconSewing : AggregateRoot<GarmentServiceSubconSewing, GarmentServiceSubconSewingReadModel>
    {
        public string ServiceSubconSewingNo { get; private set; }
        public DateTimeOffset ServiceSubconSewingDate { get; private set; }
        public bool IsUsed { get; internal set; }
        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }
        public int QtyPacking { get; private set; }
        public string UomUnit { get; private set; }
        //
        public GarmentServiceSubconSewing(Guid identity, string serviceSubconSewingNo, DateTimeOffset serviceSubconSewingDate, bool isUsed, BuyerId buyerId, string buyerCode, string buyerName, int qtyPacking, string uomUnit) : base(identity)
        {
            
            Identity = identity;
            ServiceSubconSewingNo = serviceSubconSewingNo;
            ServiceSubconSewingDate = serviceSubconSewingDate;
            IsUsed = isUsed;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;
            QtyPacking = qtyPacking;
            UomUnit = uomUnit;

            ReadModel = new GarmentServiceSubconSewingReadModel(Identity)
            {
                ServiceSubconSewingNo = ServiceSubconSewingNo,
                ServiceSubconSewingDate = ServiceSubconSewingDate,
                //UnitId = UnitId.Value,
                //UnitCode = UnitCode,
                //UnitName = UnitName,
                IsUsed = IsUsed,
                BuyerId = BuyerId.Value,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName,
                QtyPacking = QtyPacking,
                UomUnit = UomUnit
            };

            ReadModel.AddDomainEvent(new OnGarmentServiceSubconSewingPlaced(Identity));

        }

        public GarmentServiceSubconSewing(GarmentServiceSubconSewingReadModel readModel) : base(readModel)
        {
            ServiceSubconSewingNo = readModel.ServiceSubconSewingNo;
            ServiceSubconSewingDate = readModel.ServiceSubconSewingDate;
            //UnitId = new UnitDepartmentId(readModel.UnitId);
            //UnitCode = readModel.UnitCode;
            //UnitName = readModel.UnitName;
            IsUsed = readModel.IsUsed;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
            QtyPacking = readModel.QtyPacking;
            UomUnit = readModel.UomUnit;
        }

        public void SetDate(DateTimeOffset ServiceSubconSewingDate)
        {
            if (this.ServiceSubconSewingDate != ServiceSubconSewingDate)
            {
                this.ServiceSubconSewingDate = ServiceSubconSewingDate;
                ReadModel.ServiceSubconSewingDate = ServiceSubconSewingDate;
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

        public void SetBuyerId(BuyerId SupplierId)
        {
            if (this.BuyerId != BuyerId)
            {
                this.BuyerId = BuyerId;
                ReadModel.BuyerId = BuyerId.Value;
            }
        }
        public void SetBuyerCode(string BuyerCode)
        {
            if (this.BuyerCode != BuyerCode)
            {
                this.BuyerCode = BuyerCode;
                ReadModel.BuyerCode = BuyerCode;
            }
        }
        public void SetBuyerName(string BuyerName)
        {
            if (this.BuyerName != BuyerName)
            {
                this.BuyerName = BuyerName;
                ReadModel.BuyerName = BuyerName;
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
            if(this.UomUnit != uomUnit)
            {
                this.UomUnit = uomUnit;
                ReadModel.UomUnit = uomUnit;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentServiceSubconSewing GetEntity()
        {
            return this;
        }
    }
}
