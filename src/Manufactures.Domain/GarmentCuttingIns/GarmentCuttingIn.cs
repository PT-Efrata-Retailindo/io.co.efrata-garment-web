using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.GarmentCuttingIns
{
    public class GarmentCuttingIn : AggregateRoot<GarmentCuttingIn, GarmentCuttingInReadModel>
    {
        public string CutInNo { get; private set; }
        public string CuttingType { get; private set; }
        public string CuttingFrom { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public DateTimeOffset CuttingInDate { get; private set; }
        public double FC { get; private set; }
		public string UId { get; private set; }

		public GarmentCuttingIn(Guid identity, string cutInNo, string cuttingType, string cuttingFrom, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, DateTimeOffset cuttingInDate, double fC) : base(identity)
        {
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => rONo);

            //MarkTransient();

            Identity = identity;
            CutInNo = cutInNo;
            CuttingType = cuttingType;
            CuttingFrom = cuttingFrom;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            CuttingInDate = cuttingInDate;
            FC = fC;

            ReadModel = new GarmentCuttingInReadModel(Identity)
            {
                CutInNo = CutInNo,
                CuttingType = CuttingType,
                CuttingFrom= CuttingFrom,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                CuttingInDate = CuttingInDate,
                FC = FC,
            };

            ReadModel.AddDomainEvent(new OnGarmentCuttingInPlaced(Identity));
        }

        public GarmentCuttingIn(GarmentCuttingInReadModel readModel) : base(readModel)
        {
            CutInNo = readModel.CutInNo;
            CuttingType = readModel.CuttingType;
            CuttingFrom = readModel.CuttingFrom;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            CuttingInDate = readModel.CuttingInDate;
            FC = readModel.FC;
        }

        public void SetFC(double FC)
        {
            if (this.FC != FC)
            {
                this.FC = FC;
                ReadModel.FC = FC;
            }
        }

        public void SetDate(DateTimeOffset CuttingInDate)
        {
            if (this.CuttingInDate != CuttingInDate)
            {
                this.CuttingInDate = CuttingInDate;
                ReadModel.CuttingInDate = CuttingInDate;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentCuttingIn GetEntity()
        {
            return this;
        }
    }
}
