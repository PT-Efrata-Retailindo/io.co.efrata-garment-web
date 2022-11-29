using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands
{
    public class RemoveGarmentSampleDeliveryReturnCommand : ICommand<GarmentSampleDeliveryReturn>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}
