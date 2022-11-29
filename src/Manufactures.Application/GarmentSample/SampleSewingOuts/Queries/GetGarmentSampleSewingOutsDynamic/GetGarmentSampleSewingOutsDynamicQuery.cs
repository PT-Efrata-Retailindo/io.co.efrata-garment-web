using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsDynamic
{
    public class GetGarmentSampleSewingOutsDynamicQuery : IQuery<GarmentSampleSewingOutsDynamicViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string search { get; private set; }
        public string select { get; private set; }
        public string keyword { get; private set; }
        public string filter { get; private set; }

        public GetGarmentSampleSewingOutsDynamicQuery(int page, int size, string order, string search, string select, string keyword, string filter)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.search = search;
            this.select = select;
            this.keyword = keyword;
            this.filter = filter;
        }
    }
}
