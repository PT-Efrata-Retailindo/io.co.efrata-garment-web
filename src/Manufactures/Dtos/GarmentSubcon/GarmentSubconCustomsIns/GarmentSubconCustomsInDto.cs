using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentSubconCustomsIns
{
    public class GarmentSubconCustomsInDto : BaseDto
    {
        public GarmentSubconCustomsInDto(GarmentSubconCustomsIn garmentSubconCustomsInList)
        {
            Id = garmentSubconCustomsInList.Identity;
            BcNo = garmentSubconCustomsInList.BcNo;
            BcType = garmentSubconCustomsInList.BcType;
            BcDate = garmentSubconCustomsInList.BcDate;
            SubconType = garmentSubconCustomsInList.SubconType;
            SubconContractId = garmentSubconCustomsInList.SubconContractId;
            SubconContractNo = garmentSubconCustomsInList.SubconContractNo;
            Supplier = new Supplier(garmentSubconCustomsInList.SupplierId.Value, garmentSubconCustomsInList.SupplierCode, garmentSubconCustomsInList.SupplierName);
            Remark = garmentSubconCustomsInList.Remark;
            CreatedBy = garmentSubconCustomsInList.AuditTrail.CreatedBy;
            IsUsed = garmentSubconCustomsInList.IsUsed;
            SubconCategory = garmentSubconCustomsInList.SubconCategory;
            Items = new List<GarmentSubconCustomsInItemDto>();
        }

        public Guid Id { get; set; }
        public string BcNo { get; set; }
        public string BcType { get; set; }
        public DateTimeOffset BcDate { get; set; }
        public string SubconType { get; set; }
        public Guid SubconContractId { get; set; }
        public string SubconContractNo { get; set; }
        public Supplier Supplier { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public string SubconCategory { get; set; }
        public List<GarmentSubconCustomsInItemDto> Items { get; set; }


    }
}
