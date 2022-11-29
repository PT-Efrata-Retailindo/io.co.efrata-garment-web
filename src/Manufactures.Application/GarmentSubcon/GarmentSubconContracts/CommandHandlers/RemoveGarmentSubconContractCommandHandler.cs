using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers
{
    public class RemoveGarmentSubconContractCommandHandler : ICommandHandler<RemoveGarmentSubconContractCommand, GarmentSubconContract>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;
        private readonly IGarmentSubconContractItemRepository _garmentSubconContractItemRepository;

        public RemoveGarmentSubconContractCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            _garmentSubconContractItemRepository = storage.GetRepository<IGarmentSubconContractItemRepository>();
        }

        public async Task<GarmentSubconContract> Handle(RemoveGarmentSubconContractCommand request, CancellationToken cancellationToken)
        {
            var subconContract = _garmentSubconContractRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconContract(o)).Single();
            _garmentSubconContractItemRepository.Find(o => o.SubconContractId == subconContract.Identity).ForEach(async subconContractItem =>
            {
                subconContractItem.Remove();
                
                await _garmentSubconContractItemRepository.Update(subconContractItem);
            });

            subconContract.Remove();
            await _garmentSubconContractRepository.Update(subconContract);

            _storage.Save();

            return subconContract;
        }
    }
}
