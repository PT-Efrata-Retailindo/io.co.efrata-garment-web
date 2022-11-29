using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconContracts.Repositories
{
    public class GarmentSubconContractRepository : AggregateRepostory<GarmentSubconContract, GarmentSubconContractReadModel>, IGarmentSubconContractRepository
    {
        public IQueryable<GarmentSubconContractReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconContractReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ContractNo",
                "SupplierCode",
                "SupplierName",
                "ContractType",
                "AgreementNo",
                "JobType",
                "BPJNo",
                "FinishedGoodType"
            };

            data = QueryHelper<GarmentSubconContractReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconContractReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        

        protected override GarmentSubconContract Map(GarmentSubconContractReadModel readModel)
        {
            return new GarmentSubconContract(readModel);
        }
    }
}
