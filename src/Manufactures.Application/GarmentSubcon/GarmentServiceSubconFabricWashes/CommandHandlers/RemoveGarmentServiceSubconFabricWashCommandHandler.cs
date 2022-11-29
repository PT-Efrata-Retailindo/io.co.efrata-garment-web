using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.CommandHandlers
{
    public class RemoveGarmentServiceSubconFabricWashCommandHandler : ICommandHandler<RemoveGarmentServiceSubconFabricWashCommand, GarmentServiceSubconFabricWash>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconFabricWashItemRepository _garmentServiceSubconFabricWashItemRepository;
        private readonly IGarmentServiceSubconFabricWashDetailRepository _garmentServiceSubconFabricWashDetailRepository;

        public RemoveGarmentServiceSubconFabricWashCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentServiceSubconFabricWashItemRepository = storage.GetRepository<IGarmentServiceSubconFabricWashItemRepository>();
            _garmentServiceSubconFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSubconFabricWashDetailRepository>();
        }

        public async Task<GarmentServiceSubconFabricWash> Handle(RemoveGarmentServiceSubconFabricWashCommand request, CancellationToken cancellationToken)
        {
            var serviceSubconFabricWash = _garmentServiceSubconFabricWashRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconFabricWash(o)).Single();

            _garmentServiceSubconFabricWashItemRepository.Find(o => o.ServiceSubconFabricWashId == serviceSubconFabricWash.Identity).ForEach(async serviceSubconFabricWashItem =>
            {
                _garmentServiceSubconFabricWashDetailRepository.Find(i => i.ServiceSubconFabricWashItemId == serviceSubconFabricWashItem.Identity).ForEach(async serviceSubconFabricWashDetail =>
                {
                    serviceSubconFabricWashDetail.Remove();
                    await _garmentServiceSubconFabricWashDetailRepository.Update(serviceSubconFabricWashDetail);
                });
                serviceSubconFabricWashItem.Remove();
                await _garmentServiceSubconFabricWashItemRepository.Update(serviceSubconFabricWashItem);
            });

            serviceSubconFabricWash.Remove();
            await _garmentServiceSubconFabricWashRepository.Update(serviceSubconFabricWash);

            _storage.Save();

            return serviceSubconFabricWash;
        }
    }
}
