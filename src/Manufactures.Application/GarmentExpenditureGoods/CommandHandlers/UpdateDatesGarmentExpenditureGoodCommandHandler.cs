using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentExpenditureGoods.CommandHandlers
{
    public class UpdateDatesGarmentExpenditureGoodCommandHandler : ICommandHandler<UpdateDatesGarmentExpenditureGoodCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;

        public UpdateDatesGarmentExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var ExGoods = _garmentExpenditureGoodRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentExpenditureGood(a)).ToList();

            foreach (var model in ExGoods)
            {
                model.SetExpenditureDate(request.Date);
                model.Modify();
                await _garmentExpenditureGoodRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
