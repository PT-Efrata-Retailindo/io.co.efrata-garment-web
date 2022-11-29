using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingIns.Commands
{
    public class RemoveGarmentSewingInCommand : ICommand<GarmentSewingIn>
    {
        public RemoveGarmentSewingInCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}