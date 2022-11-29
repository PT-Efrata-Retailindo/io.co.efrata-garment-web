using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.CustomsOuts.CommandHandlers
{
    public class PlaceGarmentSubconCustomsOutCommandHandler : ICommandHandler<PlaceGarmentSubconCustomsOutCommand, GarmentSubconCustomsOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsOutRepository _garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository _garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;

        public PlaceGarmentSubconCustomsOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            _garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
        }

        public async Task<GarmentSubconCustomsOut> Handle(PlaceGarmentSubconCustomsOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentSubconCustomsOut garmentSubconCustomsOut = new GarmentSubconCustomsOut(
                Guid.NewGuid(),
                request.CustomsOutNo,
                request.CustomsOutDate,
                request.CustomsOutType,
                request.SubconType,
                request.SubconContractId,
                request.SubconContractNo,
                new SupplierId(request.Supplier.Id),
                request.Supplier.Code,
                request.Supplier.Name,
                request.Remark,
                request.SubconCategory
            );

            foreach (var item in request.Items)
            {
                GarmentSubconCustomsOutItem garmentSubconCustomsOutItem = new GarmentSubconCustomsOutItem(
                    Guid.NewGuid(),
                    garmentSubconCustomsOut.Identity,
                    item.SubconDLOutNo,
                    item.SubconDLOutId,
                    item.Quantity
                );

                var subconDLOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(x => x.Identity == item.SubconDLOutId).Select(s => new GarmentSubconDeliveryLetterOut(s)).Single();
                subconDLOut.SetIsUsed(true);
                subconDLOut.Modify();
                await _garmentSubconDeliveryLetterOutRepository.Update(subconDLOut);

                await _garmentSubconCustomsOutItemRepository.Update(garmentSubconCustomsOutItem);
            }

            await _garmentSubconCustomsOutRepository.Update(garmentSubconCustomsOut);

            _storage.Save();

            return garmentSubconCustomsOut;
        }
    }
}
