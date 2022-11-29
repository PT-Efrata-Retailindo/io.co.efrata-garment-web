using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleFinishingOuts.CommandHandlers
{
    public class UpdateGarmentSampleFinishingOutCommandHandler : ICommandHandler<UpdateGarmentSampleFinishingOutCommand, GarmentSampleFinishingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleFinishingOutRepository _GarmentSampleFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository _GarmentSampleFinishingOutItemRepository;
        private readonly IGarmentSampleFinishingOutDetailRepository _GarmentSampleFinishingOutDetailRepository;
        private readonly IGarmentSampleFinishingInItemRepository _GarmentSampleFinishingInItemRepository;

        public UpdateGarmentSampleFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            _GarmentSampleFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            _GarmentSampleFinishingOutDetailRepository = storage.GetRepository<IGarmentSampleFinishingOutDetailRepository>();
            _GarmentSampleFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
        }

        public async Task<GarmentSampleFinishingOut> Handle(UpdateGarmentSampleFinishingOutCommand request, CancellationToken cancellationToken)
        {
            var finishOut = _GarmentSampleFinishingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleFinishingOut(o)).Single();

            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();

            _GarmentSampleFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).ForEach(async finishOutItem =>
            {
                var item = request.Items.Where(o => o.Id == finishOutItem.Identity).Single();

                var diffSewInQuantity = item.IsSave ? (finishOutItem.Quantity - (request.IsDifferentSize ? item.TotalQuantity : item.Quantity)) : finishOutItem.Quantity;

                if (finishingInItemToBeUpdated.ContainsKey(finishOutItem.FinishingInItemId))
                {
                    finishingInItemToBeUpdated[finishOutItem.FinishingInItemId] += diffSewInQuantity;
                }
                else
                {
                    finishingInItemToBeUpdated.Add(finishOutItem.FinishingInItemId, diffSewInQuantity);
                }

                if (!item.IsSave)
                {
                    item.Quantity = 0;

                    if (request.IsDifferentSize)
                    {
                        _GarmentSampleFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).ForEach(async finishOutDetail =>
                        {
                            finishOutDetail.Remove();
                            await _GarmentSampleFinishingOutDetailRepository.Update(finishOutDetail);
                        });
                    }

                    finishOutItem.Remove();

                }
                else
                {


                    if (request.IsDifferentSize)
                    {
                        _GarmentSampleFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).ForEach(async finishOutDetail =>
                        {
                            if (finishOutDetail.Identity != Guid.Empty)
                            {
                                var detail = item.Details.Where(o => o.Id == finishOutDetail.Identity).SingleOrDefault();

                                if (detail != null)
                                {
                                    finishOutDetail.SetQuantity(detail.Quantity);
                                    finishOutDetail.SetSizeId(new SizeId(detail.Size.Id));
                                    finishOutDetail.SetSizeName(detail.Size.Size);

                                    finishOutDetail.Modify();

                                }
                                else
                                {
                                    finishOutDetail.Remove();
                                }
                                await _GarmentSampleFinishingOutDetailRepository.Update(finishOutDetail);
                            }
                            else
                            {
                                GarmentSampleFinishingOutDetail GarmentSampleFinishingOutDetail = new GarmentSampleFinishingOutDetail(
                                Guid.NewGuid(),
                                finishOutItem.Identity,
                                finishOutDetail.SizeId,
                                finishOutDetail.SizeName,
                                finishOutDetail.Quantity,
                                finishOutDetail.UomId,
                                finishOutDetail.UomUnit
                            );
                                await _GarmentSampleFinishingOutDetailRepository.Update(GarmentSampleFinishingOutDetail);
                            }

                        });

                        foreach (var detail in item.Details)
                        {
                            if (detail.Id == Guid.Empty)
                            {
                                GarmentSampleFinishingOutDetail GarmentSampleFinishingOutDetail = new GarmentSampleFinishingOutDetail(
                                    Guid.NewGuid(),
                                    finishOutItem.Identity,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit
                                );
                                await _GarmentSampleFinishingOutDetailRepository.Update(GarmentSampleFinishingOutDetail);
                            }
                        }
                        finishOutItem.SetQuantity(item.TotalQuantity);
                        finishOutItem.SetRemainingQuantity(item.TotalQuantity);
                    }
                    else
                    {
                        finishOutItem.SetQuantity(item.Quantity);
                        finishOutItem.SetRemainingQuantity(item.Quantity);
                    }

                    finishOutItem.SetPrice(item.Price);
                    finishOutItem.Modify();
                }


                await _GarmentSampleFinishingOutItemRepository.Update(finishOutItem);
            });

            foreach (var finishingInItem in finishingInItemToBeUpdated)
            {
                var garmentSewInItem = _GarmentSampleFinishingInItemRepository.Query.Where(x => x.Identity == finishingInItem.Key).Select(s => new GarmentSampleFinishingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + finishingInItem.Value);
                garmentSewInItem.Modify();
                await _GarmentSampleFinishingInItemRepository.Update(garmentSewInItem);
            }

            finishOut.SetDate(request.FinishingOutDate.GetValueOrDefault());
            finishOut.Modify();
            await _GarmentSampleFinishingOutRepository.Update(finishOut);

            _storage.Save();

            return finishOut;
        }
    }
}
