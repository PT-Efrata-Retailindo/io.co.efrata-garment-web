using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.GarmentSample.SampleCuttingIns
{
    public class GarmentSampleCuttingInDto : BaseDto
    {
        public GarmentSampleCuttingInDto(GarmentSampleCuttingIn garmentSampleCuttingIn)
        {
            Id = garmentSampleCuttingIn.Identity;
            CutInNo = garmentSampleCuttingIn.CutInNo;
            CuttingType = garmentSampleCuttingIn.CuttingType;
            RONo = garmentSampleCuttingIn.RONo;
            Article = garmentSampleCuttingIn.Article;
            Unit = new UnitDepartment(garmentSampleCuttingIn.UnitId.Value, garmentSampleCuttingIn.UnitCode, garmentSampleCuttingIn.UnitName);
            CuttingInDate = garmentSampleCuttingIn.CuttingInDate;
            FC = garmentSampleCuttingIn.FC;
            CuttingFrom = garmentSampleCuttingIn.CuttingFrom;
            Items = new List<GarmentSampleCuttingInItemDto>();
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
        public List<GarmentSampleCuttingInItemDto> Items { get; set; }
    }
}
