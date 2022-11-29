using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleStocks.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleStocks
{
    public class GarmentSampleStock : AggregateRoot<GarmentSampleStock, GarmentSampleStockReadModel>
    {

        public string SampleStockNo { get; private set; }
        public string ArchiveType { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Description { get; private set; }


        public GarmentSampleStock(Guid identity, string sampleStockNo, string archiveType,string rONo, string article, GarmentComodityId comodityId, string comodityCode, string comodityName, SizeId sizeId, string sizeName, UomId uomId, string uomUnit, double quantity, string description) : base(identity)
        {
            Validator.ThrowIfNull(() => rONo);

            //MarkTransient();

            Identity = identity;
            SampleStockNo = sampleStockNo;
            ArchiveType = archiveType;
            RONo = rONo;
            Article = article;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Description = description;

            ReadModel = new GarmentSampleStockReadModel(Identity)
            {
                SampleStockNo = SampleStockNo,
                RONo = RONo,
                Article = Article,
                ArchiveType = ArchiveType,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Description=Description
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleStockPlaced(Identity));
        }

        public GarmentSampleStock(GarmentSampleStockReadModel readModel) : base(readModel)
        {
            SampleStockNo = readModel.SampleStockNo;
            RONo = readModel.RONo;
            Article = readModel.Article;
            ArchiveType = readModel.ArchiveType;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomUnit = readModel.UomUnit;
            UomId = new UomId(readModel.UomId);
            Description = readModel.Description;

        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }
        

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSampleStock GetEntity()
        {
            return this;
        }
    }
}
