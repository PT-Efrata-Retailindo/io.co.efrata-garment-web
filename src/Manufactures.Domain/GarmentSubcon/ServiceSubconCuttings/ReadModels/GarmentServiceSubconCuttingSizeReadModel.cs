using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels
{
    public class GarmentServiceSubconCuttingSizeReadModel : ReadModelBase
    {
        public GarmentServiceSubconCuttingSizeReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CuttingInId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public int SizeId { get; internal set; }
        public string SizeName { get; internal set; }
        public double Quantity { get; internal set; }
        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }
        public string Color { get; internal set; }
        public Guid ServiceSubconCuttingDetailId { get; internal set; }

        public virtual GarmentServiceSubconCuttingDetailReadModel GarmentServiceSubconCuttingDetail { get; internal set; }
    }
}
