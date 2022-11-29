using System;
using System.Collections.Generic;
using Infrastructure.Domain.Queries;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.GetSampleCuttingOutForTraceable
{
    public class GetSampleCuttingOutForTraceableQuery : IQuery<GetSampleCuttingOutForTraceableListViewModel>
    {
        public List<string> ro { get; private set; }
        public string token { get; private set; }

        public GetSampleCuttingOutForTraceableQuery(List<string> ro, string token)
        {
            this.ro = ro;
        }
    }
}
