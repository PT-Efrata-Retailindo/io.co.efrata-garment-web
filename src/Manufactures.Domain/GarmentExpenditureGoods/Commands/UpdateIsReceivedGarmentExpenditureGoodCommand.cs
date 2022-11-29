using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.Commands
{
    public class UpdateIsReceivedGarmentExpenditureGoodCommand : ICommand<GarmentExpenditureGood>
    {
        public UpdateIsReceivedGarmentExpenditureGoodCommand(Guid id, bool isReceived)
        {
            Identity = id;
            IsReceived =isReceived;
        }

        public Guid Identity { get; private set; }
        public bool IsReceived { get; private set; }
    }
}
