using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns
{
    public class GarmentSubconCustomsIn : AggregateRoot<GarmentSubconCustomsIn, GarmentSubconCustomsInReadModel>
    {
        public string BcNo { get; private set; }
        public DateTimeOffset BcDate { get; private set; }
        public string BcType { get; private set; }
        public string SubconType { get; private set; }
        public Guid SubconContractId { get; private set; }
        public string SubconContractNo { get; private set; }
        public SupplierId SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public string Remark { get; set; }
        public bool IsUsed { get; internal set; }
        public string SubconCategory { get; private set; }

        public GarmentSubconCustomsIn(Guid identity, string bcNo, DateTimeOffset bcDate, string bcType, string subconType, Guid subconContractId, string subconContractNo, SupplierId supplierId, string supplierCode, string supplierName, string remark, bool isUsed, string subconCategory) : base(identity)
        {
            Identity = identity;
            BcNo = bcNo;
            BcType = bcType;
            BcDate = bcDate;
            SubconType = subconType;
            SubconContractId = subconContractId;
            SubconContractNo = subconContractNo;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            Remark = remark;
            IsUsed = isUsed;
            SubconCategory = subconCategory;

            ReadModel = new GarmentSubconCustomsInReadModel(Identity)
            {
                BcNo = BcNo,
                BcDate = BcDate,
                BcType = BcType,
                SubconType = SubconType,
                SubconContractId = SubconContractId,
                SubconContractNo = SubconContractNo,
                SupplierId = SupplierId.Value,
                SupplierCode = SupplierCode,
                SupplierName = SupplierName,
                Remark = Remark,
                IsUsed = IsUsed,
                SubconCategory = SubconCategory,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCustomsInPlaced(Identity));
        }

        public GarmentSubconCustomsIn(GarmentSubconCustomsInReadModel readModel) : base(readModel)
        {
            BcNo = readModel.BcNo;
            BcDate = readModel.BcDate;
            BcType = readModel.BcType;
            SubconType = readModel.SubconType;
            SubconContractId = readModel.SubconContractId;
            SubconContractNo = readModel.SubconContractNo;
            SupplierId = new SupplierId(readModel.SupplierId);
            SupplierCode = readModel.SupplierCode;
            SupplierName = readModel.SupplierName;
            Remark = readModel.Remark;
            IsUsed = readModel.IsUsed;
            SubconCategory = readModel.SubconCategory;
        }

        public void SetBcDate(DateTimeOffset bcDate)
        {
            if (BcDate != bcDate)
            {
                BcDate = bcDate;
                ReadModel.BcDate = bcDate;

                MarkModified();
            }
        }

        public void SetBcNo(string bcNo)
        {
            if(BcNo != bcNo)
            {
                BcNo = bcNo;
                ReadModel.BcNo = bcNo;

                MarkModified();
            }
        }

        public void SetBcType(string bcType)
        {
            if (BcType != bcType)
            {
                BcType = bcType;
                ReadModel.BcType = bcType;

                MarkModified();
            }
        }

        public void SetRemark(string remark)
        {
            if (Remark != remark)
            {
                Remark = remark;
                ReadModel.Remark = remark;

                MarkModified();
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

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCustomsIn GetEntity()
        {
            return this;
        }
    }
}
