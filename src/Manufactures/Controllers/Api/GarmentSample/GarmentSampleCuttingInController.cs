using Barebone.Controllers;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Dtos.GarmentSample.SampleCuttingIns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-cutting-ins")]
    public class GarmentSampleCuttingInController : ControllerApiBase
    {
        private readonly IGarmentSampleCuttingInRepository _garmentSampleCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentSampleCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentSampleCuttingInDetailRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;

        public GarmentSampleCuttingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSampleCuttingInRepository = Storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentSampleCuttingInItemRepository = Storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentSampleCuttingInDetailRepository = Storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentSamplePreparingItemRepository = Storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSampleCuttingInRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            var DocId = query.Select(x => x.Identity);
            var DocItemId = _garmentSampleCuttingInItemRepository.Query.Where(x => DocId.Contains(x.CutInId)).Select(x => x.Identity);
            var queryDetail = _garmentSampleCuttingInDetailRepository.Query.Where(x => DocItemId.Contains(x.CutInItemId));
            double totalQty = queryDetail.Sum(x => x.CuttingInQuantity);

            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSampleCuttingInListDto> garmentSampleCuttingInListDtos = _garmentSampleCuttingInRepository
                .Find(query)
                .Select(cutIn => new GarmentSampleCuttingInListDto(cutIn))
                .ToList();

            var dtoIds = garmentSampleCuttingInListDtos.Select(s => s.Id).ToList();
            var items = _garmentSampleCuttingInItemRepository.Query
                .Where(o => dtoIds.Contains(o.CutInId))
                .Select(s => new { s.Identity, s.CutInId, s.UENNo })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            var details = _garmentSampleCuttingInDetailRepository.Query
                .Where(o => itemIds.Contains(o.CutInItemId))
                .Select(s => new { s.Identity, s.CutInItemId, s.ProductCode, s.CuttingInQuantity })
                .ToList();

            Parallel.ForEach(garmentSampleCuttingInListDtos, dto =>
            {
                var currentItems = items.Where(w => w.CutInId == dto.Id);
                dto.UENNos = currentItems.Select(i => i.UENNo).ToList();
                dto.Products = currentItems.SelectMany(i => details.Where(w => w.CutInItemId == i.Identity).Select(d => d.ProductCode)).Distinct().ToList();
                dto.TotalCuttingInQuantity = currentItems.Sum(i => details.Where(w => w.CutInItemId == i.Identity).Sum(d => d.CuttingInQuantity));
            });

            await Task.Yield();
            return Ok(garmentSampleCuttingInListDtos, info: new
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

            GarmentSampleCuttingInDto garmentSampleCuttingInDto = _garmentSampleCuttingInRepository.Find(o => o.Identity == guid).Select(cutIn => new GarmentSampleCuttingInDto(cutIn)
            {
                Items = _garmentSampleCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).Select(cutInItem => new GarmentSampleCuttingInItemDto(cutInItem)
                {
                    Details = _garmentSampleCuttingInDetailRepository.Find(o => o.CutInItemId == cutInItem.Identity).Select(cutInDetail => new GarmentSampleCuttingInDetailDto(cutInDetail)
                    {
                        PreparingRemainingQuantity = _garmentSamplePreparingItemRepository.Query.Where(o => o.Identity == cutInDetail.PreparingItemId).Select(o => o.RemainingQuantity).FirstOrDefault() + cutInDetail.PreparingQuantity,
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSampleCuttingInDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleCuttingInCommand command)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleCuttingInCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentSampleCuttingInCommand command = new RemoveGarmentSampleCuttingInCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("by-roNo")]
        public async Task<IActionResult> GetLoaderByRO(string keyword, string filter = "{}")
        {
            var query = _garmentSampleCuttingInRepository.Read(1, int.MaxValue, "{}", "", filter);
            query = query.Where(o => o.RONo.Contains(keyword) && o.Items.Any(a => a.Details.Any(b => b.RemainingQuantity > 0)));

            var rOs = _garmentSampleCuttingInRepository.Find(query)
                .Select(o => new { o.RONo, o.Article }).Distinct().ToList();

            await Task.Yield();

            return Ok(rOs);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSampleCuttingInRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var newQuery = _garmentSampleCuttingInRepository.ReadExecute(query);

            await Task.Yield();
            return Ok(newQuery, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentSampleCuttingInCommand command)
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
