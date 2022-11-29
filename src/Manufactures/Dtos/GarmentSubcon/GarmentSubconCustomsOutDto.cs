using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconCustomsOutDto : BaseDto
    {
        public GarmentSubconCustomsOutDto(GarmentSubconCustomsOut garmentSubconCustomsOut)
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
            SubconCategory = garmentSubconCustomsOut.SubconCategory;
            Items = new List<GarmentSubconCustomsOutItemDto>();
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
        public double TotalQty { get; set; }
        public double UsedQty { get; set; }
        public string SubconCategory { get; set; }
        public List<GarmentSubconCustomsOutItemDto> Items { get; set; }
    }
}
