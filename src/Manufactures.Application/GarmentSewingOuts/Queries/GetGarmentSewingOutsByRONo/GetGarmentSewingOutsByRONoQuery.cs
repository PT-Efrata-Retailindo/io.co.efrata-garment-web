using Infrastructure.Domain.Queries;

namespace Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsByRONo
{
    public class GetGarmentSewingOutsByRONoQuery : IQuery<GarmentSewingOutsByRONoViewModel>
    {
        public string keyword { get; private set; }
        public string filter { get; private set; }

        public GetGarmentSewingOutsByRONoQuery(string keyword, string filter)
        {
            this.keyword = keyword;
            this.filter = filter;
        }
    }
}
