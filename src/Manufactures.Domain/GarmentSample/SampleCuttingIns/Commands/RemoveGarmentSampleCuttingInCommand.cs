using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands
{
    public class RemoveGarmentSampleCuttingInCommand : ICommand<GarmentSampleCuttingIn>
    {
        public RemoveGarmentSampleCuttingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
