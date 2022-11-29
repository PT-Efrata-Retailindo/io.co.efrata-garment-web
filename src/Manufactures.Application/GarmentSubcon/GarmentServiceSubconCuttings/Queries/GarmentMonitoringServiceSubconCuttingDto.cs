using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.Queries
{
    public class GarmentMonitoringServiceSubconCuttingDto
    {
        public GarmentMonitoringServiceSubconCuttingDto()
        { 
        }
        public string subconNo { get; internal set; }
        public string subconType { get; internal set; }
        public string unitName { get; internal set; }
        public string buyerName { get; internal set; }
        public DateTimeOffset? subconDate { get; internal set; }
        public string roNo { get; internal set; }
        public string article { get; internal set; }
        public string comodity { get; internal set; }
        public string designColor { get; internal set; }
        //public double cuttingInQuantity { get; internal set; }
        public string sizeName { get; internal set; }
        public double quantity { get; internal set; }
        public string uomUnit { get; internal set; }
        public string color { get; internal set; }
        public string uomUnitPacking { get; internal set; }
        public int qtyPacking { get; internal set; }

        public GarmentMonitoringServiceSubconCuttingDto(GarmentMonitoringServiceSubconCuttingDto garmentMonitoring)
        {

            subconNo = garmentMonitoring.subconNo;
            subconType = garmentMonitoring.subconType;
            unitName = garmentMonitoring.unitName;
            buyerName = garmentMonitoring.buyerName;
            subconDate = garmentMonitoring.subconDate;
            roNo = garmentMonitoring.roNo;
            article = garmentMonitoring.article;
            comodity = garmentMonitoring.comodity;
            designColor = garmentMonitoring.designColor;
            //cuttingInQuantity = garmentMonitoring.cuttingInQuantity;
            sizeName = garmentMonitoring.sizeName;
            quantity = garmentMonitoring.quantity;
            uomUnit = garmentMonitoring.uomUnit;
            color = garmentMonitoring.color;
            uomUnitPacking = garmentMonitoring.uomUnitPacking;
            qtyPacking = garmentMonitoring.qtyPacking;
        }

    }
}
