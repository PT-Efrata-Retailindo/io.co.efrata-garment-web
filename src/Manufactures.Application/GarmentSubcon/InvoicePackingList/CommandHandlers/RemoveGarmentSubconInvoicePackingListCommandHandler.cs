using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Manufactures.Application.GarmentSubcon.InvoicePackingList.CommandHandlers
{
    public class RemoveGarmentSubconInvoicePackingListCommandHandler : ICommandHandler<RemoveGarmentSubconInvoicePackingListCommand, SubconInvoicePackingList>
    {
        private readonly IStorage _storage;
        private readonly ISubconInvoicePackingListRepository _subconInvoicePackingListRepository;
        private readonly ISubconInvoicePackingListItemRepository _subconInvoicePackingListItemRepository;

        public RemoveGarmentSubconInvoicePackingListCommandHandler(IStorage storage)
        {
            _storage = storage;
            _subconInvoicePackingListRepository = storage.GetRepository<ISubconInvoicePackingListRepository>();
            _subconInvoicePackingListItemRepository = storage.GetRepository<ISubconInvoicePackingListItemRepository>();
        }

        public async Task<SubconInvoicePackingList> Handle(RemoveGarmentSubconInvoicePackingListCommand request, CancellationToken cancellationToken)
        {
            var subconContract = _subconInvoicePackingListRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new SubconInvoicePackingList(o)).Single();
            _subconInvoicePackingListItemRepository.Find(o => o.InvoicePackingListId == subconContract.Identity).ForEach(async subconContractItem =>
            {
                subconContractItem.Remove();

                await _subconInvoicePackingListItemRepository.Update(subconContractItem);
            });

            subconContract.Remove();
            await _subconInvoicePackingListRepository.Update(subconContract);

            _storage.Save();

            return subconContract;
        }
    }
}
