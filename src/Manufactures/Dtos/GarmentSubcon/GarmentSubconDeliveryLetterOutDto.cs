using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconDeliveryLetterOutDto : BaseDto
    {
        public GarmentSubconDeliveryLetterOutDto(GarmentSubconDeliveryLetterOut garmentSubconDeliveryLetterOutList)
        {
            Id = garmentSubconDeliveryLetterOutList.Identity;
            DLNo = garmentSubconDeliveryLetterOutList.DLNo;
            DLType = garmentSubconDeliveryLetterOutList.DLType;
            ContractType = garmentSubconDeliveryLetterOutList.ContractType;
            DLDate = garmentSubconDeliveryLetterOutList.DLDate;
            UENId = garmentSubconDeliveryLetterOutList.UENId;
            UENNo = garmentSubconDeliveryLetterOutList.UENNo;
            PONo = garmentSubconDeliveryLetterOutList.PONo;
            EPOItemId = garmentSubconDeliveryLetterOutList.EPOItemId;
            Remark = garmentSubconDeliveryLetterOutList.Remark;
            CreatedBy = garmentSubconDeliveryLetterOutList.AuditTrail.CreatedBy;
            IsUsed = garmentSubconDeliveryLetterOutList.IsUsed;
            Items = new List<GarmentSubconDeliveryLetterOutItemDto>();
            ServiceType = garmentSubconDeliveryLetterOutList.ServiceType;
            SubconCategory = garmentSubconDeliveryLetterOutList.SubconCategory;
            EPONo = garmentSubconDeliveryLetterOutList.EPONo;
            EPOId = garmentSubconDeliveryLetterOutList.EPOId;
            QtyPacking = garmentSubconDeliveryLetterOutList.QtyPacking;
            UomUnit = garmentSubconDeliveryLetterOutList.UomUnit;
        }

        public Guid Id { get; set; }
        public string DLNo { get; set; }
        public string DLType { get; set; }
        public string ContractType { get; set; }
        public DateTimeOffset DLDate { get; set; }

        public int UENId { get; set; }
        public string UENNo { get; set; }

        public string PONo { get; set; }
        public int EPOItemId { get; set; }

        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public string ServiceType { get; set; }
        public string SubconCategory { get; set; }
        public int EPOId { get; set; }
        public string EPONo { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public List<GarmentSubconDeliveryLetterOutItemDto> Items { get; set; }
    }
}
