using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands
{
    public class RemoveGarmentServiceSubconFabricWashCommand : ICommand<GarmentServiceSubconFabricWash>
    {
        public RemoveGarmentServiceSubconFabricWashCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
