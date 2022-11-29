using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable
{
    public class GetTotalQuantityTraceableQuery : IQuery<GarmentTotalQtyTraceableListViewModel>
    {
        public string rono { get; private set; }

        public GetTotalQuantityTraceableQuery(string Rono, string token)
        {
            this.rono = Rono;
        }
    }
}
