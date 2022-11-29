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
    public class RejectGarmentSampleRequestCommandHandler : ICommandHandler<RejectGarmentSampleRequestCommand, GarmentSampleRequest>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;

        public RejectGarmentSampleRequestCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
        }

        public async Task<GarmentSampleRequest> Handle(RejectGarmentSampleRequestCommand request, CancellationToken cancellationToken)
        {
            var SampleRequest = _GarmentSampleRequestRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleRequest(o)).Single();

            SampleRequest.SetIsRejected(request.IsRejected);
            SampleRequest.SetRejectedDate(request.RejectedDate);
            SampleRequest.SetRejectedBy(request.RejectedBy);
            SampleRequest.SetRejectedReason(request.RejectedReason);

            SampleRequest.Modify();

            await _GarmentSampleRequestRepository.Update(SampleRequest);

            _storage.Save();

            return SampleRequest;
        }
    }
}
