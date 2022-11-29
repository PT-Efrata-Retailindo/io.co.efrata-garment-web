using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapDestinations.Commands
{
    public class RemoveGarmentScrapDestinationCommand : ICommand<GarmentScrapDestination>
    {
        public RemoveGarmentScrapDestinationCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
