using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsByRONo
{
    public class GarmentSewingOutByRONoDto
    {
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
    }
}
