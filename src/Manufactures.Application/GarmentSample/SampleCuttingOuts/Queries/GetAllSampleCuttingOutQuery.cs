using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries
{
    public class GetAllSampleCuttingOutQuery : IQuery<SampleCuttingOutListViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string keyword { get; private set; }
        public string filter { get; private set; }

        public GetAllSampleCuttingOutQuery(int page, int size, string order, string keyword, string filter)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.keyword = keyword;
            this.filter = filter;
        }
    }
}
