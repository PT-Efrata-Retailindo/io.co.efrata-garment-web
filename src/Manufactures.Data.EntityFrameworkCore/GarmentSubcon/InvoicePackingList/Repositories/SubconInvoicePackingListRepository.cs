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
    public class SubconInvoicePackingListRepository : AggregateRepostory<SubconInvoicePackingList, SubconInvoicePackingListReadModel>, ISubconInvoicePackingListRepository
    {
        public IQueryable<SubconInvoicePackingListReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<SubconInvoicePackingListReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ContractNo",
                "InvoiceNo",
                "SupplierName",
                "Remark",
                "BCType",
            };

            data = QueryHelper<SubconInvoicePackingListReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<SubconInvoicePackingListReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override SubconInvoicePackingList Map(SubconInvoicePackingListReadModel readModel)
        {
            return new SubconInvoicePackingList(readModel);
        }
    }
}
