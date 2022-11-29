using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconShrinkagePanels.Repositories
{
    public class ShrinkagePanelItemRepository : AggregateRepostory<GarmentServiceSubconShrinkagePanelItem, GarmentServiceSubconShrinkagePanelItemReadModel>, IGarmentServiceSubconShrinkagePanelItemRepository
    {
        IQueryable<GarmentServiceSubconShrinkagePanelItemReadModel> IGarmentServiceSubconShrinkagePanelItemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconShrinkagePanelItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "UnitExpenditureNo"
            };

            data = QueryHelper<GarmentServiceSubconShrinkagePanelItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconShrinkagePanelItemReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSubconShrinkagePanelItem Map(GarmentServiceSubconShrinkagePanelItemReadModel readModel)
        {
            return new GarmentServiceSubconShrinkagePanelItem(readModel);
        }
    }
}
