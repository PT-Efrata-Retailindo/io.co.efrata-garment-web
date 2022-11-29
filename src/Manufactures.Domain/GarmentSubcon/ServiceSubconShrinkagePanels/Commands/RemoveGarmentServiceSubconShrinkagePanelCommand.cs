using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands
{
    public class RemoveGarmentServiceSubconShrinkagePanelCommand : ICommand<GarmentServiceSubconShrinkagePanel>
    {
        public RemoveGarmentServiceSubconShrinkagePanelCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
