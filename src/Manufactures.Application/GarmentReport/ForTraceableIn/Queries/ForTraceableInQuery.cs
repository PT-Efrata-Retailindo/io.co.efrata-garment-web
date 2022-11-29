using System;
using Infrastructure.Domain.Queries;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentReport.ForTraceableIn.Queries
{
    public class ForTraceableInQuery : IQuery <ForTraceableInListViewModel>
    {
        public string uenitemid { get; private set; }

        public ForTraceableInQuery(string UENItemId, string token)
        {
            this.uenitemid = UENItemId;
        }
    }
}
