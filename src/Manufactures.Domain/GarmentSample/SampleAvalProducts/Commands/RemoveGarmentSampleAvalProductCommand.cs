using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands
{
    public class RemoveGarmentSampleAvalProductCommand : ICommand<GarmentSampleAvalProduct>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}
