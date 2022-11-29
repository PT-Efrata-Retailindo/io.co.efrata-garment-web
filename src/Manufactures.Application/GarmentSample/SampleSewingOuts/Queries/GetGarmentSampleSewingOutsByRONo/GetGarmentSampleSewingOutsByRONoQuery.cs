using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsByRONo
{
    public class GetGarmentSampleSewingOutsByRONoQuery : IQuery<GarmentSampleSewingOutsByRONoViewModel>
    {
        public string keyword { get; private set; }
        public string filter { get; private set; }

        public GetGarmentSampleSewingOutsByRONoQuery(string keyword, string filter)
        {
            this.keyword = keyword;
            this.filter = filter;
        }
    }
}
