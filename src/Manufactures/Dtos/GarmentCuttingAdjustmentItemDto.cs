using Manufactures.Domain.GarmentCuttingAdjustments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class GarmentCuttingAdjustmentItemDto
    {
        public GarmentCuttingAdjustmentItemDto(GarmentCuttingAdjustmentItem garmentCuttingAdjustmentItem)
        {
            Id = garmentCuttingAdjustmentItem.Identity;
            AdjustmentCuttingId = garmentCuttingAdjustmentItem.AdjustmentCuttingId;
            CutInDetailId = garmentCuttingAdjustmentItem.CutInDetailId;
            PreparingItemId = garmentCuttingAdjustmentItem.PreparingItemId;
            FC = garmentCuttingAdjustmentItem.FC;
            ActualFC = garmentCuttingAdjustmentItem.ActualFC;
            Quantity = garmentCuttingAdjustmentItem.Quantity;
            ActualQuantity = garmentCuttingAdjustmentItem.ActualQuantity;
        }

        public Guid Id { get; set; }
        public Guid AdjustmentCuttingId { get; set; }
        public Guid CutInDetailId { get; set; }
        public Guid PreparingItemId { get; set; }
        public decimal FC { get; set; }
        public decimal ActualFC { get; set; }
        public decimal Quantity { get; set; }
        public decimal ActualQuantity { get; set; }
    }
}
