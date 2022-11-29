using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetCuttingOutForTraceable
{
    public class GetCuttingOutForTraceableQuery : IQuery<GetCuttingOutForTraceableListViewModel>
    {
        public List<string> ro { get; private set; }
        public string token { get; private set; }

        public GetCuttingOutForTraceableQuery(List<string> ro, string token)
        {
            this.ro = ro;
        }
    }
}
