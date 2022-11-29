using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalComponents.Commands
{
    public class UpdateIsReceivedGarmentAvalComponentCommand : ICommand<bool>
    {
        public UpdateIsReceivedGarmentAvalComponentCommand(string id, bool isReceived)
        {
            Identities = id;
            IsReceived = isReceived;
        }

        public string Identities { get; private set; }
        public bool IsReceived { get; private set; }
    }
}
