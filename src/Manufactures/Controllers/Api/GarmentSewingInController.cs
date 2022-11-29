using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("sewing-ins")]
    public class GarmentSewingInController : ControllerApiBase
    {
        //Enhance Jason Aug 2021
        private readonly ISewingInHomeListViewRepository _sewingInHomeListViewRepository;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public GarmentSewingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _sewingInHomeListViewRepository = Storage.GetRepository<ISewingInHomeListViewRepository>();
            _garmentSewingInRepository = Storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = Storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        [HttpGet]
        //Original GET Method with Customization by Previous Developer
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingInRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSewingInItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSewingInListDto> garmentSewingInDto = _garmentSewingInRepository
                .Find(query)
                .Select(o => new GarmentSewingInListDto(o))
                .ToList();

            var dtoIds = garmentSewingInDto.Select(s => s.Id).ToList();
            var garmentSewingInItemDto = _garmentSewingInItemRepository.Query
                .Where(o => dtoIds.Contains(o.SewingInId))
                .Select(s => new GarmentSewingInItemDto(s))
                .ToList();

            var itemIds = garmentSewingInItemDto.Select(s => s.Id).ToList();

            Parallel.ForEach(garmentSewingInDto, itemDto =>
            {
                var garmentSewingDOItems = garmentSewingInItemDto.Where(x => x.SewingInId == itemDto.Id).ToList();

                itemDto.Items = garmentSewingDOItems;

                itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();

                itemDto.Products = itemDto.Items.Select(i => i.Product.Code).ToList();
                itemDto.TotalQuantity = itemDto.Items.Sum(i => i.Quantity);
                itemDto.TotalRemainingQuantity = itemDto.Items.Sum(i => i.RemainingQuantity);

            });

            if (!string.IsNullOrEmpty(keyword))
            {
                garmentSewingInItemDto = garmentSewingInItemDto.Where(x => x.Product.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                List<GarmentSewingInListDto> ListTemp = new List<GarmentSewingInListDto>();
                foreach (var a in garmentSewingInItemDto)
                {
                    var temp = garmentSewingInDto.Where(x => x.Id.Equals(a.SewingInId)).ToArray();
                    foreach (var b in temp)
                    {
                        ListTemp.Add(b);
                    }
                }

                var garmentSewingInDtoList = garmentSewingInDto.Where(x => x.SewingInNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Unit.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.UnitFrom.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    ).ToList();

                var i = 0;
                foreach (var data in ListTemp)
                {
                    i = 0;
                    foreach (var item in garmentSewingInDtoList)
                    {
                        if (data.Id == item.Id)
                        {
                            i++;
                        }
                    }
                    if (i == 0)
                    {
                        garmentSewingInDtoList.Add(data);
                    }
                }
                var garmentSewingInDtoListArray = garmentSewingInDtoList.ToArray();
                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentSewingInDtoListArray = QueryHelper<GarmentSewingInListDto>.Order(garmentSewingInDtoList.AsQueryable(), OrderDictionary).ToArray();
                }
                else
                {
                    garmentSewingInDtoListArray = garmentSewingInDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
                }

                //garmentSewingInDtoListArray = garmentSewingInDtoListArray.Take(size).Skip((page - 1) * size).ToArray();

                await Task.Yield();
                return Ok(garmentSewingInDtoListArray, info: new
                {
                    page,
                    size,
                    total,
                    totalQty
                });
            }
            else
            {
                //if (order != "{}")
                //{
                //    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                //    garmentSewingInDto = QueryHelper<GarmentSewingInListDto>.Order(garmentSewingInDto.AsQueryable(), OrderDictionary).ToArray();
                //}
                //else
                //{
                //    garmentSewingInDto = garmentSewingInDto.OrderByDescending(x => x.LastModifiedDate).ToArray();
                //}

                //garmentSewingInDto = garmentSewingInDto.Take(size).Skip((page - 1) * size).ToArray();

                await Task.Yield();
                return Ok(garmentSewingInDto, info: new
                {
                    page,
                    size,
                    total,
                    totalQty
                });
            }
            //List<GarmentSewingInListDto> garmentSewingInListDtos = _garmentSewingInRepository.Find(query).Select(sewingIn =>
            //{
            //    var items = _garmentSewingInItemRepository.Query.Where(o => o.SewingInId == sewingIn.Identity).Select(sewingInItem => new
            //    {
            //        sewingInItem.ProductCode,
            //        sewingInItem.Quantity,
            //    }).ToList();

            //    return new GarmentSewingInListDto(sewingIn)
            //    {
            //        Products = items.Select(i => i.ProductCode).ToList(),
            //        TotalQuantity = items.Sum(i => i.Quantity),
            //    };
            //}).ToList();

            //await Task.Yield();
            //return Ok(garmentSewingInListDtos, info: new
            //{
            //    page,
            //    size,
            //    count
            //});
        }

        [HttpGet("list-optimized")]
        //Enhance Performance / Load Time on Sewing In List Page : Jason Aug 2021
        public async Task<IActionResult> GetListOptimized(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _sewingInHomeListViewRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.TotalQuantity);
            query = query.Skip((page - 1) * size).Take(size);

            List<SewingInHomeListViewDto> sewingInHomeListViewDto = _sewingInHomeListViewRepository
                .Find(query)
                .Select(o => new SewingInHomeListViewDto(o))
                .ToList();

            await Task.Yield();
            return Ok(sewingInHomeListViewDto, info: new
            {
                page,
                size,
                total,
                totalQty
            });


            //    List<GarmentSewingInListDto> garmentSewingInDto = _garmentSewingInRepository
            //        .Find(query)
            //        .Select(o => new GarmentSewingInListDto(o))
            //        .ToList();

            //    var dtoIds = garmentSewingInDto.Select(s => s.Id).ToList();
            //    var garmentSewingInItemDto = _garmentSewingInItemRepository.Query
            //        .Where(o => dtoIds.Contains(o.SewingInId))
            //        .Select(s => new GarmentSewingInItemDto(s))
            //        .ToList();

            //    var itemIds = garmentSewingInItemDto.Select(s => s.Id).ToList();

            //    Parallel.ForEach(garmentSewingInDto, itemDto =>
            //    {
            //        var garmentSewingDOItems = garmentSewingInItemDto.Where(x => x.SewingInId == itemDto.Id).ToList();

            //        itemDto.Items = garmentSewingDOItems;

            //        itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();

            //        itemDto.Products = itemDto.Items.Select(i => i.Product.Code).ToList();
            //        itemDto.TotalQuantity = itemDto.Items.Sum(i => i.Quantity);
            //        itemDto.TotalRemainingQuantity = itemDto.Items.Sum(i => i.RemainingQuantity);

            //    });

            //    if (!string.IsNullOrEmpty(keyword))
            //    {
            //        garmentSewingInItemDto = garmentSewingInItemDto.Where(x => x.Product.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            //        List<GarmentSewingInListDto> ListTemp = new List<GarmentSewingInListDto>();
            //        foreach (var a in garmentSewingInItemDto)
            //        {
            //            var temp = garmentSewingInDto.Where(x => x.Id.Equals(a.SewingInId)).ToArray();
            //            foreach (var b in temp)
            //            {
            //                ListTemp.Add(b);
            //            }
            //        }

            //        var garmentSewingInDtoList = garmentSewingInDto.Where(x => x.SewingInNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                            || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                            || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                            || x.Unit.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                            || x.UnitFrom.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            //                            ).ToList();

            //        var i = 0;
            //        foreach (var data in ListTemp)
            //        {
            //            i = 0;
            //            foreach (var item in garmentSewingInDtoList)
            //            {
            //                if (data.Id == item.Id)
            //                {
            //                    i++;
            //                }
            //            }
            //            if (i == 0)
            //            {
            //                garmentSewingInDtoList.Add(data);
            //            }
            //        }
            //        var garmentSewingInDtoListArray = garmentSewingInDtoList.ToArray();
            //        if (order != "{}")
            //        {
            //            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //            garmentSewingInDtoListArray = QueryHelper<GarmentSewingInListDto>.Order(garmentSewingInDtoList.AsQueryable(), OrderDictionary).ToArray();
            //        }
            //        else
            //        {
            //            garmentSewingInDtoListArray = garmentSewingInDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
            //        }

            //        await Task.Yield();
            //        return Ok(garmentSewingInDtoListArray, info: new
            //        {
            //            page,
            //            size,
            //            total,
            //            totalQty
            //        });
            //    }
            //    else
            //    {
            //        await Task.Yield();
            //        return Ok(garmentSewingInDto, info: new
            //        {
            //            page,
            //            size,
            //            total,
            //            totalQty
            //        });
            //    }
        }

        [HttpGet("original")]
        //Original GET Method by Previous Developer
        //public async Task<IActionResult> GetOriginal(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        //{
        //    VerifyUser();

        //    var query = _garmentSewingInRepository.Read(page, size, order, keyword, filter);
        //    var total = query.Count();
        //    double totalQty = query.Sum(a => a.GarmentSewingInItem.Sum(b => b.Quantity));
        //    query = query.Skip((page - 1) * size).Take(size);

        //    var garmentSewingInDto = _garmentSewingInRepository.Find(query).Select(o => new GarmentSewingInListDto(o)).ToArray();
        //    var garmentSewingInItemDto = _garmentSewingInItemRepository.Find(_garmentSewingInItemRepository.Query).Select(o => new GarmentSewingInItemDto(o)).ToList();
        //    var garmentSewingInItemDtoArray = _garmentSewingInItemRepository.Find(_garmentSewingInItemRepository.Query).Select(o => new GarmentSewingInItemDto(o)).ToArray();

        //    Parallel.ForEach(garmentSewingInDto, itemDto =>
        //    {
        //        var garmentSewingDOItems = garmentSewingInItemDto.Where(x => x.SewingInId == itemDto.Id).ToList();

        //        itemDto.Items = garmentSewingDOItems;

        //        itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();

        //        itemDto.Products = itemDto.Items.Select(i => i.Product.Code).ToList();
        //        itemDto.TotalQuantity = itemDto.Items.Sum(i => i.Quantity);
        //        itemDto.TotalRemainingQuantity = itemDto.Items.Sum(i => i.RemainingQuantity);
        //    });

        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        garmentSewingInItemDtoArray = garmentSewingInItemDto.Where(x => x.Product.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
        //        List<GarmentSewingInListDto> ListTemp = new List<GarmentSewingInListDto>();
        //        foreach (var a in garmentSewingInItemDtoArray)
        //        {
        //            var temp = garmentSewingInDto.Where(x => x.Id.Equals(a.SewingInId)).ToArray();
        //            foreach (var b in temp)
        //            {
        //                ListTemp.Add(b);
        //            }
        //        }

        //        var garmentSewingInDtoList = garmentSewingInDto.Where(x => x.SewingInNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        //                            || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        //                            || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        //                            || x.Unit.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        //                            || x.UnitFrom.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        //                            ).ToList();

        //        var i = 0;
        //        foreach (var data in ListTemp)
        //        {
        //            i = 0;
        //            foreach (var item in garmentSewingInDtoList)
        //            {
        //                if (data.Id == item.Id)
        //                {
        //                    i++;
        //                }
        //            }
        //            if (i == 0)
        //            {
        //                garmentSewingInDtoList.Add(data);
        //            }
        //        }
        //        var garmentSewingInDtoListArray = garmentSewingInDtoList.ToArray();
        //        if (order != "{}")
        //        {
        //            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
        //            garmentSewingInDtoListArray = QueryHelper<GarmentSewingInListDto>.Order(garmentSewingInDtoList.AsQueryable(), OrderDictionary).ToArray();
        //        }
        //        else
        //        {
        //            garmentSewingInDtoListArray = garmentSewingInDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
        //        }

        //        //garmentSewingInDtoListArray = garmentSewingInDtoListArray.Take(size).Skip((page - 1) * size).ToArray();

        //        await Task.Yield();
        //        return Ok(garmentSewingInDtoListArray, info: new
        //        {
        //            page,
        //            size,
        //            total,
        //            totalQty
        //        });
        //    }
        //    else
        //    {
        //        //if (order != "{}")
        //        //{
        //        //    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
        //        //    garmentSewingInDto = QueryHelper<GarmentSewingInListDto>.Order(garmentSewingInDto.AsQueryable(), OrderDictionary).ToArray();
        //        //}
        //        //else
        //        //{
        //        //    garmentSewingInDto = garmentSewingInDto.OrderByDescending(x => x.LastModifiedDate).ToArray();
        //        //}

        //        //garmentSewingInDto = garmentSewingInDto.Take(size).Skip((page - 1) * size).ToArray();

        //        await Task.Yield();
        //        return Ok(garmentSewingInDto, info: new
        //        {
        //            page,
        //            size,
        //            total,
        //            totalQty
        //        });
        //    }
        //    //List<GarmentSewingInListDto> garmentSewingInListDtos = _garmentSewingInRepository.Find(query).Select(sewingIn =>
        //    //{
        //    //    var items = _garmentSewingInItemRepository.Query.Where(o => o.SewingInId == sewingIn.Identity).Select(sewingInItem => new
        //    //    {
        //    //        sewingInItem.ProductCode,
        //    //        sewingInItem.Quantity,
        //    //    }).ToList();

        //    //    return new GarmentSewingInListDto(sewingIn)
        //    //    {
        //    //        Products = items.Select(i => i.ProductCode).ToList(),
        //    //        TotalQuantity = items.Sum(i => i.Quantity),
        //    //    };
        //    //}).ToList();

        //    //await Task.Yield();
        //    //return Ok(garmentSewingInListDtos, info: new
        //    //{
        //    //    page,
        //    //    size,
        //    //    count
        //    //});
        //}

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingInRepository.ReadComplete(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSewingInItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            var garmentSewingInDto = _garmentSewingInRepository.Find(query).Select(o => new GarmentSewingInListDto(o)).ToArray();

            await Task.Yield();
            return Ok(garmentSewingInDto, info: new
            {
                page,
                size,
                total,
                totalQty
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSewingInDto garmentSewingIn = _garmentSewingInRepository.Find(o => o.Identity == guid).Select(sewingIn => new GarmentSewingInDto(sewingIn)
            {
                Items = _garmentSewingInItemRepository.Find(o => o.SewingInId == sewingIn.Identity).OrderBy(i=>i.Color).ThenBy(i=>i.SizeName).Select(sewingInItem => new GarmentSewingInItemDto(sewingInItem)).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSewingIn);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSewingInCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            VerifyUser();
            var garmentSewingInId = Guid.Parse(id);

            if (!Guid.TryParse(id, out Guid orderId))
                return NotFound();

            RemoveGarmentSewingInCommand command = new RemoveGarmentSewingInCommand(garmentSewingInId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingInRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSewingInDto = _garmentSewingInRepository.Find(query).Select(o => new GarmentSewingInDto(o)).ToArray();
            var garmentSewingInItemDto = _garmentSewingInItemRepository.Find(_garmentSewingInItemRepository.Query).Select(o => new GarmentSewingInItemDto(o)).ToList();
            
            Parallel.ForEach(garmentSewingInDto, itemDto =>
            {
                var garmentSewingInItems = garmentSewingInItemDto.Where(x => x.SewingInId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentSewingInItems;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSewingInDto = QueryHelper<GarmentSewingInDto>.Order(garmentSewingInDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSewingInDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentSewingInCommand command)
        {
            VerifyUser();

            if (command.Date == null || command.Date == DateTimeOffset.MinValue)
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Tanggal harus diisi"
                });
            else if (command.Date.Date > DateTimeOffset.Now.Date)
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Tanggal tidak boleh lebih dari hari ini"
                });

            var order = await Mediator.Send(command);

            return Ok();
        }
    }
}