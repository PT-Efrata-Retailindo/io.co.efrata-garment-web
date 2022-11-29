using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts
{
    public class GarmentSubconCustomsOutItem : AggregateRoot<GarmentSubconCustomsOutItem, GarmentSubconCustomsOutItemReadModel>
    {

        public string SubconDLOutNo { get; private set; }
        public Guid SubconDLOutId { get; private set; }
        public double Quantity { get; private set; }
        public Guid SubconCustomsOutId { get; internal set; }

        public GarmentSubconCustomsOutItem(Guid identity, Guid subconCustomsOutId, string subconDLOutNo, Guid subconDLOutId, double quantity) : base(identity)
        {
            Identity = identity;
            SubconDLOutNo = subconDLOutNo;
            SubconDLOutId = subconDLOutId;
            Quantity = quantity;
            SubconCustomsOutId = subconCustomsOutId;
            ReadModel = new GarmentSubconCustomsOutItemReadModel(Identity)
            {
                SubconDLOutNo = SubconDLOutNo,
                SubconDLOutId = SubconDLOutId,
                Quantity = Quantity,
                SubconCustomsOutId= SubconCustomsOutId
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCustomsOutPlaced(Identity));
        }
        public GarmentSubconCustomsOutItem(GarmentSubconCustomsOutItemReadModel readModel) : base(readModel)
        {
            SubconDLOutNo = readModel.SubconDLOutNo;
            SubconDLOutId = readModel.SubconDLOutId;
            Quantity = readModel.Quantity;
            SubconCustomsOutId = readModel.SubconCustomsOutId;
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCustomsOutItem GetEntity()
        {
            return this;
        }
        
    }
}
