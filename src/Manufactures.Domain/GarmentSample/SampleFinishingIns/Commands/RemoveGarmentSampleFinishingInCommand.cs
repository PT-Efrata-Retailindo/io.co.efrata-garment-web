using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands
{
    public class RemoveGarmentSampleFinishingInCommand : ICommand<GarmentSampleFinishingIn>
    {
        public RemoveGarmentSampleFinishingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
