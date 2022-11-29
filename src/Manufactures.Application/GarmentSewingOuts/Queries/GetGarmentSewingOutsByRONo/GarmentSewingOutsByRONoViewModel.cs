using System.Collections.Generic;

namespace Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsByRONo
{
    public class GarmentSewingOutsByRONoViewModel
    {
        public List<GarmentSewingOutByRONoDto> data { get; private set; }

        public GarmentSewingOutsByRONoViewModel(List<GarmentSewingOutByRONoDto> data)
        {
            this.data = data;
        }
    }
}
