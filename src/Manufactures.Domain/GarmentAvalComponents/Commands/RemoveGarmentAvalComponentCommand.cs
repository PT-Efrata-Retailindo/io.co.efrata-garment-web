using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalComponents.Commands
{
    public class RemoveGarmentAvalComponentCommand : ICommand<GarmentAvalComponent>
    {
        public RemoveGarmentAvalComponentCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
