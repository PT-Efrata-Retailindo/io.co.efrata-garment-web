using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetPrepareTraceable
{
    public class GetPrepareTraceableQuery : IQuery<GetPrepareTraceableListViewModel>
    {
        public string Ro { get; private set; }

        public GetPrepareTraceableQuery(string Ro, string token)
        {
            this.Ro = Ro;
        }
    }
}
