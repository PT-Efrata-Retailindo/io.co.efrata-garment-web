using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class CuttingOutJoinedDto
    {
        public GarmentCuttingOutListDto cuttingOutList { get; set; }
        public GarmentCuttingOutItemDto cuttingOutItem { get; set; }
        public GarmentCuttingOutDetailDto cuttingOutDetail { get; set; }
    }
}
