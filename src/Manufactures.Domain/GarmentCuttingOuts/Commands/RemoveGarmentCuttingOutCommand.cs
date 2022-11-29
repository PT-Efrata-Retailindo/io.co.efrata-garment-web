using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.Commands
{
    public class RemoveGarmentCuttingOutCommand : ICommand<GarmentCuttingOut>
    {
        public RemoveGarmentCuttingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}