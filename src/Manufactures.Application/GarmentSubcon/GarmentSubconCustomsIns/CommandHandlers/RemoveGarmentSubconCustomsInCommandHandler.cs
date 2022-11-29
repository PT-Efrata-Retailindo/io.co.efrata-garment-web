using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconCustomsIns.CommandHandlers
{
    public class RemoveGarmentSubconCustomsInCommandHandler : ICommandHandler<RemoveGarmentSubconCustomsInCommand, GarmentSubconCustomsIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsInRepository _garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository _garmentSubconCustomsInItemRepository;

        public RemoveGarmentSubconCustomsInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            _garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
        }

        public async Task<GarmentSubconCustomsIn> Handle(RemoveGarmentSubconCustomsInCommand request, CancellationToken cancellationToken)
        {
            var subconCustomsIn = _garmentSubconCustomsInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCustomsIn(o)).Single();

            _garmentSubconCustomsInItemRepository.Find(o => o.SubconCustomsInId == subconCustomsIn.Identity).ForEach(async subconCustomsInItem =>
            {
                subconCustomsInItem.Remove();
                await _garmentSubconCustomsInItemRepository.Update(subconCustomsInItem);
            });

            subconCustomsIn.Remove();
            await _garmentSubconCustomsInRepository.Update(subconCustomsIn);

            _storage.Save();

            return subconCustomsIn;
        }
    }
}
