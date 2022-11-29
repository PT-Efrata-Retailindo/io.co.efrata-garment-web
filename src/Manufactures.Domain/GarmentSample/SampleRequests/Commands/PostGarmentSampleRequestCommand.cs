using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.Commands
{
    public class PostGarmentSampleRequestCommand : ICommand<int>
    {
        public PostGarmentSampleRequestCommand(List<string> id, bool posted)
        {
            Identities = id;
            Posted = posted;
        }

        public List<string> Identities { get; private set; }
        public bool Posted { get; private set; }
    }
}
