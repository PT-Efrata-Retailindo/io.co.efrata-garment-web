using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands
{
    public class RemoveGarmentSampleExpenditureGoodCommand : ICommand<GarmentSampleExpenditureGood>
    {
        public RemoveGarmentSampleExpenditureGoodCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}

