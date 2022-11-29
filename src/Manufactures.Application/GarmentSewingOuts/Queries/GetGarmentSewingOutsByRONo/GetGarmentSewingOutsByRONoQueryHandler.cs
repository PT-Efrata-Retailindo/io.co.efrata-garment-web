using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsByRONo
{
    public class GetGarmentSewingOutsByRONoQueryHandler : IQueryHandler<GetGarmentSewingOutsByRONoQuery, GarmentSewingOutsByRONoViewModel>
    {
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;

        public GetGarmentSewingOutsByRONoQueryHandler(IStorage storage)
        {
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
        }

        public async Task<GarmentSewingOutsByRONoViewModel> Handle(GetGarmentSewingOutsByRONoQuery request, CancellationToken cancellationToken)
        {
            var query = _garmentSewingOutRepository.Read(1, int.MaxValue, "{}", "", request.filter);

            if (!string.IsNullOrWhiteSpace(request.keyword))
            {
                query = query.Where(o => o.RONo.Contains(request.keyword));
            }

            var data = query.Select(o => new GarmentSewingOutByRONoDto
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

            return new GarmentSewingOutsByRONoViewModel(data);
        }
    }
}
