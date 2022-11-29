using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests
{
    public class GarmentSampleRequest : AggregateRoot<GarmentSampleRequest, GarmentSampleRequestReadModel>
    {
        public string SampleCategory { get; private set; }
        public string CreatedBy { get; private set; }
        public string SampleRequestNo { get; private set; }
        public string RONoSample { get; internal set; }
        public string RONoCC { get; internal set; }
        public DateTimeOffset Date { get; private set; }

        public BuyerId BuyerId { get; private set; }
        public string BuyerCode { get; private set; }
        public string BuyerName { get; private set; }

        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }

        public string SampleType { get; private set; }
        public string Packing { get; private set; }
        public DateTimeOffset SentDate { get; private set; }
        public string POBuyer { get; private set; }
        public string Attached { get; private set; }
        public string Remark { get; private set; }

        public bool IsPosted { get; private set; }

        public bool IsReceived { get; private set; }
        public DateTimeOffset? ReceivedDate { get; private set; }
        public string ReceivedBy { get; private set; }

        public bool IsRejected { get; private set; }
        public DateTimeOffset? RejectedDate { get; private set; }
        public string RejectedBy { get; private set; }
        public string RejectedReason { get; private set; }

        public bool IsRevised { get; private set; }
        public DateTimeOffset? RevisedDate { get; set; }
        public string RevisedBy { get; private set; }
        public string RevisedReason { get; private set; }

        public string ImagesPath { get; private set; }
        public string DocumentsPath { get; private set; }
        public string ImagesName { get; private set; }
        public string DocumentsFileName { get; private set; }
        public SectionId SectionId { get; internal set; }
        public string SectionCode { get; internal set; }
        public string SampleTo { get; private set; }

        public GarmentSampleRequest(Guid identity, string sampleCategory, string sampleRequestNo, string rONoSample, string rONoCC, DateTimeOffset date, BuyerId buyerId, string buyerCode, string buyerName, GarmentComodityId comodityId, string comodityCode, string comodityName, string sampleType, string packing, DateTimeOffset sentDate, string pOBuyer, string attached, string remark, bool isPosted, bool isReceived, DateTimeOffset? receivedDate, string receivedBy, bool isRejected, DateTimeOffset? rejectedDate, string rejectedBy, string rejectedReason, bool isRevised, DateTimeOffset? revisedDate, string revisedBy, string revisedReason, string imagesPath, string documentsPath, string imagesName, string documentsFileName, SectionId sectionId, string sectionCode, string sampleTo) : base(identity)
        {
            SampleCategory = sampleCategory;
            SampleRequestNo = sampleRequestNo;
            RONoSample = rONoSample;
            RONoCC = rONoCC;
            Date = date;
            BuyerId = buyerId;
            BuyerCode = buyerCode;
            BuyerName = buyerName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            SampleType = sampleType;
            Packing = packing;
            SentDate = sentDate;
            POBuyer = pOBuyer;
            Attached = attached;
            Remark = remark;
            IsPosted = isPosted;
            IsReceived = isReceived;
            ReceivedDate = receivedDate;
            ReceivedBy = receivedBy;
            IsRejected = isRejected;
            RejectedDate = rejectedDate;
            RejectedBy = rejectedBy;
            RejectedReason = rejectedReason;
            IsRevised = isRevised;
            RevisedDate = revisedDate;
            RevisedBy = revisedBy;
            RevisedReason = revisedReason;
            ImagesPath = imagesPath;
            DocumentsPath = documentsPath;
            ImagesName = imagesName;
            DocumentsFileName = documentsFileName;
            SectionId = sectionId;
            SectionCode = sectionCode;
            SampleTo = sampleTo;

            ReadModel = new GarmentSampleRequestReadModel(Identity)
            {
                SampleCategory = SampleCategory,
                SampleRequestNo = SampleRequestNo,
                RONoSample = RONoSample,
                RONoCC = RONoCC,
                Date = Date,
                BuyerId = BuyerId.Value,
                BuyerCode = BuyerCode,
                BuyerName = BuyerName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                SampleType = SampleType,
                Packing = Packing,
                SentDate = SentDate,
                POBuyer = POBuyer,
                Attached = Attached,
                Remark = Remark,
                IsPosted = IsPosted,
                IsReceived = IsReceived,
                ReceivedDate = ReceivedDate,
                ReceivedBy = ReceivedBy,
                IsRejected=IsRejected,
                RejectedDate = RejectedDate,
                RejectedBy = RejectedBy,
                RejectedReason = RejectedReason,
                IsRevised = IsRevised,
                RevisedDate = RevisedDate,
                RevisedBy = RevisedBy,
                RevisedReason = RevisedReason,
                ImagesPath = ImagesPath,
                DocumentsPath = DocumentsPath,
                ImagesName = ImagesName,
                DocumentsFileName = DocumentsFileName,
                SectionId=SectionId.Value,
                SectionCode=SectionCode,
                SampleTo=SampleTo
        };
            ReadModel.AddDomainEvent(new OnGarmentSampleRequestPlaced(Identity));
        }

        public GarmentSampleRequest(GarmentSampleRequestReadModel readModel) : base(readModel)
        {
            SampleCategory = readModel.SampleCategory;
            SampleRequestNo = readModel.SampleRequestNo;
            RONoSample = readModel.RONoSample;
            RONoCC = readModel.RONoCC;
            Date = readModel.Date;
            BuyerId = new BuyerId(readModel.BuyerId);
            BuyerCode = readModel.BuyerCode;
            BuyerName = readModel.BuyerName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            SampleType = readModel.SampleType;
            Packing = readModel.Packing;
            SentDate = readModel.SentDate;
            POBuyer = readModel.POBuyer;
            Attached = readModel.Attached;
            Remark = readModel.Remark;
            IsPosted = readModel.IsPosted;
            IsReceived = readModel.IsReceived;
            ReceivedDate = readModel.ReceivedDate;
            ReceivedBy = readModel.ReceivedBy;
            IsRejected = readModel.IsRejected;
            RejectedDate = readModel.RejectedDate;
            RejectedBy = readModel.RejectedBy;
            RejectedReason = readModel.RejectedReason;
            IsRevised = readModel.IsRevised;
            RevisedDate = readModel.RevisedDate;
            RevisedBy = readModel.RevisedBy;
            RevisedReason = readModel.RevisedReason;
            ImagesPath = readModel.ImagesPath;
            DocumentsPath = readModel.DocumentsPath;
            ImagesName = readModel.ImagesName;
            DocumentsFileName = readModel.DocumentsFileName;
            SectionCode = readModel.SectionCode;
            SectionId = new SectionId(readModel.SectionId);
            CreatedBy = readModel.CreatedBy;
            SampleTo = readModel.SampleTo;
        }

        

        protected override GarmentSampleRequest GetEntity()
        {
            return this;
        }
        public void SetSampleRequestNo(string SampleRequestNo)
        {
            if (this.SampleRequestNo != SampleRequestNo)
            {
                this.SampleRequestNo = SampleRequestNo;
                ReadModel.SampleRequestNo = SampleRequestNo;
            }
        }

        public void SetIsPosted(bool IsPosted)
        {
            if (this.IsPosted != IsPosted)
            {
                this.IsPosted = IsPosted;
                ReadModel.IsPosted = IsPosted;
            }
        }

        public void SetIsReceived(bool IsReceived)
        {
            if (this.IsReceived != IsReceived)
            {
                this.IsReceived = IsReceived;
                ReadModel.IsReceived = IsReceived;
            }
        }
        public void SetBuyerId(BuyerId BuyerId)
        {
            if (this.BuyerId != BuyerId)
            {
                this.BuyerId = BuyerId;
                ReadModel.BuyerId = BuyerId.Value;
            }
        }
        public void SetBuyerCode(string BuyerCode)
        {
            if (this.BuyerCode != BuyerCode)
            {
                this.BuyerCode = BuyerCode;
                ReadModel.BuyerCode = BuyerCode;
            }
        }

        public void SetSampleTo(string SampleTo)
        {
            if (this.SampleTo != SampleTo)
            {
                this.SampleTo = SampleTo;
                ReadModel.SampleTo = SampleTo;
            }
        }

        public void SetBuyerName(string BuyerName)
        {
            if (this.BuyerName != BuyerName)
            {
                this.BuyerName = BuyerName;
                ReadModel.BuyerName = BuyerName;
            }
        }

        public void SetRemark(string remark)
        {
            if (this.Remark != remark)
            {
                this.Remark = remark;
                ReadModel.Remark = remark;
            }
        }

        public void SetSentDate(DateTimeOffset SentDate)
        {
            if (this.SentDate != SentDate)
            {
                this.SentDate = SentDate;
                ReadModel.SentDate = SentDate;
            }
        }

        public void SetDate(DateTimeOffset Date)
        {
            if (this.Date != Date)
            {
                this.Date = Date;
                ReadModel.Date = Date;
            }
        }

        public void SetComodityId(GarmentComodityId ComodityId)
        {
            if (this.ComodityId != ComodityId)
            {
                this.ComodityId = ComodityId;
                ReadModel.ComodityId = ComodityId.Value;
            }
        }
        public void SetComodityCode(string ComodityCode)
        {
            if (this.ComodityCode != ComodityCode)
            {
                this.ComodityCode = ComodityCode;
                ReadModel.ComodityCode = ComodityCode;
            }
        }
        public void SetComodityName(string ComodityName)
        {
            if (this.ComodityName != ComodityName)
            {
                this.ComodityName = ComodityName;
                ReadModel.ComodityName = ComodityName;
            }
        }
        public void SetRONoCC(string RONoCC)
        {
            if (this.RONoCC != RONoCC)
            {
                this.RONoCC = RONoCC;
                ReadModel.RONoCC = RONoCC;
            }
        }

        public void SetPacking(string Packing)
        {
            if (this.Packing != Packing)
            {
                this.Packing = Packing;
                ReadModel.Packing = Packing;
            }
        }
        public void SetSampleType(string SampleType)
        {
            if (this.SampleType != SampleType)
            {
                this.SampleType = SampleType;
                ReadModel.SampleType = SampleType;
            }
        }
        public void SetPOBuyer(string POBuyer)
        {
            if (this.POBuyer != POBuyer)
            {
                this.POBuyer = POBuyer;
                ReadModel.POBuyer = POBuyer;
            }
        }
        public void SetAttached(string Attached)
        {
            if (this.Attached != Attached)
            {
                this.Attached = Attached;
                ReadModel.Attached = Attached;
            }
        }

        public void SetReceivedDate(DateTimeOffset ReceivedDate)
        {
            if (this.ReceivedDate != ReceivedDate)
            {
                this.ReceivedDate = ReceivedDate;
                ReadModel.ReceivedDate = ReceivedDate;
            }
        }

        public void SetReceivedBy(string ReceivedBy)
        {
            if (this.ReceivedBy != ReceivedBy)
            {
                this.ReceivedBy = ReceivedBy;
                ReadModel.ReceivedBy = ReceivedBy;
            }
        }

        public void SetSectionId(SectionId SectionId)
        {
            if (this.SectionId != SectionId)
            {
                this.SectionId = SectionId;
                ReadModel.SectionId = SectionId.Value;
            }
        }
        public void SetSectionCode(string SectionCode)
        {
            if (this.SectionCode != SectionCode)
            {
                this.SectionCode = SectionCode;
                ReadModel.SectionCode = SectionCode;
            }
        }

        public void SetIsRejected(bool IsRejected)
        {
            if (this.IsRejected != IsRejected)
            {
                this.IsRejected = IsRejected;
                ReadModel.IsRejected = IsRejected;
            }
        }

        public void SetIsRevised(bool IsRevised)
        {
            if (this.IsRevised != IsRevised)
            {
                this.IsRevised = IsRevised;
                ReadModel.IsRevised = IsRevised;
            }
        }

        public void SetImagesPath(string ImagesPath)
        {
            if (this.ImagesPath != ImagesPath)
            {
                this.ImagesPath = ImagesPath;
                ReadModel.ImagesPath = ImagesPath;
            }
        }

        public void SetDocumentsPath(string DocumentsPath)
        {
            if (this.DocumentsPath != DocumentsPath)
            {
                this.DocumentsPath = DocumentsPath;
                ReadModel.DocumentsPath = DocumentsPath;
            }
        }

        public void SetImagesName(List<string> ImagesName)
        {
            this.ImagesName = JsonConvert.SerializeObject(ImagesName);
            ReadModel.ImagesName = JsonConvert.SerializeObject(ImagesName);
        }

        public void SetDocumentsFileName(List<string> DocumentsFileName)
        {
            this.DocumentsFileName = JsonConvert.SerializeObject(DocumentsFileName);
            ReadModel.DocumentsFileName = JsonConvert.SerializeObject(DocumentsFileName);
        }

        public void SetRejectedDate(DateTimeOffset RejectedDate)
        {
            if (this.RejectedDate != RejectedDate)
            {
                this.RejectedDate = RejectedDate;
                ReadModel.RejectedDate = RejectedDate;
            }
        }

        public void SetRejectedBy(string RejectedBy)
        {
            if (this.RejectedBy != RejectedBy)
            {
                this.RejectedBy = RejectedBy;
                ReadModel.RejectedBy = RejectedBy;
            }
        }

        public void SetRejectedReason(string RejectedReason)
        {
            if (this.RejectedReason != RejectedReason)
            {
                this.RejectedReason = RejectedReason;
                ReadModel.RejectedReason = RejectedReason;
            }
        }

        public void SetRevisedDate(DateTimeOffset RevisedDate)
        {
            if (this.RevisedDate != RevisedDate)
            {
                this.RevisedDate = RevisedDate;
                ReadModel.RevisedDate = RevisedDate;
            }
        }

        public void SetRevisedBy(string RevisedBy)
        {
            if (this.RevisedBy != RevisedBy)
            {
                this.RevisedBy = RevisedBy;
                ReadModel.RevisedBy = RevisedBy;
            }
        }

        public void SetRevisedReason(string RevisedReason)
        {
            if (this.RevisedReason != RevisedReason)
            {
                this.RevisedReason = RevisedReason;
                ReadModel.RevisedReason = RevisedReason;
            }
        }

        public void Modify()
        {
            MarkModified();
        }
    }
}
