using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetGarmentSampleAvalComponents
{
    public class GetGarmentSampleAvalComponentQuery : IQuery<GarmentSampleAvalComponentViewModel>
    {
        public Guid Identity { get; private set; }

        public GetGarmentSampleAvalComponentQuery(Guid identity)
        {
            this.Identity = identity;
        }
    }
}
