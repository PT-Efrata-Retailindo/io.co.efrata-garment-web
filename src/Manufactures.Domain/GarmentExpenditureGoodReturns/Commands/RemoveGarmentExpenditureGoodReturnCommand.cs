using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoodReturns.Commands
{
    public class RemoveGarmentExpenditureGoodReturnCommand : ICommand<GarmentExpenditureGoodReturn>
    {
        public RemoveGarmentExpenditureGoodReturnCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
