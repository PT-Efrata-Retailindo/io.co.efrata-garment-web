using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.CustomsOuts.CommandHandlers
{
    public class RemoveGarmentSubconCustomsOutCommandHandler : ICommandHandler<RemoveGarmentSubconCustomsOutCommand, GarmentSubconCustomsOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsOutRepository _garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository _garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;

        public RemoveGarmentSubconCustomsOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            _garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
        }


        public async Task<GarmentSubconCustomsOut> Handle(RemoveGarmentSubconCustomsOutCommand request, CancellationToken cancellationToken)
        {
            var subconCustomsOut = _garmentSubconCustomsOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCustomsOut(o)).Single();

            _garmentSubconCustomsOutItemRepository.Find(o => o.SubconCustomsOutId == subconCustomsOut.Identity).ForEach(async subconCustomsOutItem =>
            {
                subconCustomsOutItem.Remove();

                var subconDLOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(x => x.Identity == subconCustomsOutItem.SubconDLOutId).Select(s => new GarmentSubconDeliveryLetterOut(s)).Single();
                subconDLOut.SetIsUsed(false);
                subconDLOut.Modify();
                await _garmentSubconDeliveryLetterOutRepository.Update(subconDLOut);

                await _garmentSubconCustomsOutItemRepository.Update(subconCustomsOutItem);
            });

            subconCustomsOut.Remove();
            await _garmentSubconCustomsOutRepository.Update(subconCustomsOut);

            _storage.Save();

            return subconCustomsOut;
        }
    }
}
