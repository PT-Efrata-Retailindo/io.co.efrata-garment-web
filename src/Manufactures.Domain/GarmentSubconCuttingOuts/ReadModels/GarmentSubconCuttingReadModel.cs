using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels
{
    public class GarmentSubconCuttingReadModel : ReadModelBase
    {
        public GarmentSubconCuttingReadModel(Guid identity) : base(identity)
        {
        }
        public string RONo { get; internal set; }
        public string SizeName { get; internal set; }
        public int SizeId { get; internal set; }
        public double Quantity { get; internal set; }
        public double FinishingInQuantity { get; internal set; }

        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public string Remark { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public double BasicPrice { get; internal set; }

    }
}
