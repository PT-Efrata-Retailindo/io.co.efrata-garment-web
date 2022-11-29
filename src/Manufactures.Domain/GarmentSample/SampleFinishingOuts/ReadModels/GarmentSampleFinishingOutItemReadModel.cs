using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels
{
    public class GarmentSampleFinishingOutItemReadModel : ReadModelBase
    {
        public GarmentSampleFinishingOutItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid FinishingOutId { get; internal set; }
        public Guid FinishingInId { get; internal set; }
        public Guid FinishingInItemId { get; internal set; }
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
        public string UId { get; internal set; }
        public virtual ICollection<GarmentSampleFinishingOutDetailReadModel> GarmentSampleFinishingOutDetail { get; internal set; }
        public virtual GarmentSampleFinishingOutReadModel GarmentSampleFinishingOutIdentity { get; internal set; }
    }
}
