using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAdjustments.Commands
{
    public class RemoveGarmentAdjustmentCommand : ICommand<GarmentAdjustment>
    {
        public RemoveGarmentAdjustmentCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
