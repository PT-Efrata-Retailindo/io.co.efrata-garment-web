using System;
using Infrastructure.Domain.Commands;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands
{
    public class RemoveGarmentServiceSubconSewingCommand : ICommand<GarmentServiceSubconSewing>
    {
        public RemoveGarmentServiceSubconSewingCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
