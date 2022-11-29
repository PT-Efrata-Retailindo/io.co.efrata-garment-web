using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands
{
    public class RemoveGarmentSubconInvoicePackingListCommand : ICommand<SubconInvoicePackingList>
    {
        public RemoveGarmentSubconInvoicePackingListCommand(Guid id)
        {
            Identity = id;
        }

        public Guid Identity { get; private set; }
    }
}
