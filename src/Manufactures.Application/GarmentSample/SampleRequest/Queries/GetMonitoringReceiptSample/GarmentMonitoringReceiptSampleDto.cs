using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample
{
    public class GarmentMonitoringReceiptSampleDto
    {
        public GarmentMonitoringReceiptSampleDto()
        {
        }

        public Guid Id { get; internal set; }
        public string sampleRequestNo { get; internal set; }
        public string roNoSample { get; internal set; }
        public string article { get; internal set; }
        public string sampleCategory { get; internal set; }
        public string sampleType { get; internal set; }
        public string buyer { get; internal set; }
        public string style { get; internal set; }
        public string color { get; internal set; }
        public string sizeName { get; internal set; }
        public string sizeDescription { get; internal set; }
        public double quantity { get; internal set; }
        public DateTimeOffset sentDate { get; internal set; }
        public  DateTimeOffset ? receivedDate { get; internal set; }
        public string garmentSectionName { get; internal set; }
        public DateTimeOffset sampleRequestDate { get; internal set; }
        public string sampleTo { get; internal set; }

        public GarmentMonitoringReceiptSampleDto(GarmentMonitoringReceiptSampleDto garmentMonitoringReceiptSampleDto)
        {
            Id = garmentMonitoringReceiptSampleDto.Id;
            roNoSample = garmentMonitoringReceiptSampleDto.roNoSample;
            sampleRequestNo = garmentMonitoringReceiptSampleDto.sampleRequestNo;
            sampleCategory = garmentMonitoringReceiptSampleDto.sampleCategory;
            sampleType = garmentMonitoringReceiptSampleDto.sampleType;
            buyer = garmentMonitoringReceiptSampleDto.buyer;
            style = garmentMonitoringReceiptSampleDto.style;
            color = garmentMonitoringReceiptSampleDto.color;
            sizeDescription = garmentMonitoringReceiptSampleDto.sizeDescription;
            sizeName = garmentMonitoringReceiptSampleDto.sizeName;
            quantity = garmentMonitoringReceiptSampleDto.quantity;
            sentDate = garmentMonitoringReceiptSampleDto.sentDate;
            receivedDate = garmentMonitoringReceiptSampleDto.receivedDate;
            sampleRequestDate = garmentMonitoringReceiptSampleDto.sampleRequestDate;
            garmentSectionName = garmentMonitoringReceiptSampleDto.garmentSectionName;
            sampleTo = garmentMonitoringReceiptSampleDto.sampleTo;
        }
    }
}
