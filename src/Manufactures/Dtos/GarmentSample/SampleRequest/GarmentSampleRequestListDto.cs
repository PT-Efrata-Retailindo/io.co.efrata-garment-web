using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleRequest
{
    public class GarmentSampleRequestListDto : BaseDto
    {
        public GarmentSampleRequestListDto(GarmentSampleRequest garmentSampleRequest)
        {
            Id = garmentSampleRequest.Identity;
            SampleCategory = garmentSampleRequest.SampleCategory;
            SampleRequestNo = garmentSampleRequest.SampleRequestNo;
            RONoSample = garmentSampleRequest.RONoSample;
            Buyer = new Buyer(garmentSampleRequest.BuyerId.Value, garmentSampleRequest.BuyerCode, garmentSampleRequest.BuyerName);
            Comodity = new GarmentComodity(garmentSampleRequest.ComodityId.Value, garmentSampleRequest.ComodityCode, garmentSampleRequest.ComodityName);
            Date = garmentSampleRequest.Date;
            SentDate = garmentSampleRequest.SentDate;
            POBuyer = garmentSampleRequest.POBuyer;
            IsPosted = garmentSampleRequest.IsPosted;
            IsReceived = garmentSampleRequest.IsReceived;
            ReceivedDate = garmentSampleRequest.ReceivedDate;
            ReceivedBy = garmentSampleRequest.ReceivedBy;
            IsRejected = garmentSampleRequest.IsRejected;
            IsRevised = garmentSampleRequest.IsRevised;
        }

        public Guid Id { get; set; }
        public string SampleCategory { get; set; }
        public string SampleRequestNo { get; set; }
        public string RONoSample { get; internal set; }
        public DateTimeOffset Date { get; set; }

        public Buyer Buyer { get; set; }

        public GarmentComodity Comodity { get; set; }

        public DateTimeOffset SentDate { get; set; }
        public string POBuyer { get; set; }
        public bool IsPosted { get; set; }
        public bool IsReceived { get; set; }
        public bool IsRejected { get; set; }
        public bool IsRevised { get; set; }
        public DateTimeOffset? ReceivedDate { get; set; }
        public string ReceivedBy { get; set; }
    }
}
