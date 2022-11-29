using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.InvoicePackingList.Repositories
{
    public class SubconInvoicePackingListItemRepository : AggregateRepostory<SubconInvoicePackingListItem, SubconInvoicePackingListItemReadModel>, ISubconInvoicePackingListItemRepository
    {
        protected override SubconInvoicePackingListItem Map(SubconInvoicePackingListItemReadModel readModel)
        {
            return new SubconInvoicePackingListItem(readModel);
        }
    }
}
