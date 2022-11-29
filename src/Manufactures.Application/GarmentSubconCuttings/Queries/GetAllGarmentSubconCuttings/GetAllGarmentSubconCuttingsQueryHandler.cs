using ExtCore.Data.Abstractions;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubconCuttings.Queries.GetAllGarmentSubconCuttings
{
    public class GetAllGarmentSubconCuttingsQueryHandler : IQueryHandler<GetAllGarmentSubconCuttingsQuery, GetGarmentSubconCuttingListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingRepository _garmentSubconCuttingRepository;

        public GetAllGarmentSubconCuttingsQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCuttingRepository = storage.GetRepository<IGarmentSubconCuttingRepository>();
        }

        public async Task<GetGarmentSubconCuttingListViewModel> Handle(GetAllGarmentSubconCuttingsQuery request, CancellationToken cancellationToken)
        {
            var Query = _garmentSubconCuttingRepository.Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.filter);
            Query = QueryHelper<GarmentSubconCuttingReadModel>.Filter(Query, FilterDictionary);

            List<string> SearchAttributes = new List<string> { "RONo" };
            Query = QueryHelper<GarmentSubconCuttingReadModel>.Search(Query, SearchAttributes, request.keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.order);
            Query = OrderDictionary.Count == 0 ? Query.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconCuttingReadModel>.Order(Query, OrderDictionary);

            int total = Query.Count();
            Query = Query.Skip((request.page - 1) * request.size).Take(request.size);

            List<GarmentSubconCuttingDto> garmentSubconCuttingDtos = _garmentSubconCuttingRepository.Find(Query)
                .Select(s => new GarmentSubconCuttingDto(s))
                .ToList();

            await Task.Yield();

            return new GetGarmentSubconCuttingListViewModel
            {
                data = garmentSubconCuttingDtos,
                total = total
            };
        }
    }
}
