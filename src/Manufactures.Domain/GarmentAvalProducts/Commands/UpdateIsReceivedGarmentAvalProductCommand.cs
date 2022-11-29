using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.Commands
{
    public class UpdateIsReceivedGarmentAvalProductCommand : ICommand<bool>
    {
        public UpdateIsReceivedGarmentAvalProductCommand(List<string> ids, bool isReceived)
        {
            Identities = ids;
            IsReceived = isReceived;
        }

        public List<string> Identities { get; private set; }
        public bool IsReceived { get; private set; }
    }
}
