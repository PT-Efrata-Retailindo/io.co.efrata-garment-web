using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts
{
    public class GarmentSubconCustomsOut : AggregateRoot<GarmentSubconCustomsOut, GarmentSubconCustomsOutReadModel>
    {
        
        public string CustomsOutNo { get; private set; }
        public DateTimeOffset CustomsOutDate { get; private set; }
        public string CustomsOutType { get; private set; }
        public string SubconType { get; private set; }
        public Guid SubconContractId { get; private set; }
        public string SubconContractNo { get; private set; }
        public SupplierId SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public string Remark { get; private set; }
        public string SubconCategory { get; private set; }

        public GarmentSubconCustomsOut(Guid identity, string customsOutNo, DateTimeOffset customsOutDate, string customsOutType, string subconType, Guid subconContractId, string subconContractNo, SupplierId supplierId, string supplierCode, string supplierName, string remark, string subconCategory) : base(identity)
        {
            Identity = identity;
            CustomsOutNo = customsOutNo;
            CustomsOutDate = customsOutDate;
            CustomsOutType = customsOutType;
            SubconType = subconType;
            SubconContractId = subconContractId;
            SubconContractNo = subconContractNo;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            Remark = remark;
            SubconCategory = subconCategory;

            ReadModel = new GarmentSubconCustomsOutReadModel(Identity)
            {
                CustomsOutNo = CustomsOutNo,
                CustomsOutDate = CustomsOutDate,
                CustomsOutType = CustomsOutType,
                SubconContractId = SubconContractId,
                SubconType = SubconType,
                SubconContractNo = SubconContractNo,
                SupplierId = SupplierId.Value,
                SupplierCode = SupplierCode,
                SupplierName = SupplierName,
                Remark = Remark,
                SubconCategory = SubconCategory,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCustomsOutPlaced(Identity));
        }

        public GarmentSubconCustomsOut(GarmentSubconCustomsOutReadModel readModel) : base(readModel)
        {
            CustomsOutNo = readModel.CustomsOutNo;
            CustomsOutDate = readModel.CustomsOutDate;
            CustomsOutType = readModel.CustomsOutType;
            SubconType = readModel.SubconType;
            SubconContractNo = readModel.SubconContractNo;
            SubconContractId = readModel.SubconContractId;
            SupplierId = new SupplierId(readModel.SupplierId);
            SupplierName = readModel.SupplierName;
            SupplierCode = readModel.SupplierCode;
            Remark = readModel.Remark;
            SubconCategory = readModel.SubconCategory;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCustomsOut GetEntity()
        {
            return this;
        }

        public void SetDate(DateTimeOffset CustomsOutDate)
        {
            if (this.CustomsOutDate != CustomsOutDate)
            {
                this.CustomsOutDate = CustomsOutDate;
                ReadModel.CustomsOutDate = CustomsOutDate;
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
        public void SetCustomsOutNo(string CustomsOutNo)
        {
            if (this.CustomsOutNo != CustomsOutNo)
            {
                this.CustomsOutNo = CustomsOutNo;
                ReadModel.CustomsOutNo = CustomsOutNo;
            }
        }
        public void SetCustomsOutType(string CustomsOutType)
        {
            if (this.CustomsOutType != CustomsOutType)
            {
                this.CustomsOutType = CustomsOutType;
                ReadModel.Remark = CustomsOutType;
            }
        }
        public void SetSubconCategory(string subconCategory)
        {
            if (this.SubconCategory != subconCategory)
            {
                this.SubconCategory = subconCategory;
                ReadModel.SubconCategory = subconCategory;
            }
        }
    }
}
