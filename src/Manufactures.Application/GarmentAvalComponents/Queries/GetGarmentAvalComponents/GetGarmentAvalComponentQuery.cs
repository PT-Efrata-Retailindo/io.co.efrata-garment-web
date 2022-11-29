using Infrastructure.Domain.Queries;
using System;

namespace Manufactures.Domain.GarmentAvalComponents.Queries.GetGarmentAvalComponents
{
    public class GetGarmentAvalComponentQuery : IQuery<GarmentAvalComponentViewModel>
    {
        public Guid Identity { get; private set; }

        public GetGarmentAvalComponentQuery(Guid identity)
        {
            this.Identity = identity;
        }
    }
}
