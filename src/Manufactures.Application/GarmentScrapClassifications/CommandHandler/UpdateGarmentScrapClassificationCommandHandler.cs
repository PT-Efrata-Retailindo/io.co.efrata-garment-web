using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapClassifications;
using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Moonlay;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapClassifications.CommandHandler
{
	public class UpdateGarmentScrapClassificationCommandHandler : ICommandHandler<UpdateGarmentScrapClassificationCommand, GarmentScrapClassification>
	{
		private readonly IStorage _storage;

		private readonly IGarmentScrapClassificationRepository _garmentScrapClassificationRepository;

		public UpdateGarmentScrapClassificationCommandHandler(IStorage storage)
		{
			_storage = storage;
			_garmentScrapClassificationRepository = storage.GetRepository<IGarmentScrapClassificationRepository>();
		}

		public async Task<GarmentScrapClassification> Handle(UpdateGarmentScrapClassificationCommand request, CancellationToken cancellationToken)
		{
			var gscrapClassification = _garmentScrapClassificationRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentScrapClassification(o)).Single();

			if (gscrapClassification == null)
				throw Validator.ErrorValidation(("Identity", "Invalid Id: " + request.Identity));

			gscrapClassification.setCode(request.Code);
			gscrapClassification.setName(request.Name);
			gscrapClassification.setDescription(request.Description);
			gscrapClassification.Modify();
			await _garmentScrapClassificationRepository.Update(gscrapClassification);
			_storage.Save();
			return gscrapClassification;
		}

	}
}
