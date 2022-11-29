using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalProducts.CommandHandlers
{
    public class UpdateIsReceivedGarmentSampleAvalProductCommandHandler : ICommandHandler<UpdateIsReceivedGarmentSampleAvalProductCommand, bool>
    {
        private readonly IGarmentSampleAvalProductRepository _garmentSampleAvalProductRepository;
        private readonly IGarmentSampleAvalProductItemRepository _garmentSampleAvalProductItemRepository;
        private readonly IStorage _storage;

        public UpdateIsReceivedGarmentSampleAvalProductCommandHandler(IStorage storage)
        {
            _garmentSampleAvalProductRepository = storage.GetRepository<IGarmentSampleAvalProductRepository>();
            _garmentSampleAvalProductItemRepository = storage.GetRepository<IGarmentSampleAvalProductItemRepository>();
            _storage = storage;
        }

        public async Task<bool> Handle(UpdateIsReceivedGarmentSampleAvalProductCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var SampleAvalProductItems = _garmentSampleAvalProductItemRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleAvalProductItem(a)).ToList();

            foreach (var item in SampleAvalProductItems)
            {
                item.SetIsReceived(request.IsReceived);
                item.SetDeleted();
                await _garmentSampleAvalProductItemRepository.Update(item);
            }
            _storage.Save();

            return request.IsReceived;
        }
    }
}
