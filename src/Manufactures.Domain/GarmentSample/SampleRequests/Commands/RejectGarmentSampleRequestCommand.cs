using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.Commands
{
    public class RejectGarmentSampleRequestCommand : ICommand<GarmentSampleRequest>
    {
        public Guid Identity { get; set; }
        public bool IsRejected { get; set; }
        public DateTimeOffset RejectedDate { get; set; }
        public string RejectedBy { get; set; }
        public string RejectedReason { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
}
