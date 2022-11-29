using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using System;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsDynamic
{
    public class GetGarmentSewingOutsDynamicQueryHandler : IQueryHandler<GetGarmentSewingOutsDynamicQuery, GarmentSewingOutsDynamicViewModel>
    {
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;

        public GetGarmentSewingOutsDynamicQueryHandler(IStorage storage)
        {
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
        }

        public async Task<GarmentSewingOutsDynamicViewModel> Handle(GetGarmentSewingOutsDynamicQuery request, CancellationToken cancellationToken)
        {
            var query = _garmentSewingOutRepository.ReadDynamic(request.order, request.search, request.select, request.keyword, request.filter);

            var total = query.Count();
            var data = query.Skip((request.page - 1) * request.size).Take(request.size).ToDynamicList();

            await Task.Yield();

            return new GarmentSewingOutsDynamicViewModel(total, data);
        }
    }
}
