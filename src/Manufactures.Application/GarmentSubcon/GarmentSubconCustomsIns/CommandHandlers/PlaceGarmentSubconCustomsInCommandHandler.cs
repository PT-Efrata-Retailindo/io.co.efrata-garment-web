using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconCustomsIns.CommandHandlers
{
    public class PlaceGarmentSubconCustomsInCommandHandler : ICommandHandler<PlaceGarmentSubconCustomsInCommand, GarmentSubconCustomsIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsInRepository _garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository _garmentSubconCustomsInItemRepository;

        public PlaceGarmentSubconCustomsInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            _garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
        }

        public async Task<GarmentSubconCustomsIn> Handle(PlaceGarmentSubconCustomsInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentSubconCustomsIn garmentSubconCustomsIn = new GarmentSubconCustomsIn(
                Guid.NewGuid(),
                request.BcNo,
                request.BcDate.GetValueOrDefault(),
                request.BcType,
                request.SubconType,
                request.SubconContractId,
                request.SubconContractNo,
                new SupplierId(request.Supplier.Id),
                request.Supplier.Code,
                request.Supplier.Name,
                request.Remark,
                request.IsUsed,
                request.SubconCategory
            );

            foreach (var item in request.Items)
            {
                GarmentSubconCustomsInItem garmentSubconCustomsInItem = new GarmentSubconCustomsInItem(
                    Guid.NewGuid(),
                    garmentSubconCustomsIn.Identity,
                    new SupplierId(item.Supplier.Id),
                    item.Supplier.Code,
                    item.Supplier.Name,
                    item.DoId,
                    item.DoNo,
                    item.Quantity
                );

                await _garmentSubconCustomsInItemRepository.Update(garmentSubconCustomsInItem);
            }


            await _garmentSubconCustomsInRepository.Update(garmentSubconCustomsIn);

            _storage.Save();

            return garmentSubconCustomsIn;
        }
    }
}
