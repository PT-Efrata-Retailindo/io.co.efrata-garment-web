using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands
{
    public class RemoveGarmentSubconCustomsOutCommand : ICommand<GarmentSubconCustomsOut>
    {
        public RemoveGarmentSubconCustomsOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
