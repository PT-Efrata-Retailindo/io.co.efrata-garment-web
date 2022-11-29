using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconShrinkagePanels
{
    public class GarmentServiceSubconShrinkagePanelListDto : BaseDto
    {
        public GarmentServiceSubconShrinkagePanelListDto(GarmentServiceSubconShrinkagePanel garmentServiceSubconShrinkagePanelList)
        {
            Id = garmentServiceSubconShrinkagePanelList.Identity;
            ServiceSubconShrinkagePanelNo = garmentServiceSubconShrinkagePanelList.ServiceSubconShrinkagePanelNo;
            ServiceSubconShrinkagePanelDate = garmentServiceSubconShrinkagePanelList.ServiceSubconShrinkagePanelDate;
            Remark = garmentServiceSubconShrinkagePanelList.Remark;
            CreatedBy = garmentServiceSubconShrinkagePanelList.AuditTrail.CreatedBy;
            IsUsed = garmentServiceSubconShrinkagePanelList.IsUsed;
            QtyPacking = garmentServiceSubconShrinkagePanelList.QtyPacking;
            UomUnit = garmentServiceSubconShrinkagePanelList.UomUnit;
            Items = new List<GarmentServiceSubconShrinkagePanelItemDto>();
        }

        public Guid Id { get; set; }
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset ServiceSubconShrinkagePanelDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public List<GarmentServiceSubconShrinkagePanelItemDto> Items { get; set; }
    }
}
