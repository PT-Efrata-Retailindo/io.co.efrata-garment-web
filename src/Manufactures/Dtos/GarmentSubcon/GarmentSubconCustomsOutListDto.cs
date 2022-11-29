using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconCustomsOutListDto : BaseDto
    {
        public GarmentSubconCustomsOutListDto(GarmentSubconCustomsOut garmentSubconCustomsOut)
        {
            Id = garmentSubconCustomsOut.Identity;
            CustomsOutNo = garmentSubconCustomsOut.CustomsOutNo;
            CustomsOutDate = garmentSubconCustomsOut.CustomsOutDate;
            SubconContractId = garmentSubconCustomsOut.SubconContractId;
            CustomsOutType = garmentSubconCustomsOut.CustomsOutType;
            SubconType = garmentSubconCustomsOut.SubconType;
            SubconContractNo = garmentSubconCustomsOut.SubconContractNo;
            Supplier = new Supplier(garmentSubconCustomsOut.SupplierId.Value, garmentSubconCustomsOut.SupplierCode, garmentSubconCustomsOut.SupplierName);
            Remark = garmentSubconCustomsOut.Remark;
            CreatedBy = garmentSubconCustomsOut.AuditTrail.CreatedBy;
        }
        public Guid Id { get; set; }
        public string CustomsOutNo { get; set; }
        public DateTimeOffset CustomsOutDate { get; set; }
        public string CustomsOutType { get; set; }
        public string SubconType { get; set; }
        public Guid SubconContractId { get; set; }
        public string SubconContractNo { get; set; }
        public Supplier Supplier { get; set; }
        public string Remark { get; set; }
    }
}
