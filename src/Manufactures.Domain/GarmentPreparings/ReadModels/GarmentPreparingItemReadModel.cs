using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings.ReadModels
{
    public class GarmentPreparingItemReadModel : ReadModelBase
    {
        public GarmentPreparingItemReadModel(Guid identity) : base(identity)
        {

        }

        public int UENItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string FabricType { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public double BasicPrice { get; internal set; }
        public Guid GarmentPreparingId { get; internal set; }
		public string UId { get; private set; }
		public virtual GarmentPreparingReadModel GarmentPreparingIdentity { get; internal set; }


        public string ROSource { get; internal set; }
        public string CustomsCategory { get; internal set; }
    }
}