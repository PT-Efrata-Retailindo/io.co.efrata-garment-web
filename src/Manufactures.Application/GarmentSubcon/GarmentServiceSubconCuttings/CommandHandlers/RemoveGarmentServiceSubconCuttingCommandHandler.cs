using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers
{
    public class RemoveGarmentServiceSubconCuttingCommandHandler : ICommandHandler<RemoveGarmentServiceSubconCuttingCommand, GarmentServiceSubconCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconCuttingRepository _garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository _garmentServiceSubconCuttingItemRepository;
        private readonly IGarmentServiceSubconCuttingDetailRepository _garmentServiceSubconCuttingDetailRepository;
        private readonly IGarmentServiceSubconCuttingSizeRepository _garmentServiceSubconCuttingSizeRepository;

        public RemoveGarmentServiceSubconCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
            _garmentServiceSubconCuttingDetailRepository = storage.GetRepository<IGarmentServiceSubconCuttingDetailRepository>();
            _garmentServiceSubconCuttingSizeRepository = storage.GetRepository<IGarmentServiceSubconCuttingSizeRepository>();
        }

        public async Task<GarmentServiceSubconCutting> Handle(RemoveGarmentServiceSubconCuttingCommand request, CancellationToken cancellationToken)
        {
            var subconCutting = _garmentServiceSubconCuttingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconCutting(o)).Single();

            _garmentServiceSubconCuttingItemRepository.Find(o => o.ServiceSubconCuttingId == subconCutting.Identity).ForEach(async subconCuttingItem =>
            {
                _garmentServiceSubconCuttingDetailRepository.Find(i => i.ServiceSubconCuttingItemId == subconCuttingItem.Identity).ForEach(async subconDetail =>
                    {
                        _garmentServiceSubconCuttingSizeRepository.Find(i => i.ServiceSubconCuttingDetailId == subconDetail.Identity).ForEach(async subconSize =>
                        {
                            subconSize.Remove();
                            await _garmentServiceSubconCuttingSizeRepository.Update(subconSize);
                        });
                        subconDetail.Remove();
                        await _garmentServiceSubconCuttingDetailRepository.Update(subconDetail);
                    });
                subconCuttingItem.Remove();
                await _garmentServiceSubconCuttingItemRepository.Update(subconCuttingItem);
            });

            subconCutting.Remove();
            await _garmentServiceSubconCuttingRepository.Update(subconCutting);

            _storage.Save();

            return subconCutting;
        }
    }
}