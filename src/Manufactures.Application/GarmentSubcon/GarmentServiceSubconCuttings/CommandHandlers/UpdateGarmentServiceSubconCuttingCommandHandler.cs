using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers
{
    public class UpdateGarmentServiceSubconCuttingCommandHandler : ICommandHandler<UpdateGarmentServiceSubconCuttingCommand, GarmentServiceSubconCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconCuttingRepository _garmentServiceSubconCuttingRepository;
        private readonly IGarmentServiceSubconCuttingItemRepository _garmentServiceSubconCuttingItemRepository;
        private readonly IGarmentServiceSubconCuttingDetailRepository _garmentServiceSubconCuttingDetailRepository;
        private readonly IGarmentServiceSubconCuttingSizeRepository _garmentServiceSubconCuttingSizeRepository;

        public UpdateGarmentServiceSubconCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentServiceSubconCuttingItemRepository = storage.GetRepository<IGarmentServiceSubconCuttingItemRepository>();
            _garmentServiceSubconCuttingDetailRepository = storage.GetRepository<IGarmentServiceSubconCuttingDetailRepository>();
            _garmentServiceSubconCuttingSizeRepository = storage.GetRepository<IGarmentServiceSubconCuttingSizeRepository>();
        }

        public async Task<GarmentServiceSubconCutting> Handle(UpdateGarmentServiceSubconCuttingCommand request, CancellationToken cancellationToken)
        {
            var subconCutting = _garmentServiceSubconCuttingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconCutting(o)).Single();
            
            //_garmentServiceSubconCuttingItemRepository.Find(o => o.ServiceSubconCuttingId == subconCutting.Identity).ForEach(async subconCuttingItem =>
            //{
            //    var item = request.Items.Where(o => o.Id == subconCuttingItem.Identity).SingleOrDefault();

            //    if (item==null)
            //    {
            //        _garmentServiceSubconCuttingDetailRepository.Find(i => i.ServiceSubconCuttingItemId == subconCuttingItem.Identity).ForEach(async subconDetail =>
            //        {
            //            subconDetail.Remove();
            //            await _garmentServiceSubconCuttingDetailRepository.Update(subconDetail);
            //        });
            //        subconCuttingItem.Remove();

            //    }
            //    else
            //    {
            //        _garmentServiceSubconCuttingDetailRepository.Find(i => i.ServiceSubconCuttingItemId == subconCuttingItem.Identity).ForEach(async subconDetail =>
            //        {
            //            var detail = item.Details.Where(o => o.Id == subconDetail.Identity).Single();
            //            if (!detail.IsSave)
            //            {
            //                subconDetail.Remove();
            //            }
            //            else
            //            {
            //                subconDetail.SetQuantity(detail.Quantity);
            //                subconDetail.Modify();
            //            }
            //            await _garmentServiceSubconCuttingDetailRepository.Update(subconDetail);
            //        });
            //        subconCuttingItem.Modify();
            //    }


            //    await _garmentServiceSubconCuttingItemRepository.Update(subconCuttingItem);
            //});

           

            subconCutting.SetDate(request.SubconDate.GetValueOrDefault());
            subconCutting.SetBuyerId(new BuyerId(request.Buyer.Id));
            subconCutting.SetBuyerCode(request.Buyer.Code);
            subconCutting.SetBuyerName(request.Buyer.Name);
            subconCutting.SetUomId(new UomId(request.Uom.Id));
            subconCutting.SetUomUnit(request.Uom.Unit);
            subconCutting.SetQtyPacking(request.QtyPacking);
            subconCutting.Modify();
            await _garmentServiceSubconCuttingRepository.Update(subconCutting);

            _storage.Save();

            return subconCutting;
        }
    }
}