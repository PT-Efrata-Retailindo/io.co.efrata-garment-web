using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetWIP
{
    public class GarmentWIPDto
    {
        public GarmentWIPDto()
        {
        }

        public string Kode { get; internal set; }
        public string Comodity { get; internal set; }
        public string UnitQtyName { get; internal set; }
        public double WIP { get; internal set; }
        public GarmentWIPDto(GarmentWIPDto garmentWIPDto)
        {
            Kode = garmentWIPDto.Kode;
            Comodity = garmentWIPDto.Comodity;
            UnitQtyName = garmentWIPDto.UnitQtyName;
            WIP = garmentWIPDto.WIP;
        }
    }
}
