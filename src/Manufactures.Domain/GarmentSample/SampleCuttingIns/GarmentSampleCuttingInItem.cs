using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingIns
{
    public class GarmentSampleCuttingInItem : AggregateRoot<GarmentSampleCuttingInItem, GarmentSampleCuttingInItemReadModel>
    {
        public Guid CutInId { get; private set; }
        public Guid PreparingId { get; private set; }
        public int UENId { get; private set; }
        public string UENNo { get; private set; }
        public Guid SewingOutId { get; private set; }
        public string SewingOutNo { get; private set; }

        public GarmentSampleCuttingInItem(Guid identity, Guid cutInId, Guid preparingId, int uENId, string uENNo, Guid sewingOutId, string sewingOutNo) : base(identity)
        {
            Identity = identity;
            CutInId = cutInId;
            PreparingId = preparingId;
            UENId = uENId;
            UENNo = uENNo;
            SewingOutId = sewingOutId;
            SewingOutNo = sewingOutNo;

            ReadModel = new GarmentSampleCuttingInItemReadModel(identity)
            {
                CutInId = CutInId,
                PreparingId = PreparingId,
                UENId = UENId,
                UENNo = UENNo,
                SewingOutId = SewingOutId,
                SewingOutNo = SewingOutNo
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleCuttingInPlaced(Identity));
        }

        public GarmentSampleCuttingInItem(GarmentSampleCuttingInItemReadModel readModel) : base(readModel)
        {
            CutInId = readModel.CutInId;
            PreparingId = readModel.PreparingId;
            UENId = readModel.UENId;
            UENNo = readModel.UENNo;
            SewingOutId = readModel.SewingOutId;
            SewingOutNo = readModel.SewingOutNo;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSampleCuttingInItem GetEntity()
        {
            return this;
        }
    }
}
