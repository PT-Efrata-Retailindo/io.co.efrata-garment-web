using Infrastructure.Domain.Queries;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class GetAllCuttingOutQuery : IQuery<CuttingOutListViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string keyword { get; private set; }
        public string filter { get; private set; }

        public GetAllCuttingOutQuery(int page, int size, string order, string keyword, string filter)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.keyword = keyword;
            this.filter = filter;
        }
    }
}
