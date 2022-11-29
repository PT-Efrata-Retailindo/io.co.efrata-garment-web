using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentLoadings.Commands
{
    public class RemoveGarmentLoadingCommand : ICommand<GarmentLoading>
    {
        public RemoveGarmentLoadingCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }

}
