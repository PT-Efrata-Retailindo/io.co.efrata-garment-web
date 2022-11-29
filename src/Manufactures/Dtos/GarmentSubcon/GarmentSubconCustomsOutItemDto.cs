using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSubcon
{
    public class GarmentSubconCustomsOutItemDto : BaseDto
    {
        public GarmentSubconCustomsOutItemDto(GarmentSubconCustomsOutItem garmentSubconCustomsOutItem)
        {
            Id = garmentSubconCustomsOutItem.Identity;
            SubconDLOutNo = garmentSubconCustomsOutItem.SubconDLOutNo;
            SubconDLOutId = garmentSubconCustomsOutItem.SubconDLOutId;
            Quantity = garmentSubconCustomsOutItem.Quantity;
            SubconCustomsOutId = garmentSubconCustomsOutItem.SubconCustomsOutId;
        }
        public Guid Id { get; set; }
        public string SubconDLOutNo { get; set; }
        public Guid SubconDLOutId { get; set; }
        public double Quantity { get; set; }
        public Guid SubconCustomsOutId { get; set; }
    }
}
