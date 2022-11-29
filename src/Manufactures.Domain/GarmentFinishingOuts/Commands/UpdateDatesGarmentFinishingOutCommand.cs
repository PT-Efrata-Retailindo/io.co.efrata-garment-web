using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts.Commands
{
    public class UpdateDatesGarmentFinishingOutCommand : ICommand<int>
    {
        public UpdateDatesGarmentFinishingOutCommand(List<string> ids, DateTimeOffset date)
        {
            Identities = ids;
            Date = date;
        }

        public List<string> Identities { get; private set; }
        public DateTimeOffset Date { get; private set; }
    }
}
