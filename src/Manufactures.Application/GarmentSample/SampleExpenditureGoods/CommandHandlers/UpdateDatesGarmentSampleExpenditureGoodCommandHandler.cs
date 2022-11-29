using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.CommandHandlers
{
    public class UpdateDatesGarmentSampleExpenditureGoodCommandHandler : ICommandHandler<UpdateDatesGarmentSampleExpenditureGoodCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleExpenditureGoodRepository _GarmentSampleExpenditureGoodRepository;

        public UpdateDatesGarmentSampleExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentSampleExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var ExGoods = _GarmentSampleExpenditureGoodRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleExpenditureGood(a)).ToList();

            foreach (var model in ExGoods)
            {
                model.SetExpenditureDate(request.Date);
                model.Modify();
                await _GarmentSampleExpenditureGoodRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
