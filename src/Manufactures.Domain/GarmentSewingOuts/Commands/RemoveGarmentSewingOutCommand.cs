using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.Commands
{
    public class RemoveGarmentSewingOutCommand : ICommand<GarmentSewingOut>
    {
        public RemoveGarmentSewingOutCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
