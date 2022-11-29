using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconFinishingIns.Commands
{
    public class RemoveGarmentSubconFinishingInCommand : ICommand<GarmentFinishingIn>
    {
        public RemoveGarmentSubconFinishingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
