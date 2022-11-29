using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.Commands
{
    public class RemoveGarmentExpenditureGoodCommand : ICommand<GarmentExpenditureGood>
    {
        public RemoveGarmentExpenditureGoodCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
