using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleCuttingIns
{
    public class GarmentSampleCuttingInItemDto : BaseDto
    {
        public GarmentSampleCuttingInItemDto(GarmentSampleCuttingInItem garmentSampleCuttingInItem)
        {
            Id = garmentSampleCuttingInItem.Identity;
            CutInId = garmentSampleCuttingInItem.CutInId;
            PreparingId = garmentSampleCuttingInItem.PreparingId;
            UENId = garmentSampleCuttingInItem.UENId;
            UENNo = garmentSampleCuttingInItem.UENNo;
            SewingOutId = garmentSampleCuttingInItem.SewingOutId;
            SewingOutNo = garmentSampleCuttingInItem.SewingOutNo;
            Details = new List<GarmentSampleCuttingInDetailDto>();
        }

        public Guid Id { get; set; }
        public Guid CutInId { get; set; }
        public Guid PreparingId { get; set; }
        public int UENId { get; set; }
        public string UENNo { get; set; }
        public Guid SewingOutId { get; set; }
        public string SewingOutNo { get; set; }
        public List<GarmentSampleCuttingInDetailDto> Details { get; set; }
    }
}
