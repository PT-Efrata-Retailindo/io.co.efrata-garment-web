using ExtCore.Data.Abstractions;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentScrapClassifications.Commands
{
	public class PlaceGarmentScrapClassificationCommand : ICommand<GarmentScrapClassification>
	{
		
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public class PlaceGarmentScrapClassificationCommandValidator : AbstractValidator<PlaceGarmentScrapClassificationCommand>
		{
			public PlaceGarmentScrapClassificationCommandValidator(IStorage storage)
			{
				IGarmentScrapClassificationRepository _garmentScrapClassificationRepository= storage.GetRepository<IGarmentScrapClassificationRepository>();
				RuleFor(r => r.Code).Must((c) =>
				{
					var a=_garmentScrapClassificationRepository.Find(s => s.Code == c);
					return a == null || a.Count <1 ;
				}).WithMessage("Kode Jenis Barang sudah di input").When(s=>s.Code != null);
				RuleFor(r => r.Code).NotNull().WithMessage("Kode Jenis Barang Aval harus diisi");
				RuleFor(r => r.Name).Must((c) =>
				{
					var a = _garmentScrapClassificationRepository.Find(s => s.Name == c);
					return a == null || a.Count < 1;
				}).WithMessage("Nama Jenis Barang sudah ada").When(s => s.Name != null);
				RuleFor(r => r.Name).NotNull().WithMessage("Nama Jenis Barang Aval harus diisi");
			}
		
		}

	}
}
