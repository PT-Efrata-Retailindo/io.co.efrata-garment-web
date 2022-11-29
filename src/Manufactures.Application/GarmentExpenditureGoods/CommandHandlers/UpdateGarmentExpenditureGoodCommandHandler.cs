using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentExpenditureGoods.CommandHandlers
{
    public class UpdateGarmentExpenditureGoodCommandHandler : ICommandHandler<UpdateGarmentExpenditureGoodCommand, GarmentExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;
		private readonly IGarmentExpenditureGoodInvoiceRelationRepository _garmentExpenditureGoodInvoiceRelationRepository;

		public UpdateGarmentExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
			_garmentExpenditureGoodInvoiceRelationRepository = storage.GetRepository<IGarmentExpenditureGoodInvoiceRelationRepository>();

		}

		public async Task<GarmentExpenditureGood> Handle(UpdateGarmentExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var ExpenditureGood = _garmentExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentExpenditureGood(o)).Single();
			var InvoiceRelation = _garmentExpenditureGoodInvoiceRelationRepository.Query.Where(o => o.ExpenditureGoodId == ExpenditureGood.Identity).Select(o => new GarmentExpenditureGoodInvoiceRelation(o)).Single();


			_garmentExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == ExpenditureGood.Identity).ForEach(async expenditureItem =>
            {
                await _garmentExpenditureGoodItemRepository.Update(expenditureItem);
            });
            ExpenditureGood.SetCarton(request.Carton);
            ExpenditureGood.SetExpenditureDate(request.ExpenditureDate);
            ExpenditureGood.SetPackingListId(request.PackingListId);
            ExpenditureGood.SetInvoice(request.Invoice);
            ExpenditureGood.SetIsReceived(request.IsReceived);
            ExpenditureGood.Modify();
            await _garmentExpenditureGoodRepository.Update(ExpenditureGood);
			if (InvoiceRelation != null)
			{
				InvoiceRelation.SetInvoiceId(request.InvoiceId);
				InvoiceRelation.SetInvoiceNo(request.Invoice);
				InvoiceRelation.SetPackingListId(request.PackingListId);
				InvoiceRelation.Modify();
				await _garmentExpenditureGoodInvoiceRelationRepository.Update(InvoiceRelation);
			}
            _storage.Save();

            return ExpenditureGood;
        }
    }
}
