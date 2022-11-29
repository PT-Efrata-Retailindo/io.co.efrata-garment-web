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
    public class GarmentServiceSubconShrinkagePanelRepository : AggregateRepostory<GarmentServiceSubconShrinkagePanel, GarmentServiceSubconShrinkagePanelReadModel>, IGarmentServiceSubconShrinkagePanelRepository
    {
        IQueryable<GarmentServiceSubconShrinkagePanelReadModel> IGarmentServiceSubconShrinkagePanelRepository.Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconShrinkagePanelReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ServiceSubconShrinkagePanelNo",
            };

            data = QueryHelper<GarmentServiceSubconShrinkagePanelReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconShrinkagePanelReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSubconShrinkagePanel Map(GarmentServiceSubconShrinkagePanelReadModel readModel)
        {
            return new GarmentServiceSubconShrinkagePanel(readModel);
        }
    }
}
