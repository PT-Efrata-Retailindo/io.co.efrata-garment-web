using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.Monitoring
{
    public class GarmentSampleCuttingMonitoringDto
    {
        public GarmentSampleCuttingMonitoringDto()
        {
        }

        public Guid Id { get; internal set; }
        public string roJob { get; internal set; }
        public string article { get; internal set; }
        public string productCode { get; internal set; }
        public string buyerCode { get; internal set; }
        public double qtyOrder { get; internal set; }
        public string style { get; internal set; }
        public double hours { get; internal set; }
        public double stock { get; internal set; }
        public double cuttingQtyMeter { get; internal set; }
        public double cuttingQtyPcs { get; internal set; }
        public double fc { get; internal set; }
        public double expenditure { get; internal set; }
        public double remainQty { get; internal set; }
        public decimal price { get; internal set; }
        public decimal nominal { get; internal set; }
        public GarmentSampleCuttingMonitoringDto(GarmentSampleCuttingMonitoringDto garmentSampleCuttingMonitoringDto)
        {
            Id = garmentSampleCuttingMonitoringDto.Id;
            roJob = garmentSampleCuttingMonitoringDto.roJob;
            article = garmentSampleCuttingMonitoringDto.article;
            qtyOrder = garmentSampleCuttingMonitoringDto.qtyOrder;
            productCode = garmentSampleCuttingMonitoringDto.productCode;
            style = garmentSampleCuttingMonitoringDto.style;
            hours = garmentSampleCuttingMonitoringDto.hours;
            cuttingQtyMeter = garmentSampleCuttingMonitoringDto.cuttingQtyMeter;
            stock = garmentSampleCuttingMonitoringDto.stock;
            cuttingQtyPcs = garmentSampleCuttingMonitoringDto.cuttingQtyPcs;
            fc = garmentSampleCuttingMonitoringDto.fc;
            expenditure = garmentSampleCuttingMonitoringDto.expenditure;
            remainQty = garmentSampleCuttingMonitoringDto.remainQty;
            nominal = garmentSampleCuttingMonitoringDto.nominal;
        }
    }
}
