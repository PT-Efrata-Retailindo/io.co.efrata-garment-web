using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleAvalProducts
{
    public class GarmentSampleAvalProductDto
    {
        public GarmentSampleAvalProductDto()
        {
            Items = new List<GarmentSampleAvalProductItemDto>();
        }

        public GarmentSampleAvalProductDto(GarmentSampleAvalProduct garmentAvalProduct)
        {
            Id = garmentAvalProduct.Identity;

            LastModifiedDate = garmentAvalProduct.AuditTrail.ModifiedDate ?? garmentAvalProduct.AuditTrail.CreatedDate;
            LastModifiedBy = garmentAvalProduct.AuditTrail.ModifiedBy ?? garmentAvalProduct.AuditTrail.CreatedBy;

            RONo = garmentAvalProduct.RONo;
            Article = garmentAvalProduct.Article;
            AvalDate = garmentAvalProduct.AvalDate;
            Unit = new UnitDepartment(garmentAvalProduct.UnitId.Value, garmentAvalProduct.UnitCode, garmentAvalProduct.UnitName);
            CreatedBy = garmentAvalProduct.AuditTrail.CreatedBy;
        }

        public Guid Id { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset? AvalDate { get; set; }
        public UnitDepartment Unit { get; set; }
        public string CreatedBy { get; set; }
        public List<GarmentSampleAvalProductItemDto> Items { get; set; }
    }
}
