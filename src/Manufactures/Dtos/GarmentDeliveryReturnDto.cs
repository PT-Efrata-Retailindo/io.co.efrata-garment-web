using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos
{
    public class GarmentDeliveryReturnDto
    {
        public GarmentDeliveryReturnDto()
        {
            Items = new List<GarmentDeliveryReturnItemDto>();
        }

        public GarmentDeliveryReturnDto(GarmentDeliveryReturn garmentDeliveryReturn)
        {
            Id = garmentDeliveryReturn.Identity;

            LastModifiedDate = garmentDeliveryReturn.AuditTrail.ModifiedDate ?? garmentDeliveryReturn.AuditTrail.CreatedDate;
            LastModifiedBy = garmentDeliveryReturn.AuditTrail.ModifiedBy ?? garmentDeliveryReturn.AuditTrail.CreatedBy;

            DRNo = garmentDeliveryReturn.DRNo;
            RONo = garmentDeliveryReturn.RONo;
            Article = garmentDeliveryReturn.Article;
            UnitDOId = garmentDeliveryReturn.UnitDOId;
            UnitDONo = garmentDeliveryReturn.UnitDONo;
            UENId = garmentDeliveryReturn.UENId;
            PreparingId = garmentDeliveryReturn.PreparingId;
            ReturnDate = garmentDeliveryReturn.ReturnDate;
            ReturnType = garmentDeliveryReturn.ReturnType;
            Unit = new UnitDepartment(garmentDeliveryReturn.UnitId.Value, garmentDeliveryReturn.UnitName, garmentDeliveryReturn.UnitCode);
            Storage = new Storage(garmentDeliveryReturn.StorageId.Value, garmentDeliveryReturn.StorageName, garmentDeliveryReturn.StorageCode);
            IsUsed = garmentDeliveryReturn.IsUsed;
            CreatedBy = garmentDeliveryReturn.AuditTrail.CreatedBy;
        }

        public Guid Id { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
        public string DRNo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public int UnitDOId { get; set; }
        public string UnitDONo { get; set; }
        public int UENId { get; set; }
        public string PreparingId { get; set; }
        public DateTimeOffset? ReturnDate { get; set; }
        public string ReturnType { get; set; }
        public UnitDepartment Unit { get; set; }
        public Storage Storage { get; set; }
        public bool IsUsed { get; set; }
        public string CreatedBy { get; set; }
        public List<GarmentDeliveryReturnItemDto> Items { get; set; }
    }
}