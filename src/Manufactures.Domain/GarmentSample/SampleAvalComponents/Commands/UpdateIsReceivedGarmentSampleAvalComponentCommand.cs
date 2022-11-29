using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands
{
    public class UpdateIsReceivedGarmentSampleAvalComponentCommand : ICommand<bool>
    {
        public UpdateIsReceivedGarmentSampleAvalComponentCommand(string id, bool isReceived)
        {
            Identities = id;
            IsReceived = isReceived;
        }

        public string Identities { get; private set; }
        public bool IsReceived { get; private set; }
    }
}
