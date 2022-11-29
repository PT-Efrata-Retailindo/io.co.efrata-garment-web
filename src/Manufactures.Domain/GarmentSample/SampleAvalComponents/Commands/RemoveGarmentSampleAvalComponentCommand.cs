using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands
{
    public class RemoveGarmentSampleAvalComponentCommand : ICommand<GarmentSampleAvalComponent>
    {
        public RemoveGarmentSampleAvalComponentCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
