using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentTradingFinishingIns.Commands
{
    public class RemoveGarmentTradingFinishingInCommand : ICommand<GarmentFinishingIn>
    {
        public RemoveGarmentTradingFinishingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
