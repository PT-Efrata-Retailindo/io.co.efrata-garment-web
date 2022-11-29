using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentLoadings.Commands
{
    public class UpdateDatesGarmentLoadingCommand : ICommand<int>
    {
        public UpdateDatesGarmentLoadingCommand(List<string> ids, DateTimeOffset date)
        {
            Identities = ids;
            Date = date;
        }

        public List<string> Identities { get; private set; }
        public DateTimeOffset Date { get; private set; }
    }
}
