using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GarmentDeliveryReturns.Commands
{
    public class RemoveGarmentDeliveryReturnCommand : ICommand<GarmentDeliveryReturn>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}