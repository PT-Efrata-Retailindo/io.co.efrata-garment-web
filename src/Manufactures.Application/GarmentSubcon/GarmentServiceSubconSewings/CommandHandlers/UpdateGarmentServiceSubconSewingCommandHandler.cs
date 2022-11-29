using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers
{
    public class UpdateGarmentServiceSubconSewingCommandHandler : ICommandHandler<UpdateGarmentServiceSubconSewingCommand, GarmentServiceSubconSewing>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;
        private readonly IGarmentServiceSubconSewingItemRepository _garmentServiceSubconSewingItemRepository;

        public UpdateGarmentServiceSubconSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconSewingRepository = _storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconSewingItemRepository = _storage.GetRepository<IGarmentServiceSubconSewingItemRepository>();
        }

        public async Task<GarmentServiceSubconSewing> Handle(UpdateGarmentServiceSubconSewingCommand request, CancellationToken cancellationToken)
        {
            var serviceSubconSewing = _garmentServiceSubconSewingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconSewing(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            //_garmentServiceSubconSewingItemRepository.Find(o => o.ServiceSubconSewingId == serviceSubconSewing.Identity).ForEach(async serviceSubconSewingItem =>
            //{
            //    var item = request.Items.Where(o => o.Id == serviceSubconSewingItem.Identity).Single();

            //    var diffSewInQuantity = item.IsSave ? (serviceSubconSewingItem.Quantity - (request.IsDifferentSize ? item.TotalQuantity : item.Quantity)) : serviceSubconSewingItem.Quantity;

            //    if (sewInItemToBeUpdated.ContainsKey(serviceSubconSewingItem.SewingInItemId))
            //    {
            //        sewInItemToBeUpdated[serviceSubconSewingItem.SewingInItemId] += diffSewInQuantity;
            //    }
            //    else
            //    {
            //        sewInItemToBeUpdated.Add(serviceSubconSewingItem.SewingInItemId, diffSewInQuantity);
            //    }

            //    if (!item.IsSave)
            //    {
            //        item.Quantity = 0;

            //        serviceSubconSewingItem.Remove();

            //    }
            //    else
            //    {
            //        if (request.IsDifferentSize)
            //        {
            //            serviceSubconSewingItem.SetQuantity(item.TotalQuantity);
            //        }
            //        else
            //        {
            //            serviceSubconSewingItem.SetQuantity(item.Quantity);
            //        }
            //        serviceSubconSewingItem.Modify();
            //    }


            //    await _garmentServiceSubconSewingItemRepository.Update(serviceSubconSewingItem);
            //});

            serviceSubconSewing.SetDate(request.ServiceSubconSewingDate.GetValueOrDefault());
            serviceSubconSewing.SetBuyerId(new BuyerId(request.Buyer.Id));
            serviceSubconSewing.SetBuyerCode(request.Buyer.Code);
            serviceSubconSewing.SetBuyerName(request.Buyer.Name);
            serviceSubconSewing.SetQtyPacking(request.QtyPacking);
            serviceSubconSewing.SetUomUnit(request.UomUnit);
            serviceSubconSewing.Modify();
            await _garmentServiceSubconSewingRepository.Update(serviceSubconSewing);

            _storage.Save();

            return serviceSubconSewing;
        }
    }
}
