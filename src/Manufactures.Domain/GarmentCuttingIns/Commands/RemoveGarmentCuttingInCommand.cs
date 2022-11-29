using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns.Commands
{
    public class RemoveGarmentCuttingInCommand : ICommand<GarmentCuttingIn>
    {
        public RemoveGarmentCuttingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
