using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GarmentSewingDOs.Commands
{
    public class RemoveGarmentSewingDOCommand : ICommand<GarmentSewingDO>
    {
        public RemoveGarmentSewingDOCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}