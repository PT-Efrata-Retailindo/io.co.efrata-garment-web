using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleDeliveryReturns
{
    public class GarmentSampleDeliveryReturnDto
    {
        public GarmentSampleDeliveryReturnDto()
        {
            Items = new List<GarmentSampleDeliveryReturnItemDto>();
        }

        public GarmentSampleDeliveryReturnDto(GarmentSampleDeliveryReturn garmentSampleDeliveryReturn)
        {
            Id = garmentSampleDeliveryReturn.Identity;

            LastModifiedDate = garmentSampleDeliveryReturn.AuditTrail.ModifiedDate ?? garmentSampleDeliveryReturn.AuditTrail.CreatedDate;
            LastModifiedBy = garmentSampleDeliveryReturn.AuditTrail.ModifiedBy ?? garmentSampleDeliveryReturn.AuditTrail.CreatedBy;

            DRNo = garmentSampleDeliveryReturn.DRNo;
            RONo = garmentSampleDeliveryReturn.RONo;
            Article = garmentSampleDeliveryReturn.Article;
            UnitDOId = garmentSampleDeliveryReturn.UnitDOId;
            UnitDONo = garmentSampleDeliveryReturn.UnitDONo;
            UENId = garmentSampleDeliveryReturn.UENId;
            PreparingId = garmentSampleDeliveryReturn.PreparingId;
            ReturnDate = garmentSampleDeliveryReturn.ReturnDate;
            ReturnType = garmentSampleDeliveryReturn.ReturnType;
            Unit = new UnitDepartment(garmentSampleDeliveryReturn.UnitId.Value, garmentSampleDeliveryReturn.UnitName, garmentSampleDeliveryReturn.UnitCode);
            Storage = new Storage(garmentSampleDeliveryReturn.StorageId.Value, garmentSampleDeliveryReturn.StorageName, garmentSampleDeliveryReturn.StorageCode);
            IsUsed = garmentSampleDeliveryReturn.IsUsed;
            CreatedBy = garmentSampleDeliveryReturn.AuditTrail.CreatedBy;
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
        public List<GarmentSampleDeliveryReturnItemDto> Items { get; set; }
    }
}
