using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalComponents.ReadModels
{
    public class GarmentAvalComponentItemReadModel : ReadModelBase
    {
        public GarmentAvalComponentItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid AvalComponentId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public Guid SewingOutItemId { get; internal set; }
        public Guid SewingOutDetailId { get; internal set; }
        public long ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public string Color { get; internal set; }
        public double Quantity { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public long SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public decimal Price { get; internal set; }
        public decimal BasicPrice { get; internal set; }

		public string UId { get; internal set; }
		public virtual GarmentAvalComponentReadModel GarmentAvalComponent { get; internal set; }
    }
}
