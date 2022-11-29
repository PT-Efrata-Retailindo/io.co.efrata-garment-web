using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts.Commands
{
    public class RemoveGarmentSubconContractCommand : ICommand<GarmentSubconContract>
    {
        public RemoveGarmentSubconContractCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
