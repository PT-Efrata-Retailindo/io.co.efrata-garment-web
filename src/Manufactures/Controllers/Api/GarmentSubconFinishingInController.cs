using Barebone.Controllers;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSubconFinishingIns.Commands;
using Manufactures.Dtos;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("subcon-finishing-ins")]
    public class GarmentSubconFinishingInController : ControllerApiBase
    {

        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;

        public GarmentSubconFinishingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentFinishingInRepository = Storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = Storage.GetRepository<IGarmentFinishingInItemRepository>();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconFinishingInCommand command)
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
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentSubconFinishingInCommand command = new RemoveGarmentSubconFinishingInCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpGet("{id}/{buyer}")]
        public async Task<IActionResult> GetPdf(string id, string buyer)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
            GarmentFinishingInDto garmentFinishingInDto = _garmentFinishingInRepository.Find(o => o.Identity == guid).Select(finIn => new GarmentFinishingInDto(finIn)
            {
                Items = _garmentFinishingInItemRepository.Find(o => o.FinishingInId == finIn.Identity).Select(finInItem => new GarmentFinishingInItemDto(finInItem)
                {

                }).ToList()
            }
            ).FirstOrDefault();
            var stream = GarmentFinishingInSubconPDFTemplate.Generate(garmentFinishingInDto, buyer);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentFinishingInDto.FinishingInNo}.pdf"
            };
        }

    }
}
