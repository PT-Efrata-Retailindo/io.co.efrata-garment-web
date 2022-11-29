using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Commands;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentComodityPrices.CommandHandlers
{
    public class PlaceGarmentComodityPriceCommandHandler : ICommandHandler<PlaceGarmentComodityPriceCommand, List<GarmentComodityPrice>>
    {
        private readonly IStorage _storage;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentComodityPriceCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<List<GarmentComodityPrice>> Handle(PlaceGarmentComodityPriceCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();
            List<GarmentComodityPrice> List = new List<GarmentComodityPrice>();

            foreach (var item in request.Items)
            {
                GarmentComodityPrice garmentComodityPrice = new GarmentComodityPrice(
                    Guid.NewGuid(),
                    true,
                    request.Date,
                    new UnitDepartmentId(request.Unit.Id),
                    request.Unit.Code,
                    request.Unit.Name,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name,
                    item.Price
                );
                List.Add(garmentComodityPrice);
                await _garmentComodityPriceRepository.Update(garmentComodityPrice);
            }

            _storage.Save();

            return List;
        }
    }
}
