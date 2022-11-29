using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands
{
    public class RemoveGarmentSubconCustomsInCommand : ICommand<GarmentSubconCustomsIn>
    {
        public RemoveGarmentSubconCustomsInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
