using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.GarmentScrapSources.Commands
{
    public class RemoveGarmentScrapSourceCommand : ICommand<GarmentScrapSource>
    {
        public RemoveGarmentScrapSourceCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
