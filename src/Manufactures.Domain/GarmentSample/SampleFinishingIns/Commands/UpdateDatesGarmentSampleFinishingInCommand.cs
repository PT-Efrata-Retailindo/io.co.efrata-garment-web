using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands
{
    public class UpdateDatesGarmentSampleFinishingInCommand : ICommand<int>
    {
        public UpdateDatesGarmentSampleFinishingInCommand(List<string> ids, DateTimeOffset date, string subconType)
        {
            Identities = ids;
            Date = date;
            SubconType = subconType;
        }

        public List<string> Identities { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public string SubconType { get; private set; }
    }
}
