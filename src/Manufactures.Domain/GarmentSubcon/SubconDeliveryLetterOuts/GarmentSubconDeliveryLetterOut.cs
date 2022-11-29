using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts
{
    public class GarmentSubconDeliveryLetterOut : AggregateRoot<GarmentSubconDeliveryLetterOut, GarmentSubconDeliveryLetterOutReadModel>
    {

        public string DLNo { get; private set; }
        public string DLType { get; private set; }
        public string ContractType { get; private set; }
        public DateTimeOffset DLDate { get; private set; }

        public int UENId { get; private set; }
        public string UENNo { get; private set; }

        public string PONo { get; private set; }
        public int EPOItemId { get; private set; }

        public string Remark { get; private set; }

        public bool IsUsed { get; private set; }
        public string ServiceType { get; private set; }
        public string SubconCategory { get; private set; }
        public int EPOId { get; private set; }
        public string EPONo { get; private set; }
        public int QtyPacking { get; private set; }
        public string UomUnit { get; private set; }

        public GarmentSubconDeliveryLetterOut(Guid identity, string dLNo, string dLType, string contractType, DateTimeOffset dLDate, int uENId, string uENNo, string pONo, int ePOItemId, string remark, bool isUsed, string serviceType, string subconCategory, int epoId, string epoNo, int qtyPacking, string uomUnit) : base(identity)
        {
            Identity = identity;
            DLNo = dLNo;
            DLType = dLType;
            ContractType = contractType;
            DLDate = dLDate;
            UENId = uENId;
            UENNo = uENNo;
            PONo = pONo;
            EPOItemId = ePOItemId;
            Remark = remark;
            IsUsed = isUsed;
            ServiceType = serviceType;
            SubconCategory = subconCategory;
            EPOId = epoId;
            EPONo = epoNo;
            QtyPacking = qtyPacking;
            UomUnit = uomUnit;
            ReadModel = new GarmentSubconDeliveryLetterOutReadModel(Identity)
            {
                DLDate=DLDate,
                DLNo= DLNo,
                DLType=DLType,
                ContractType=ContractType,
                UENId=UENId,
                UENNo=UENNo,
                PONo=PONo,
                EPOItemId=EPOItemId,
                Remark=Remark,
                IsUsed = isUsed,
                ServiceType=serviceType,
                SubconCategory=subconCategory,
                EPOId = epoId,
                EPONo = epoNo,
                QtyPacking = qtyPacking,
                UomUnit = uomUnit
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconDeliveryLetterOutPlaced(Identity));
        }

        public GarmentSubconDeliveryLetterOut(GarmentSubconDeliveryLetterOutReadModel readModel) : base(readModel)
        {
            DLDate = readModel.DLDate;
            Remark = readModel.Remark;
            EPOItemId = readModel.EPOItemId;
            PONo = readModel.PONo;
            UENNo = readModel.UENNo;
            UENId = readModel.UENId;
            ContractType = readModel.ContractType;
            DLType = readModel.DLType;
            DLNo = readModel.DLNo;
            IsUsed = readModel.IsUsed;
            ServiceType = readModel.ServiceType;
            SubconCategory = readModel.SubconCategory;
            EPOId = readModel.EPOId;
            EPONo = readModel.EPONo;
            QtyPacking = readModel.QtyPacking;
            UomUnit = readModel.UomUnit;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconDeliveryLetterOut GetEntity()
        {
            return this;
        }

        public void SetDate(DateTimeOffset DLDate)
        {
            if (this.DLDate != DLDate)
            {
                this.DLDate = DLDate;
                ReadModel.DLDate = DLDate;
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

        //public void SetPONo(string poNo)
        //{
        //    if(this.PONo != poNo)
        //    {
        //        this.PONo = poNo;
        //        ReadModel.PONo = poNo;

        //        MarkModified();
        //    }
        //}

        //public void SetEPOItemId(int ePOItemId)
        //{
        //    if(this.EPOItemId != ePOItemId)
        //    {
        //        this.EPOItemId = ePOItemId;
        //        ReadModel.EPOItemId = ePOItemId;

        //        MarkModified();
        //    }
        //}

        public void SetRemark(string remark)
        {
            if(this.Remark != remark)
            {
                this.Remark = remark;
                ReadModel.Remark = remark;

                MarkModified();
            }
        }

        public void SetQtyPacking(int qtyPacking)
        {
            if (this.QtyPacking != qtyPacking)
            {
                this.QtyPacking = qtyPacking;
                ReadModel.QtyPacking = qtyPacking;

                MarkModified();
            }
        }

        public void SetUomUnit(string uomUnit)
        {
            if (this.UomUnit != uomUnit)
            {
                this.UomUnit = uomUnit;
                ReadModel.UomUnit = uomUnit;

                MarkModified();
            }
        }
    }
}
