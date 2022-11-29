using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentCuttingOutListDto : BaseDto
    {
        public GarmentCuttingOutListDto(GarmentCuttingOut garmentCuttingOut)
        {
            Id = garmentCuttingOut.Identity;
            CutOutNo = garmentCuttingOut.CutOutNo;
            CuttingOutType = garmentCuttingOut.CuttingOutType;
            UnitFrom = new UnitDepartment(garmentCuttingOut.UnitFromId.Value, garmentCuttingOut.UnitFromCode, garmentCuttingOut.UnitFromName);
            CuttingOutDate = garmentCuttingOut.CuttingOutDate;
            RONo = garmentCuttingOut.RONo;
            Article = garmentCuttingOut.Article;
            Unit = new UnitDepartment(garmentCuttingOut.UnitId.Value, garmentCuttingOut.UnitCode, garmentCuttingOut.UnitName);
            Comodity = new GarmentComodity(garmentCuttingOut.ComodityId.Value, garmentCuttingOut.ComodityCode, garmentCuttingOut.ComodityName);
            CreatedBy = garmentCuttingOut.AuditTrail.CreatedBy;
            Items = new List<GarmentCuttingOutItemDto>();
        }

        public Guid Id { get; set; }
        public string CutOutNo { get; set; }
        public string CuttingOutType { get; set; }

        public UnitDepartment UnitFrom { get; set; }
        public DateTimeOffset CuttingOutDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public GarmentComodity Comodity { get; set; }

        public double TotalRemainingQuantity { get; set; }
        public double TotalCuttingOutQuantity { get; set; }
        public List<string> Products { get; set; }
        public List<GarmentCuttingOutItemDto> Items { get; set; }
    }
}