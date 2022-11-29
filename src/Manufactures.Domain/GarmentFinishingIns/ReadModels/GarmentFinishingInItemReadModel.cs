using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns.ReadModels
{
    public class GarmentFinishingInItemReadModel : ReadModelBase
    {
        public GarmentFinishingInItemReadModel(Guid identity) : base(identity)
        {

        }
        public Guid FinishingInId { get; internal set; }
        public Guid SewingOutItemId { get; internal set; }
        public Guid SewingOutDetailId { get; internal set; }
        public Guid SubconCuttingId { get; internal set; }
        public long DODetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
		public string UId { get; internal set; }
		public virtual GarmentFinishingInReadModel GarmentFinishingIn { get; internal set; }

    }
}
