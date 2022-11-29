using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetAllGarmentSampleAvalComponents
{
    public class GetAllGarmentSampleAvalComponentsQuery : IQuery<GarmentSampleAvalComponentsListViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string keyword { get; private set; }
        public string filter { get; private set; }

        public GetAllGarmentSampleAvalComponentsQuery(int page, int size, string order, string keyword, string filter)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.keyword = keyword;
            this.filter = filter;
        }
    }
}
