using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleRequest
{
    public class GarmentSampleRequestDto : BaseDto
    {
        public GarmentSampleRequestDto(GarmentSampleRequest garmentSampleRequest)
        {
            Id = garmentSampleRequest.Identity;
            CreatedBy = garmentSampleRequest.CreatedBy;
            SampleCategory = garmentSampleRequest.SampleCategory;
            SampleRequestNo = garmentSampleRequest.SampleRequestNo;
            RONoSample = garmentSampleRequest.RONoSample;
            RONoCC = garmentSampleRequest.RONoCC;
            Buyer = new Buyer(garmentSampleRequest.BuyerId.Value, garmentSampleRequest.BuyerCode, garmentSampleRequest.BuyerName);
            Comodity = new GarmentComodity(garmentSampleRequest.ComodityId.Value, garmentSampleRequest.ComodityCode, garmentSampleRequest.ComodityName);
            Date = garmentSampleRequest.Date;
            SampleType = garmentSampleRequest.SampleType;
            Packing = garmentSampleRequest.Packing;
            SentDate = garmentSampleRequest.SentDate;
            POBuyer = garmentSampleRequest.POBuyer;
            Attached = garmentSampleRequest.Attached;
            Remark = garmentSampleRequest.Remark;
            IsPosted = garmentSampleRequest.IsPosted;
            IsReceived = garmentSampleRequest.IsReceived;
            ReceivedDate = garmentSampleRequest.ReceivedDate;
            ReceivedBy = garmentSampleRequest.ReceivedBy;
            IsRejected = garmentSampleRequest.IsRejected;
            RejectedDate = garmentSampleRequest.RejectedDate;
            RejectedBy = garmentSampleRequest.RejectedBy;
            RejectedReason = garmentSampleRequest.RejectedReason;
            IsRevised = garmentSampleRequest.IsRevised;
            RevisedDate = garmentSampleRequest.RevisedDate;
            RevisedBy = garmentSampleRequest.RevisedBy;
            RevisedReason = garmentSampleRequest.RevisedReason;
            SampleProducts = new List<GarmentSampleRequestProductDto>();
            SampleSpecifications = new List<GarmentSampleRequestSpecificationDto>();
            ImagesPath = String.IsNullOrEmpty(garmentSampleRequest.ImagesPath) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(garmentSampleRequest.ImagesPath);
            ImagesName = String.IsNullOrEmpty(garmentSampleRequest.ImagesName) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(garmentSampleRequest.ImagesName);
            DocumentsPath = String.IsNullOrEmpty(garmentSampleRequest.DocumentsPath) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(garmentSampleRequest.DocumentsPath);
            DocumentsFileName = String.IsNullOrEmpty(garmentSampleRequest.DocumentsFileName) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(garmentSampleRequest.DocumentsFileName);
            Section = new SectionValueObject(garmentSampleRequest.SectionId.Value, garmentSampleRequest.SectionCode);
            SampleTo = garmentSampleRequest.SampleTo;
        }

        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public string SampleCategory { get;  set; }
        public string SampleRequestNo { get;  set; }
        public string RONoSample { get; internal set; }
        public string RONoCC { get; internal set; }
        public DateTimeOffset Date { get;  set; }

        public Buyer Buyer { get;  set; }

        public GarmentComodity Comodity { get;  set; }

        public string SampleType { get;  set; }
        public string Packing { get;  set; }
        public DateTimeOffset SentDate { get;  set; }
        public string POBuyer { get;  set; }
        public string Attached { get;  set; }
        public string Remark { get;  set; }
        public bool IsPosted { get;  set; }
        public bool IsReceived { get;  set; }
        public DateTimeOffset? ReceivedDate { get; set; }
        public string ReceivedBy { get; set; }
        public bool IsRejected { get; set; }
        public DateTimeOffset? RejectedDate { get; set; }
        public string RejectedBy { get; set; }
        public string RejectedReason { get; set; }

        public bool IsRevised { get; set; }
        public DateTimeOffset? RevisedDate { get; set; }
        public string RevisedBy { get; set; }
        public string RevisedReason { get; set; }

        public List<string> ImagesFile { get; set; }
        public List<string> DocumentsFile { get; set; }
        public List<string> ImagesPath { get; set; }
        public List<string> DocumentsPath { get; set; }
        public List<string> ImagesName { get; set; }
        public List<string> DocumentsFileName { get; set; }
        public SectionValueObject Section { get; set; }
        public List<GarmentSampleRequestProductDto> SampleProducts { get; set; }
        public List<GarmentSampleRequestSpecificationDto> SampleSpecifications { get; set; }
        public string SampleTo { get; set; }
    }
}
