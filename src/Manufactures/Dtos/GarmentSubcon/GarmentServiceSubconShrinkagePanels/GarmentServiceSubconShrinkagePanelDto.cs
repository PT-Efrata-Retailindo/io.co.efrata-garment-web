using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconShrinkagePanels
{
    public class GarmentServiceSubconShrinkagePanelDto
    {
        public GarmentServiceSubconShrinkagePanelDto(GarmentServiceSubconShrinkagePanel garmentServiceSubconShrinkagePanelList)
        {
            Id = garmentServiceSubconShrinkagePanelList.Identity;
            ServiceSubconShrinkagePanelNo = garmentServiceSubconShrinkagePanelList.ServiceSubconShrinkagePanelNo;
            ServiceSubconShrinkagePanelDate = garmentServiceSubconShrinkagePanelList.ServiceSubconShrinkagePanelDate;
            Remark = garmentServiceSubconShrinkagePanelList.Remark;
            IsUsed = garmentServiceSubconShrinkagePanelList.IsUsed;
            QtyPacking = garmentServiceSubconShrinkagePanelList.QtyPacking;
            UomUnit = garmentServiceSubconShrinkagePanelList.UomUnit;
            Items = new List<GarmentServiceSubconShrinkagePanelItemDto>();
        }

        public Guid Id { get; internal set; }
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset ServiceSubconShrinkagePanelDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public virtual List<GarmentServiceSubconShrinkagePanelItemDto> Items { get; internal set; }
    }
}
