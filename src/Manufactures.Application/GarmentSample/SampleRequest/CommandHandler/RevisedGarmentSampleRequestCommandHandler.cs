using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleRequest.CommandHandler
{
    public class RevisedGarmentSampleRequestCommandHandler : ICommandHandler<RevisedGarmentSampleRequestCommand, GarmentSampleRequest>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;

        public RevisedGarmentSampleRequestCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
        }

        public async Task<GarmentSampleRequest> Handle(RevisedGarmentSampleRequestCommand request, CancellationToken cancellationToken)
        {
            var SampleRequest = _GarmentSampleRequestRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleRequest(o)).Single();

            SampleRequest.SetIsReceived(false);
            SampleRequest.SetIsRevised(request.IsRevised);
            SampleRequest.SetRevisedDate(request.RevisedDate);
            SampleRequest.SetRevisedBy(request.RevisedBy);
            SampleRequest.SetRevisedReason(request.RevisedReason);

            SampleRequest.Modify();

            await _GarmentSampleRequestRepository.Update(SampleRequest);

            _storage.Save();

            return SampleRequest;
        }
    }
}
