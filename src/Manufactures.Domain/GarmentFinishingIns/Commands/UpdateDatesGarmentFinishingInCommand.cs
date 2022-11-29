using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns.Commands
{
    public class UpdateDatesGarmentFinishingInCommand : ICommand<int>
    {
        public UpdateDatesGarmentFinishingInCommand(List<string> ids, DateTimeOffset date, string subconType)
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
