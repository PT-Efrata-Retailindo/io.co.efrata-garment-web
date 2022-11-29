using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsDynamic
{
    public class GetGarmentSampleSewingOutsDynamicQueryHandler : IQueryHandler<GetGarmentSampleSewingOutsDynamicQuery, GarmentSampleSewingOutsDynamicViewModel>
    {
        private readonly IGarmentSampleSewingOutRepository _garmentSampleSewingOutRepository;

        public GetGarmentSampleSewingOutsDynamicQueryHandler(IStorage storage)
        {
            _garmentSampleSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
        }

        public async Task<GarmentSampleSewingOutsDynamicViewModel> Handle(GetGarmentSampleSewingOutsDynamicQuery request, CancellationToken cancellationToken)
        {
            var query = _garmentSampleSewingOutRepository.ReadDynamic(request.order, request.search, request.select, request.keyword, request.filter);

            var total = query.Count();
            var data = query.Skip((request.page - 1) * request.size).Take(request.size).ToDynamicList();

            await Task.Yield();

            return new GarmentSampleSewingOutsDynamicViewModel(total, data);
        }
    }
}
