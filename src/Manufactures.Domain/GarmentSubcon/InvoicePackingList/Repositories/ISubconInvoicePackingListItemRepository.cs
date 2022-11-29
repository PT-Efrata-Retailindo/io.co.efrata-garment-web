using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories
{
    public interface ISubconInvoicePackingListItemRepository : IAggregateRepository<SubconInvoicePackingListItem, SubconInvoicePackingListItemReadModel>
    {
    }
}
