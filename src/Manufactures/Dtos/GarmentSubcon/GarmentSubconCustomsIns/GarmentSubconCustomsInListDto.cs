using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentSubconCustomsIns
{
    public class GarmentSubconCustomsInListDto : BaseDto
    {
        public GarmentSubconCustomsInListDto(GarmentSubconCustomsIn garmentSubconCustomsIn)
        {
            Id = garmentSubconCustomsIn.Identity;
            BcNo = garmentSubconCustomsIn.BcNo;
            BcType = garmentSubconCustomsIn.BcType;
            BcDate = garmentSubconCustomsIn.BcDate;
            SubconType = garmentSubconCustomsIn.SubconType;
            SubconContractId = garmentSubconCustomsIn.SubconContractId;
            SubconContractNo = garmentSubconCustomsIn.SubconContractNo;
            Supplier = new Supplier(garmentSubconCustomsIn.SupplierId.Value, garmentSubconCustomsIn.SupplierCode, garmentSubconCustomsIn.SupplierName);
            Remark = garmentSubconCustomsIn.Remark;
            CreatedBy = garmentSubconCustomsIn.AuditTrail.CreatedBy;
            IsUsed = garmentSubconCustomsIn.IsUsed;
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
        public List<GarmentSubconCustomsInItemDto> Items { get; set; }
    }
}
