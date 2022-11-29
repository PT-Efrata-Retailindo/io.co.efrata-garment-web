using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts.Commands
{
    public class RemoveGarmentFinishingOutCommand : ICommand<GarmentFinishingOut>
    {
        public RemoveGarmentFinishingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
