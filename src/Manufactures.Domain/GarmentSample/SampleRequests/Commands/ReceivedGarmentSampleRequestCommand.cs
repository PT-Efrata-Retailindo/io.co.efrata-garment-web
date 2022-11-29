using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.Commands
{
    public class ReceivedGarmentSampleRequestCommand : ICommand<GarmentSampleRequest>
    {
        public Guid Identity { get; set; }
        public bool IsReceived { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }
        public string ReceivedBy { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
}
