using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalComponents.CommandHandlers
{
    public class UpdateIsReceivedGarmentAvalComponentCommandHandler : ICommandHandler<UpdateIsReceivedGarmentAvalComponentCommand, bool>
    {
        private readonly IGarmentAvalComponentRepository _garmentAvalComponentRepository;
        private readonly IStorage _storage;

        public UpdateIsReceivedGarmentAvalComponentCommandHandler(IStorage storage)
        {
            _garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
            _storage = storage;
        }

        public async Task<bool> Handle(UpdateIsReceivedGarmentAvalComponentCommand request, CancellationToken cancellationToken)
        {
            Guid guid = Guid.Parse(request.Identities);
            var AvalComponent = _garmentAvalComponentRepository.Query.Where(a => a.Identity == guid).Select(a => new GarmentAvalComponent(a)).FirstOrDefault();
            AvalComponent.SetIsReceived(request.IsReceived);
            AvalComponent.SetDeleted();
            await _garmentAvalComponentRepository.Update(AvalComponent);
            _storage.Save();

            return request.IsReceived;
        }
    }
}
