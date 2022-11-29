using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.ReadModels
{
    public class GarmentSewingOutItemReadModel : ReadModelBase
    {
        public GarmentSewingOutItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid SewingOutId { get; internal set; }
        public Guid SewingInId { get; internal set; }
        public Guid SewingInItemId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public double RemainingQuantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
		public string UId { get; private set; }
		public virtual ICollection<GarmentSewingOutDetailReadModel> GarmentSewingOutDetail { get; internal set; }
        public virtual GarmentSewingOutReadModel GarmentSewingOutIdentity { get; internal set; }
    }
}
