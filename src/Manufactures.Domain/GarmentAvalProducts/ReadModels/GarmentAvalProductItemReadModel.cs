using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.ReadModels
{
    public class GarmentAvalProductItemReadModel : ReadModelBase
    {
        public GarmentAvalProductItemReadModel(Guid identity) : base(identity)
        {

        }

        public Guid APId { get; internal set; }
        public string PreparingId { get; internal set; }
        public string PreparingItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public double BasicPrice { get; internal set; }
        public bool IsReceived { get; internal set; }
		public string UId { get; set; }
		public virtual GarmentAvalProductReadModel GarmentAvalProductIdentity { get; internal set; }
    }
}