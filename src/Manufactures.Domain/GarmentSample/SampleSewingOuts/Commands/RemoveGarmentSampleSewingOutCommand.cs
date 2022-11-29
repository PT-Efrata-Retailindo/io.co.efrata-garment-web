using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands
{
    public class RemoveGarmentSampleSewingOutCommand : ICommand<GarmentSampleSewingOut>
    {
        public RemoveGarmentSampleSewingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
