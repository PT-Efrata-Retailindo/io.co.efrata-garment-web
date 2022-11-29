using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns.Commands
{
    public class RemoveGarmentFinishingInCommand : ICommand<GarmentFinishingIn>
    {
        public RemoveGarmentFinishingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
