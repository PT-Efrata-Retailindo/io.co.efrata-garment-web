using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingAdjustments.ReadModels
{
    public class GarmentCuttingAdjustmentReadModel : ReadModelBase
    {
        public GarmentCuttingAdjustmentReadModel(Guid identity) : base(identity)
        {
        }
        public string AdjustmentNo { get; internal set; }
        public string CutInNo { get; internal set; }
        public Guid CutInId { get; internal set; }
        public string RONo { get; internal set; }
        public decimal TotalFC { get; internal set; }
        public decimal TotalActualFC { get; internal set; }
        public decimal TotalQuantity { get; internal set; }
        public decimal TotalActualQuantity { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public DateTimeOffset AdjustmentDate { get; internal set; }
        public string AdjustmentDesc { get; internal set; }

        public virtual ICollection<GarmentCuttingAdjustmentItemReadModel> Items { get; internal set; }
    }
}
