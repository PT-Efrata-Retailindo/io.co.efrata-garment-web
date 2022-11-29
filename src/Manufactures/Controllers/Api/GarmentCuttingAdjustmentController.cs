using Barebone.Controllers;
using Manufactures.Domain.GarmentCuttingAdjustments.Commands;
using Manufactures.Domain.GarmentCuttingAdjustments.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{

    [ApiController]
    [Authorize]
    [Route("garment-cutting-adjustments")]
    public class GarmentCuttingAdjustmentController : ControllerApiBase
    {
        private readonly IGarmentCuttingAdjustmentRepository _garmentCuttingAdjustmentRepository;
        private readonly IGarmentCuttingAdjustmentItemRepository _garmentCuttingAdjustmentItemRepository;

        public GarmentCuttingAdjustmentController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentCuttingAdjustmentRepository = Storage.GetRepository<IGarmentCuttingAdjustmentRepository>();
            _garmentCuttingAdjustmentItemRepository = Storage.GetRepository<IGarmentCuttingAdjustmentItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentCuttingAdjustmentRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();

            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentCuttingAdjustmentListDto> garmentCuttingAdjustmentListDtos = _garmentCuttingAdjustmentRepository
                .Find(query)
                .Select(cutAdjustment => new GarmentCuttingAdjustmentListDto(cutAdjustment))
                .ToList();

            var dtoIds = garmentCuttingAdjustmentListDtos.Select(s => s.Id).ToList();
            
            await Task.Yield();
            return Ok(garmentCuttingAdjustmentListDtos, info: new
            {
                page,
                size,
                total
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentCuttingAdjustmentDto garmentCuttingAdjustmentDto = _garmentCuttingAdjustmentRepository.Find(o => o.Identity == guid).Select(cutAdjustment => new GarmentCuttingAdjustmentDto(cutAdjustment)
            {
                Items = _garmentCuttingAdjustmentItemRepository.Find(o => o.AdjustmentCuttingId == cutAdjustment.Identity).Select(cutAdjustmentItem => new GarmentCuttingAdjustmentItemDto(cutAdjustmentItem)
                {
                   
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentCuttingAdjustmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentCuttingAdjustmentCommand command)
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

    }
}
