using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.Commands
{
    public class RevisedGarmentSampleRequestCommand : ICommand<GarmentSampleRequest>
    {
        public Guid Identity { get; set; }
        public bool IsRevised { get; set; }
        public DateTimeOffset RevisedDate { get; set; }
        public string RevisedBy { get; set; }
        public string RevisedReason { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
}
