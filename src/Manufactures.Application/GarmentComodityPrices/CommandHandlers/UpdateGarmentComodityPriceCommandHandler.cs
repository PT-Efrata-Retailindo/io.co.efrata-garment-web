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
    public class UpdateGarmentComodityPriceCommandHandler : ICommandHandler<UpdateGarmentComodityPriceCommand, List<GarmentComodityPrice>>
    {
        private readonly IStorage _storage;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public UpdateGarmentComodityPriceCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<List<GarmentComodityPrice>> Handle(UpdateGarmentComodityPriceCommand request, CancellationToken cancellationToken)
        {
            List<GarmentComodityPrice> List = new List<GarmentComodityPrice>();
            foreach (var item in request.Items)
            {
                var como = _garmentComodityPriceRepository.Query.Where(o => o.Identity == item.Id).Select(o => new GarmentComodityPrice(o)).Single();

                if (como.Price != item.NewPrice)
                {
                    como.setValid(false);

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
                        item.NewPrice
                    );

                    await _garmentComodityPriceRepository.Update(garmentComodityPrice);
                }
                
                como.Modify();
                await _garmentComodityPriceRepository.Update(como);

                _storage.Save();
                List.Add(como);
                
            }
            return List;
        }
    }
}
