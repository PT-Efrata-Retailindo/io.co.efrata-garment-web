using Infrastructure.Domain;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Events.GarmentSample.SampleAvalProducts;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts
{
    public class GarmentSampleAvalProduct : AggregateRoot<GarmentSampleAvalProduct, GarmentSampleAvalProductReadModel>
    {
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public DateTimeOffset? AvalDate { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }

        public GarmentSampleAvalProduct(Guid identity, string roNo, string article, DateTimeOffset? avalDate, UnitDepartmentId unitId, string unitCode, string unitName) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            RONo = roNo;
            Article = article;
            AvalDate = avalDate;
            UnitId = unitId;
            UnitName = unitName;
            UnitCode = unitCode;

            ReadModel = new GarmentSampleAvalProductReadModel(Identity)
            {
                RONo = RONo,
                Article = Article,
                AvalDate = AvalDate,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName
            };
            ReadModel.AddDomainEvent(new OnGarmentSampleAvalProductPlaced(this.Identity));
        }

        public GarmentSampleAvalProduct(GarmentSampleAvalProductReadModel readModel) : base(readModel)
        {
            RONo = readModel.RONo;
            Article = readModel.Article;
            AvalDate = readModel.AvalDate;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
        }

        public void SetRONo(string newRONo)
        {
            Validator.ThrowIfNullOrEmpty(() => newRONo);
            if (newRONo != RONo)
            {
                RONo = newRONo;
                ReadModel.RONo = newRONo;
            }
        }

        public void SetArticle(string newArticle)
        {
            Validator.ThrowIfNullOrEmpty(() => newArticle);
            if (newArticle != Article)
            {
                Article = newArticle;
                ReadModel.Article = newArticle;
            }
        }

        public void SetAvalDate(DateTimeOffset? newAvalDate)
        {
            if (newAvalDate != AvalDate)
            {
                AvalDate = newAvalDate;
                ReadModel.AvalDate = newAvalDate;

                MarkModified();
            }
        }

        public void SetModified()
        {
            MarkModified();
        }

        protected override GarmentSampleAvalProduct GetEntity()
        {
            return this;
        }
    }
}
