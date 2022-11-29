using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings.Commands
{
    public class RemoveGarmentPreparingCommand : ICommand<GarmentPreparing>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}