using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Manufactures.Application.GarmentSubcon.InvoicePackingList.CommandHandlers
{
    public class UpdateGarmentSubconInvoicePackingListCommandHandler : ICommandHandler<UpdateGarmentSubconInvoicePackingListCommand, SubconInvoicePackingList>
    {
        private readonly IStorage _storage;
        private readonly ISubconInvoicePackingListRepository _subconInvoicePackingListRepository;
        private readonly ISubconInvoicePackingListItemRepository _subconInvoicePackingListItemRepository;

        public UpdateGarmentSubconInvoicePackingListCommandHandler(IStorage storage)
        {
            _storage = storage;
            _subconInvoicePackingListRepository = storage.GetRepository<ISubconInvoicePackingListRepository>();
            _subconInvoicePackingListItemRepository = storage.GetRepository<ISubconInvoicePackingListItemRepository>();
        }
        public async Task<SubconInvoicePackingList> Handle(UpdateGarmentSubconInvoicePackingListCommand request, CancellationToken cancellationToken)
        {
            var invoicePackingList = _subconInvoicePackingListRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new SubconInvoicePackingList(o)).Single();

            _subconInvoicePackingListItemRepository.Find(o => o.InvoicePackingListId == invoicePackingList.Identity).ForEach(async invoicePackingListItem =>
            {
                var item = request.Items.Where(o => o.Id == invoicePackingListItem.Identity).Single();

                invoicePackingListItem.SetDLNo(item.DLNo);

                invoicePackingListItem.Modify();

                await _subconInvoicePackingListItemRepository.Update(invoicePackingListItem);
            });

            invoicePackingList.SetGW(request.GW);
            invoicePackingList.SetNW(request.NW);
            invoicePackingList.SetRemark(request.Remark);


            invoicePackingList.Modify();

            await _subconInvoicePackingListRepository.Update(invoicePackingList);

            _storage.Save();

            return invoicePackingList;

        }
    }
}
