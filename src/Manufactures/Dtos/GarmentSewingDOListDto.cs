using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSewingDOListDto : BaseDto
    {
        public GarmentSewingDOListDto(GarmentSewingDO garmentSewingDO)
        {
            Id = garmentSewingDO.Identity;
            SewingDONo = garmentSewingDO.SewingDONo;
            CuttingOutId = garmentSewingDO.CuttingOutId;
            UnitFrom = new UnitDepartment(garmentSewingDO.UnitFromId.Value, garmentSewingDO.UnitFromCode, garmentSewingDO.UnitFromName);
            Unit = new UnitDepartment(garmentSewingDO.UnitId.Value, garmentSewingDO.UnitCode, garmentSewingDO.UnitName);
            RONo = garmentSewingDO.RONo;
            Article = garmentSewingDO.Article;
            Comodity = new GarmentComodity(garmentSewingDO.ComodityId.Value, garmentSewingDO.ComodityCode, garmentSewingDO.ComodityName);
            SewingDODate = garmentSewingDO.SewingDODate;
            CreatedBy = garmentSewingDO.AuditTrail.CreatedBy;
            Items = new List<GarmentSewingDOItemDto>();
        }

        public Guid Id { get; set; }
        public string SewingDONo { get; set; }
        public Guid CuttingOutId { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset SewingDODate { get; set; }
        public double TotalQuantity { get; set; }
        public List<string> Products { get; set; }

        public List<GarmentSewingDOItemDto> Items { get; set; }
    }
}
