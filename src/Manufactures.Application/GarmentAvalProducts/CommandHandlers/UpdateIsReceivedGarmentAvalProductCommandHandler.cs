using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalProducts.CommandHandlers
{
    public class UpdateIsReceivedGarmentAvalProductCommandHandler : ICommandHandler<UpdateIsReceivedGarmentAvalProductCommand, bool>
    {
        private readonly IGarmentAvalProductRepository _garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository _garmentAvalProductItemRepository;
        private readonly IStorage _storage;

        public UpdateIsReceivedGarmentAvalProductCommandHandler(IStorage storage)
        {
            _garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
            _garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            _storage = storage;
        }

        public async Task<bool> Handle(UpdateIsReceivedGarmentAvalProductCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach(var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var AvalProductItems = _garmentAvalProductItemRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentAvalProductItem(a)).ToList();

            foreach(var item in AvalProductItems)
            {
                item.SetIsReceived(request.IsReceived);
                item.SetDeleted();
                await _garmentAvalProductItemRepository.Update(item);
            }
            _storage.Save();

            return request.IsReceived;
        }
    }
}
