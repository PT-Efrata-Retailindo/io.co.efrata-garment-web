using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands
{
    public class RemoveGarmentSampleFinishingOutCommand : ICommand<GarmentSampleFinishingOut>
    {
        public RemoveGarmentSampleFinishingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
