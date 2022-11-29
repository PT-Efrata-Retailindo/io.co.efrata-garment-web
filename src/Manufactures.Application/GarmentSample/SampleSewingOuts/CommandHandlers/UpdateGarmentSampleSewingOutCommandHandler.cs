using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.CommandHandlers
{
    public class UpdateGarmentSampleSewingOutCommandHandler : ICommandHandler<UpdateGarmentSampleSewingOutCommand, GarmentSampleSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSampleSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSampleSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentSampleCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public UpdateGarmentSampleSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSampleSewingOutDetailRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSampleSewingOut> Handle(UpdateGarmentSampleSewingOutCommand request, CancellationToken cancellationToken)
        {
            var sewOut = _garmentSewingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleSewingOut(o)).Single();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

            if (sewOut.SewingTo == "CUTTING")
            {
                Guid cutInId = _garmentCuttingInItemRepository.Query.Where(a => a.SewingOutId == sewOut.Identity).First().CutInId;

                var cutIn = _garmentCuttingInRepository.Query.Where(a => a.Identity == cutInId).Select(a => new GarmentSampleCuttingIn(a)).Single();

                _garmentCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).ForEach(async cutInItem =>
                {
                    cutInItem.Modify();
                    await _garmentCuttingInItemRepository.Update(cutInItem);
                });
                cutIn.SetDate(request.SewingOutDate.GetValueOrDefault());
                cutIn.Modify();
                await _garmentCuttingInRepository.Update(cutIn);
            }

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentSewingOutItemRepository.Find(o => o.SampleSewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
            {
                var item = request.Items.Where(o => o.Id == sewOutItem.Identity).Single();

                var diffSewInQuantity = item.IsSave ? (sewOutItem.Quantity - (request.IsDifferentSize ? item.TotalQuantity : item.Quantity)) : sewOutItem.Quantity;

                if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SampleSewingInItemId))
                {
                    sewInItemToBeUpdated[sewOutItem.SampleSewingInItemId] += diffSewInQuantity;
                }
                else
                {
                    sewInItemToBeUpdated.Add(sewOutItem.SampleSewingInItemId, diffSewInQuantity);
                }

                if (!item.IsSave)
                {
                    item.Quantity = 0;

                    if (request.IsDifferentSize)
                    {
                        _garmentSewingOutDetailRepository.Find(o => o.SampleSewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            if (sewOut.SewingTo == "CUTTING")
                            {
                                GarmentSampleCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutDetailId == sewOutDetail.Identity).Select(a => new GarmentSampleCuttingInDetail(a)).Single();
                                cuttingInDetail.Remove();
                                await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                            }

                            sewOutDetail.Remove();
                            await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                        });

                        if (sewOut.SewingTo == "CUTTING")
                        {
                            Guid cuttingInItemId = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutDetailId == item.Details.First().Id).Select(a => new GarmentSampleCuttingInDetail(a)).First().CutInItemId;
                            GarmentSampleCuttingInItem cuttingInItem = _garmentCuttingInItemRepository.Query.Where(a => a.Identity == cuttingInItemId).Select(a => new GarmentSampleCuttingInItem(a)).Single();
                            cuttingInItem.Remove();
                            await _garmentCuttingInItemRepository.Update(cuttingInItem);
                        }

                    }
                    else
                    {
                        if (sewOut.SewingTo == "CUTTING")
                        {
                            GarmentSampleCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentSampleCuttingInDetail(a)).Single();
                            GarmentSampleCuttingInItem cuttingInItem = _garmentCuttingInItemRepository.Query.Where(a => a.Identity == cuttingInDetail.CutInItemId).Select(a => new GarmentSampleCuttingInItem(a)).Single();
                            cuttingInDetail.Remove();
                            await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                            cuttingInItem.Remove();
                            await _garmentCuttingInItemRepository.Update(cuttingInItem);
                        }
                    }

                    sewOutItem.Remove();

                }
                else
                {
                    if (request.IsDifferentSize)
                    {

                        _garmentSewingOutDetailRepository.Find(o => o.SampleSewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            if (sewOutDetail.Identity != Guid.Empty)
                            {

                                var detail = item.Details.Where(o => o.Id == sewOutDetail.Identity).SingleOrDefault();

                                if (detail != null)
                                {
                                    if (sewOut.SewingTo == "CUTTING")
                                    {
                                        GarmentSampleCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutDetailId == sewOutDetail.Identity).Select(a => new GarmentSampleCuttingInDetail(a)).Single();
                                        cuttingInDetail.SetCuttingInQuantity(Convert.ToInt32(detail.Quantity));
                                        cuttingInDetail.SetPrice((item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * detail.Quantity);
                                        cuttingInDetail.SetRemainingQuantity(Convert.ToInt32(detail.Quantity));
                                        cuttingInDetail.Modify();
                                        await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                                    }
                                    sewOutDetail.SetQuantity(detail.Quantity);
                                    sewOutDetail.SetSizeId(new SizeId(detail.Size.Id));
                                    sewOutDetail.SetSizeName(detail.Size.Size);

                                    sewOutDetail.Modify();
                                }
                                else
                                {
                                    if (sewOut.SewingTo == "CUTTING")
                                    {
                                        GarmentSampleCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutDetailId == sewOutDetail.Identity).Select(a => new GarmentSampleCuttingInDetail(a)).Single();
                                        cuttingInDetail.Remove();
                                        await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                                    }
                                    sewOutDetail.Remove();
                                }
                                await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                            }
                            else
                            {
                                GarmentSampleSewingOutDetail garmentSewingOutDetail = new GarmentSampleSewingOutDetail(
                                    Guid.NewGuid(),
                                    sewOutItem.Identity,
                                    sewOutDetail.SizeId,
                                    sewOutDetail.SizeName,
                                    sewOutDetail.Quantity,
                                    sewOutDetail.UomId,
                                    sewOutDetail.UomUnit
                                    );
                                await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);

                                if (sewOut.SewingTo == "CUTTING")
                                {
                                    Guid cuttingInItemId = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentSampleCuttingInDetail(a)).First().CutInItemId;
                                    GarmentSampleCuttingInDetail garmentCuttingInDetail = new GarmentSampleCuttingInDetail(
                                    Guid.NewGuid(),
                                    cuttingInItemId,
                                    Guid.Empty,
                                    sewOutItem.Identity,
                                    garmentSewingOutDetail.Identity,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    null,
                                    0,
                                    new UomId(0),
                                    null,
                                    Convert.ToInt32(garmentSewingOutDetail.Quantity),
                                    garmentSewingOutDetail.UomId,
                                    garmentSewingOutDetail.UomUnit,
                                    garmentSewingOutDetail.Quantity,
                                    item.BasicPrice,
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * garmentSewingOutDetail.Quantity,
                                    0,
                                    item.Color
                                    );

                                    await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                                }
                            }

                        });

                        foreach (var detail in item.Details)
                        {
                            if (detail.Id == Guid.Empty)
                            {
                                GarmentSampleSewingOutDetail garmentSewingOutDetail = new GarmentSampleSewingOutDetail(
                                    Guid.NewGuid(),
                                    sewOutItem.Identity,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit
                                );
                                await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);

                                if (sewOut.SewingTo == "CUTTING")
                                {
                                    Guid cuttingInItemId = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentSampleCuttingInDetail(a)).First().CutInItemId;
                                    GarmentSampleCuttingInDetail garmentCuttingInDetail = new GarmentSampleCuttingInDetail(
                                    Guid.NewGuid(),
                                    cuttingInItemId,
                                    Guid.Empty,
                                    sewOutItem.Identity,
                                    garmentSewingOutDetail.Identity,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    null,
                                    0,
                                    new UomId(0),
                                    null,
                                    Convert.ToInt32(garmentSewingOutDetail.Quantity),
                                    garmentSewingOutDetail.UomId,
                                    garmentSewingOutDetail.UomUnit,
                                    garmentSewingOutDetail.Quantity,
                                    item.BasicPrice,
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * garmentSewingOutDetail.Quantity,
                                    0,
                                    item.Color
                                    );

                                    await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                                }
                            }
                        }
                        sewOutItem.SetQuantity(item.TotalQuantity);
                        sewOutItem.SetRemainingQuantity(item.TotalQuantity);
                    }
                    else
                    {
                        sewOutItem.SetQuantity(item.Quantity);
                        sewOutItem.SetRemainingQuantity(item.Quantity);
                        if (sewOut.SewingTo == "CUTTING")
                        {
                            GarmentSampleCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentSampleCuttingInDetail(a)).Single();
                            cuttingInDetail.SetCuttingInQuantity(Convert.ToInt32(sewOutItem.Quantity));
                            cuttingInDetail.SetPrice((item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * sewOutItem.Quantity);
                            cuttingInDetail.SetRemainingQuantity(Convert.ToInt32(sewOutItem.Quantity));

                            cuttingInDetail.Modify();
                            await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                        }

                    }

                    sewOutItem.SetPrice(item.Price);
                    sewOutItem.Modify();
                }


                await _garmentSewingOutItemRepository.Update(sewOutItem);
            });

            foreach (var sewingInItem in sewInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSampleSewingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + sewingInItem.Value);
                garmentSewInItem.Modify();
                await _garmentSewingInItemRepository.Update(garmentSewInItem);
            }

            sewOut.SetDate(request.SewingOutDate.GetValueOrDefault());
            sewOut.Modify();
            await _garmentSewingOutRepository.Update(sewOut);

            _storage.Save();

            return sewOut;
        }
    }
}
