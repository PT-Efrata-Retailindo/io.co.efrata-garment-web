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
    public class PostGarmentSampleRequestCommandHandler : ICommandHandler<PostGarmentSampleRequestCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;
        public PostGarmentSampleRequestCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
        }

        public async Task<int> Handle(PostGarmentSampleRequestCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Samples = _GarmentSampleRequestRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleRequest(a)).ToList();
            int count = 0;
            foreach (var model in Samples)
            {
                count++;
                model.SetIsPosted(request.Posted);
                model.SetIsReceived(false);
                model.SetIsRejected(false);
                model.SetIsRevised(false);
                if (String.IsNullOrEmpty(model.SampleRequestNo))
                {
                    count++;
                    model.SetSampleRequestNo(GenerateSampleRequestNo(model, count));
                }
                model.Modify();
                await _GarmentSampleRequestRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }


        private string GenerateSampleRequestNo(GarmentSampleRequest request,int count)
        {
            var now = DateTime.Now;
            var day = now.ToString("dd");
            var year = now.ToString("yyyy");
            var month = now.ToString("MM");
            var code = request.BuyerCode;

            var prefix = $"{code}/{day}{month}{year}/";

            var lastSampleRequestNo = _GarmentSampleRequestRepository.Query.Where(w => w.SampleRequestNo.StartsWith(prefix) && w.SampleRequestNo!=null)
                .OrderByDescending(o => o.SampleRequestNo)
                .Select(s => int.Parse(s.SampleRequestNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SampleRequestNo = $"{prefix}{(lastSampleRequestNo + count).ToString("D3")}";

            return SampleRequestNo;
        }
    }
}
