using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.CommandHandlers
{
    public class UpdateIsReceivedGarmentSampleAvalComponentCommandHandler : ICommandHandler<UpdateIsReceivedGarmentSampleAvalComponentCommand, bool>
    {
        private readonly IGarmentSampleAvalComponentRepository _garmentSampleAvalComponentRepository;
        private readonly IStorage _storage;

        public UpdateIsReceivedGarmentSampleAvalComponentCommandHandler(IStorage storage)
        {
            _garmentSampleAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
            _storage = storage;
        }

        public async Task<bool> Handle(UpdateIsReceivedGarmentSampleAvalComponentCommand request, CancellationToken cancellationToken)
        {
            Guid guid = Guid.Parse(request.Identities);
            var SampleAvalComponent = _garmentSampleAvalComponentRepository.Query.Where(a => a.Identity == guid).Select(a => new GarmentSampleAvalComponent(a)).FirstOrDefault();
            SampleAvalComponent.SetIsReceived(request.IsReceived);
            SampleAvalComponent.SetDeleted();
            await _garmentSampleAvalComponentRepository.Update(SampleAvalComponent);
            _storage.Save();

            return request.IsReceived;
        }
    }
}
