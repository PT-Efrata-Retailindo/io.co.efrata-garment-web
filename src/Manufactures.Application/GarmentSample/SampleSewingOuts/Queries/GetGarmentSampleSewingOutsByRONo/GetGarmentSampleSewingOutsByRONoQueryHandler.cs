using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsByRONo
{
    public class GetGarmentSampleSewingOutsByRONoQueryHandler : IQueryHandler<GetGarmentSampleSewingOutsByRONoQuery, GarmentSampleSewingOutsByRONoViewModel>
    {
        private readonly IGarmentSampleSewingOutRepository _garmentSampleSewingOutRepository;

        public GetGarmentSampleSewingOutsByRONoQueryHandler(IStorage storage)
        {
            _garmentSampleSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
        }

        public async Task<GarmentSampleSewingOutsByRONoViewModel> Handle(GetGarmentSampleSewingOutsByRONoQuery request, CancellationToken cancellationToken)
        {
            var query = _garmentSampleSewingOutRepository.Read(1, int.MaxValue, "{}", "", request.filter);

            if (!string.IsNullOrWhiteSpace(request.keyword))
            {
                query = query.Where(o => o.RONo.Contains(request.keyword));
            }

            var data = query.Select(o => new GarmentSampleSewingOutByRONoDto
            {
                RONo = o.RONo,
                Article = o.Article,
                Comodity = new GarmentComodity
                {
                    Id = o.ComodityId,
                    Code = o.ComodityCode,
                    Name = o.ComodityName
                },
            }).Distinct().ToList();

            await Task.Yield();

            return new GarmentSampleSewingOutsByRONoViewModel(data);
        }
    }
}
