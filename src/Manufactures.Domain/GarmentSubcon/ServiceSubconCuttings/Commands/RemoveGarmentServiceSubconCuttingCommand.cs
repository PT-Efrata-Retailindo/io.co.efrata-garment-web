using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands
{
    public class RemoveGarmentServiceSubconCuttingCommand : ICommand<GarmentServiceSubconCutting>
    {
        public RemoveGarmentServiceSubconCuttingCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}