using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries
{
    public class GarmentSampleFinishingMonitoringDto
    {
        public GarmentSampleFinishingMonitoringDto()
        {
        }

        public string roJob { get; internal set; }
        public string article { get; internal set; }
        public string buyerCode { get; internal set; }
        public double qtyOrder { get; internal set; }
        public double stock { get; internal set; }
        public double sewingOutQtyPcs { get; internal set; }
        public double finishingOutQtyPcs { get; internal set; }
        public string uomUnit { get; internal set; }
        public string style { get; internal set; }
        public double remainQty { get; internal set; }
        public double price { get; internal set; }
        public double nominal { get; internal set; }
        public GarmentSampleFinishingMonitoringDto(GarmentSampleFinishingMonitoringDto garmentMonitoring)
        {

            roJob = garmentMonitoring.roJob;
            article = garmentMonitoring.article;
            qtyOrder = garmentMonitoring.qtyOrder;
            stock = garmentMonitoring.stock;
            sewingOutQtyPcs = garmentMonitoring.sewingOutQtyPcs;
            finishingOutQtyPcs = garmentMonitoring.finishingOutQtyPcs;
            uomUnit = garmentMonitoring.uomUnit;
            remainQty = garmentMonitoring.remainQty;
            style = garmentMonitoring.style;
            price = garmentMonitoring.price;
            buyerCode = garmentMonitoring.buyerCode;
            nominal = garmentMonitoring.nominal;
        }
    }
}
