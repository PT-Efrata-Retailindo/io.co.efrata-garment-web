using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands
{
    public class RemoveGarmentSampleCuttingOutCommand : ICommand<GarmentSampleCuttingOut>
    {
        public RemoveGarmentSampleCuttingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
