using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentDeliveryReturns.ReadModels
{
    public class GarmentDeliveryReturnItemReadModel : ReadModelBase
    {
        public GarmentDeliveryReturnItemReadModel(Guid identity) : base(identity)
        {

        }
        public Guid DRId { get; internal set; }
        public int UnitDOItemId { get; internal set; }
        public int UENItemId { get; internal set; }
        public string PreparingItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public string RONo { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
		public string UId { get; set; }
		public virtual GarmentDeliveryReturnReadModel GarmentDeliveryReturnIdentity { get; internal set; }
    }
}