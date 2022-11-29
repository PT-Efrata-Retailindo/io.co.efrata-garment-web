using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories
{
    public interface ISubconInvoicePackingListRepository : IAggregateRepository< SubconInvoicePackingList, SubconInvoicePackingListReadModel>
    {
        IQueryable<SubconInvoicePackingListReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
