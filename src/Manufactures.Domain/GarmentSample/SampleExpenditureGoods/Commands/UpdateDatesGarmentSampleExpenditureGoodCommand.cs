using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands
{
    public class UpdateDatesGarmentSampleExpenditureGoodCommand : ICommand<int>
    {
        public UpdateDatesGarmentSampleExpenditureGoodCommand(List<string> ids, DateTimeOffset date)
        {
            Identities = ids;
            Date = date;
        }

        public List<string> Identities { get; private set; }
        public DateTimeOffset Date { get; private set; }
    }
}
