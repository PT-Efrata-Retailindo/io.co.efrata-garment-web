using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentSubconCuttingOutListDto : BaseDto
    {
        public GarmentSubconCuttingOutListDto(GarmentSubconCuttingOut garmentCuttingOut)
        {
            Id = garmentCuttingOut.Identity;
            CutOutNo = garmentCuttingOut.CutOutNo;
            CuttingOutType = garmentCuttingOut.CuttingOutType;
            UnitFrom = new UnitDepartment(garmentCuttingOut.UnitFromId.Value, garmentCuttingOut.UnitFromCode, garmentCuttingOut.UnitFromName);
            CuttingOutDate = garmentCuttingOut.CuttingOutDate;
            RONo = garmentCuttingOut.RONo;
            Article = garmentCuttingOut.Article;
            CreatedBy = garmentCuttingOut.AuditTrail.CreatedBy;
            //Unit = new UnitDepartment(garmentCuttingOut.UnitId.Value, garmentCuttingOut.UnitCode, garmentCuttingOut.UnitName);
            //Comodity = new GarmentComodity(garmentCuttingOut.ComodityId.Value, garmentCuttingOut.ComodityCode, garmentCuttingOut.ComodityName);
            IsUsed = garmentCuttingOut.IsUsed;
            Items = new List<GarmentSubconCuttingOutItemDto>();
        }

        public Guid Id { get; set; }
        public string CutOutNo { get; set; }
        public string CuttingOutType { get; set; }

        public UnitDepartment UnitFrom { get; set; }
        public DateTimeOffset CuttingOutDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        //public UnitDepartment Unit { get; set; }
        //public GarmentComodity Comodity { get; set; }

        public double TotalRemainingQuantity { get; set; }
        public double TotalCuttingOutQuantity { get; set; }
        public List<string> Products { get; set; }

        public bool IsUsed { get; set; }
        public List<GarmentSubconCuttingOutItemDto> Items { get; set; }
    }
}
