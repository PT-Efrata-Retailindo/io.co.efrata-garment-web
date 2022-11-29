using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.Commands
{
    public class RemoveGarmentSampleRequestCommand : ICommand<GarmentSampleRequest>
    {
        public RemoveGarmentSampleRequestCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}