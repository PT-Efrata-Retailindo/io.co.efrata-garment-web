using ExtCore.Data.Abstractions;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class GetAllCuttingOutQueryHandler : IQueryHandler<GetAllCuttingOutQuery, CuttingOutListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;

        public GetAllCuttingOutQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
        }

        public async Task<CuttingOutListViewModel> Handle(GetAllCuttingOutQuery request, CancellationToken cancellationToken)
        {
            var cuttingOutQuery = _garmentCuttingOutRepository.Query.Where(co => co.CuttingOutType != "SUBKON");
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.filter);
            cuttingOutQuery = QueryHelper<GarmentCuttingOutReadModel>.Filter(cuttingOutQuery, FilterDictionary);
            int total = cuttingOutQuery.Count();

            //var DocId = cuttingOutQuery.Select(x => x.Identity);
            //var DocItemId = _garmentCuttingOutItemRepository.Query.Where(x => DocId.Contains(x.CutOutId)).Select(x => x.Identity);
            //var queryDetail = _garmentCuttingOutDetailRepository.Query.Where(x => DocItemId.Contains(x.CutOutItemId));
            //double totalQty = queryDetail.Sum(x => x.CuttingOutQuantity);
            ////double totalQty = cuttingOutQuery.Sum(a => a.GarmentCuttingOutItem.Sum(b => b.GarmentCuttingOutDetail.Sum(c => c.CuttingOutQuantity)));

            //cuttingOutQuery = cuttingOutQuery.Skip((request.page - 1) * request.size)
            //    .Take(request.size);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.order);
            cuttingOutQuery = OrderDictionary.Count == 0 ? cuttingOutQuery.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentCuttingOutReadModel>.Order(cuttingOutQuery, OrderDictionary);

            if (!string.IsNullOrWhiteSpace(request.keyword))
            {
                cuttingOutQuery = cuttingOutQuery
                    .Where(co => co.CutOutNo.Contains(request.keyword)
                    || co.UnitCode.Contains(request.keyword)
                    || co.RONo.Contains(request.keyword)
                    || co.Article.Contains(request.keyword)
                    || co.GarmentCuttingOutItem.Any(coi => coi.CutOutId == co.Identity && coi.ProductCode.Contains(request.keyword)));
            }

            var DocId = cuttingOutQuery.Select(x => x.Identity);
            var DocItemId = _garmentCuttingOutItemRepository.Query.Where(x => DocId.Contains(x.CutOutId)).Select(x => x.Identity);
            var queryDetail = _garmentCuttingOutDetailRepository.Query.Where(x => DocItemId.Contains(x.CutOutItemId));
            double totalQty = queryDetail.Sum(x => x.CuttingOutQuantity);

            var selectedQuery = cuttingOutQuery.Select(co => new GarmentCuttingOutListDto
            {
                Id = co.Identity,
                CutOutNo = co.CutOutNo,
                CuttingOutType = co.CuttingOutType,
                UnitFrom = new UnitDepartment(co.UnitFromId, co.UnitFromCode, co.UnitFromName),
                CuttingOutDate = co.CuttingOutDate,
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                Comodity = new GarmentComodity(co.ComodityId, co.ComodityCode, co.ComodityName)
            });

            //var selectedData = selectedQuery.ToList();
            var selectedData = selectedQuery
                .Skip((request.page - 1) * request.size)
                .Take(request.size)
                .ToList();

            foreach (var co in selectedData)
            {
                co.Items = _garmentCuttingOutItemRepository.Query.Where(x => x.CutOutId == co.Id).OrderBy(x => x.Identity).Select(coi => new GarmentCuttingOutItemDto
                {
                    Id = coi.Identity,
                    CutOutId = coi.CutOutId,
                    CuttingInId = coi.CuttingInId,
                    CuttingInDetailId = coi.CuttingInDetailId,
                    Product = new Product(coi.ProductId, coi.ProductCode, coi.ProductName),
                    DesignColor = coi.DesignColor,
                    TotalCuttingOut = coi.TotalCuttingOut,
                }).ToList();

                foreach (var coi in co.Items)
                {
                    coi.Details = _garmentCuttingOutDetailRepository.Query.Where(x => x.CutOutItemId == coi.Id).OrderBy(x => x.Identity).Select(cod => new GarmentCuttingOutDetailDto
                    {
                        Id = cod.Identity,
                        CutOutItemId = cod.CutOutItemId,
                        Size = new SizeValueObject(cod.SizeId, cod.SizeName),
                        CuttingOutQuantity = cod.CuttingOutQuantity,
                        CuttingOutUom = new Uom(cod.CuttingOutUomId, cod.CuttingOutUomUnit),
                        Color = cod.Color,
                        RemainingQuantity = cod.RemainingQuantity,
                        BasicPrice = cod.BasicPrice,
                        Price = cod.Price,
                    }).ToList();
                }

                co.Products = co.Items.Select(i => i.Product.Code).ToList();
                co.TotalCuttingOutQuantity = co.Items.Sum(i => i.Details.Sum(d => d.CuttingOutQuantity));
                co.TotalRemainingQuantity = co.Items.Sum(i => i.Details.Sum(d => d.RemainingQuantity));
            }

            await Task.Yield();
            return new CuttingOutListViewModel
            {
                data = selectedData,
                total = total,
                totalQty = totalQty
            };
        }
    }
}
