using Infrastructure.Domain.Queries;
using Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantitySampleTraceable
{
    public class GetTotalQuantitySampleTraceableQuery : IQuery<GarmentTotalQtyTraceableListViewModel>
    {
        public string rono { get; private set; }

        public GetTotalQuantitySampleTraceableQuery(string Rono, string token)
        {
            this.rono = Rono;
        }
    }
}
