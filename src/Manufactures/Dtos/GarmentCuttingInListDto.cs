using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentCuttingInListDto
    {
        public GarmentCuttingInListDto(GarmentCuttingIn garmentCuttingIn)
        {
            Id = garmentCuttingIn.Identity;
            CutInNo = garmentCuttingIn.CutInNo;
            CuttingType = garmentCuttingIn.CuttingType;
            RONo = garmentCuttingIn.RONo;
            Article = garmentCuttingIn.Article;
            Unit = new UnitDepartment(garmentCuttingIn.UnitId.Value, garmentCuttingIn.UnitCode, garmentCuttingIn.UnitName);
            CuttingInDate = garmentCuttingIn.CuttingInDate;
            FC = garmentCuttingIn.FC;
            CuttingFrom = garmentCuttingIn.CuttingFrom;
            CreatedBy = garmentCuttingIn.AuditTrail.CreatedBy;
        }

        public Guid Id { get; set; }

        public string CutInNo { get; set; }
        public string CuttingType { get; set; }
        public string CuttingFrom { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset CuttingInDate { get; set; }
        public double FC { get; set; }
        public string CreatedBy { get; set; }
        public double TotalCuttingInQuantity { get; set; }
        public List<string> UENNos { get; set; }
        public List<string> Products { get; set; }

    }
}
