using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts.Commands;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manufactures.Helpers.PDFTemplates;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("subcon-cutting-outs")]
    public class GarmentSubconCuttingOutController : ControllerApiBase
    {
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;

        public GarmentSubconCuttingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentCuttingOutRepository = Storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentCuttingOutItemRepository = Storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = Storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
            _garmentCuttingInRepository = Storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = Storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = Storage.GetRepository<IGarmentCuttingInDetailRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentCuttingOutRepository.Read(page, size, order, "", filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentCuttingOutItem.Sum(b => b.GarmentCuttingOutDetail.Sum(c => c.CuttingOutQuantity)));
            query = query.Skip((page - 1) * size).Take(size);

            var garmentCuttingOutDto = _garmentCuttingOutRepository.Find(query).Select(o => new GarmentSubconCuttingOutListDto(o)).ToArray();
            var garmentCuttingOutItemDto = _garmentCuttingOutItemRepository.Find(_garmentCuttingOutItemRepository.Query).Select(o => new GarmentSubconCuttingOutItemDto(o)).ToList();
            var garmentCuttingOutItemDtoArray = _garmentCuttingOutItemRepository.Find(_garmentCuttingOutItemRepository.Query).Select(o => new GarmentSubconCuttingOutItemDto(o)).ToArray();
            var garmentCuttingOutDetailDto = _garmentCuttingOutDetailRepository.Find(_garmentCuttingOutDetailRepository.Query).Select(o => new GarmentSubconCuttingOutDetailDto(o)).ToList();
            var garmentCuttingOutDetailDtoArray = _garmentCuttingOutDetailRepository.Find(_garmentCuttingOutDetailRepository.Query).Select(o => new GarmentSubconCuttingOutDetailDto(o)).ToArray();

            Parallel.ForEach(garmentCuttingOutDto, itemDto =>
            {
                var garmentCuttingOutItems = garmentCuttingOutItemDto.Where(x => x.CutOutId == itemDto.Id).ToList();


                itemDto.Items = garmentCuttingOutItems;

                Parallel.ForEach(itemDto.Items, detailDto =>
                {
                    var garmentCuttingOutDetails = garmentCuttingOutDetailDto.Where(x => x.CutOutItemId == detailDto.Id).ToList();
                    detailDto.Details = garmentCuttingOutDetails;

                    detailDto.Details = detailDto.Details.OrderBy(x => x.Id).ToList();
                });

                itemDto.Items = itemDto.Items.OrderBy(x => x.Id).ToList();

                itemDto.Products = itemDto.Items.Select(i => i.Product.Code).Distinct().ToList();
                itemDto.TotalCuttingOutQuantity = itemDto.Items.Sum(i => i.Details.Sum(d => d.CuttingOutQuantity));
                itemDto.TotalRemainingQuantity = itemDto.Items.Sum(i => i.Details.Sum(d => d.RemainingQuantity));
            });

            if (!string.IsNullOrEmpty(keyword))
            {
                garmentCuttingOutItemDtoArray = garmentCuttingOutItemDto.Where(x => x.Product.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
                List<GarmentSubconCuttingOutListDto> ListTemp = new List<GarmentSubconCuttingOutListDto>();
                foreach (var a in garmentCuttingOutItemDtoArray)
                {
                    var temp = garmentCuttingOutDto.Where(x => x.Id.Equals(a.CutOutId)).ToArray();
                    foreach (var b in temp)
                    {
                        ListTemp.Add(b);
                    }
                }

                var garmentCuttingOutDtoList = garmentCuttingOutDto.Where(x => x.CutOutNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || x.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    ).ToList();

                var i = 0;
                foreach (var data in ListTemp)
                {
                    i = 0;
                    foreach (var item in garmentCuttingOutDtoList)
                    {
                        if (data.Id == item.Id)
                        {
                            i++;
                        }
                    }
                    if (i == 0)
                    {
                        garmentCuttingOutDtoList.Add(data);
                    }
                }
                var garmentCuttingOutDtoListArray = garmentCuttingOutDtoList.ToArray();
                if (order != "{}")
                {
                    Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    garmentCuttingOutDtoListArray = QueryHelper<GarmentSubconCuttingOutListDto>.Order(garmentCuttingOutDtoList.AsQueryable(), OrderDictionary).ToArray();
                }
                else
                {
                    garmentCuttingOutDtoListArray = garmentCuttingOutDtoList.OrderByDescending(x => x.LastModifiedDate).ToArray();
                }
                totalQty = garmentCuttingOutDtoListArray.Sum(a => a.Items.Sum(b => b.Details.Sum(c => c.CuttingOutQuantity)));
                //garmentCuttingOutDtoListArray = garmentCuttingOutDtoListArray.Take(size).Skip((page - 1) * size).ToArray();

                await Task.Yield();
                return Ok(garmentCuttingOutDtoListArray, info: new
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
                //    garmentCuttingOutDto = QueryHelper<GarmentSubconCuttingOutListDto>.Order(garmentCuttingOutDto.AsQueryable(), OrderDictionary).ToArray();
                //}
                //else
                //{
                //    garmentCuttingOutDto = garmentCuttingOutDto.OrderByDescending(x => x.LastModifiedDate).ToArray();
                //}

                //garmentCuttingOutDto = garmentCuttingOutDto.Take(size).Skip((page - 1) * size).ToArray();

                await Task.Yield();
                return Ok(garmentCuttingOutDto, info: new
                {
                    page,
                    size,
                    total,
                    totalQty
                });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSubconCuttingOutDto garmentCuttingOutDto = _garmentCuttingOutRepository.Find(o => o.Identity == guid).Select(cutOut => new GarmentSubconCuttingOutDto(cutOut)
            {
                Items = _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).Select(cutOutItem => new GarmentSubconCuttingOutItemDto(cutOutItem)
                {
                    Details = _garmentCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).Select(cutOutDetail => new GarmentSubconCuttingOutDetailDto(cutOutDetail)
                    {
                        //PreparingRemainingQuantity = _garmentPreparingItemRepository.Query.Where(o => o.Identity == cutInDetail.PreparingItemId).Select(o => o.RemainingQuantity).FirstOrDefault() + cutInDetail.PreparingQuantity,
                    }).OrderBy(o => o.Size.Size).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentCuttingOutDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconCuttingOutCommand command)
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

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconCuttingOutCommand command)
        //{
        //    Guid guid = Guid.Parse(id);

        //    command.SetIdentity(guid);

        //    VerifyUser();

        //    var order = await Mediator.Send(command);

        //    return Ok(order.Identity);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentSubconCuttingOutCommand command = new RemoveGarmentSubconCuttingOutCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
            
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentCuttingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentCuttingOutDto = _garmentCuttingOutRepository.Find(query).Select(o => new GarmentSubconCuttingOutDto(o)).ToArray();
            var garmentCuttingOutItemDto = _garmentCuttingOutItemRepository.Find(_garmentCuttingOutItemRepository.Query).Select(o => new GarmentSubconCuttingOutItemDto(o)).ToList();
            var garmentCuttingOutDetailDto = _garmentCuttingOutDetailRepository.Find(_garmentCuttingOutDetailRepository.Query).Select(o => new GarmentSubconCuttingOutDetailDto(o)).ToList();

            Parallel.ForEach(garmentCuttingOutDto, itemDto =>
            {
                var garmentCuttingOutItems = garmentCuttingOutItemDto.Where(x => x.CutOutId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentCuttingOutItems;

                Parallel.ForEach(itemDto.Items, detailDto =>
                {
                    var garmentCuttingInDetails = garmentCuttingOutDetailDto.Where(x => x.CutOutItemId == detailDto.Id).OrderBy(x => x.Id).ToList();
                    detailDto.Details = garmentCuttingInDetails;
                });
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentCuttingOutDto = QueryHelper<GarmentSubconCuttingOutDto>.Order(garmentCuttingOutDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentCuttingOutDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("{id}/{buyer}")]
        public async Task<IActionResult> GetPdf(string id, string buyer)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentSubconCuttingOutDto garmentCuttingOutDto = _garmentCuttingOutRepository.Find(o => o.Identity == guid).Select(cutOut => new GarmentSubconCuttingOutDto(cutOut)
            {
                Items = _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).Select(cutOutItem => new GarmentSubconCuttingOutItemDto(cutOutItem)
                {
                    Details = _garmentCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).Select(cutOutDetail => new GarmentSubconCuttingOutDetailDto(cutOutDetail)
                    {
                        //PreparingRemainingQuantity = _garmentPreparingItemRepository.Query.Where(o => o.Identity == cutInDetail.PreparingItemId).Select(o => o.RemainingQuantity).FirstOrDefault() + cutInDetail.PreparingQuantity,
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentSubconCuttingOutPDFTemplate.Generate(garmentCuttingOutDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentCuttingOutDto.CutOutNo}.pdf"
            };
        }

        [HttpGet("by-roNo")]
        public async Task<IActionResult> GetLoaderByRO(string keyword, string filter = "{}")
        {
            var query = _garmentCuttingOutRepository.Read(1, int.MaxValue, "{}", "", filter);
            query = query.Where(o => o.RONo.Contains(keyword));

            var rOs = _garmentCuttingOutRepository.Find(query)
                .Select(o => new { o.RONo, o.Article,o.POSerialNumber }).Distinct().ToList();

            await Task.Yield();

            return Ok(rOs);
        }
    }
}
