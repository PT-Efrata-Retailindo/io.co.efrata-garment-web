using ExtCore.Data.Abstractions;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapSources.Repositories;

namespace Manufactures.Domain.GarmentScrapSources.Commands
{
    public class PlaceGarmentScrapSourceCommand : ICommand<GarmentScrapSource>
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public class PlaceGarmentScrapSourceCommandValidator : AbstractValidator<PlaceGarmentScrapSourceCommand>
        {
            public PlaceGarmentScrapSourceCommandValidator(IStorage storage)
            {
                IGarmentScrapSourceRepository _garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
                RuleFor(r => r.Code).Must((c) =>
                {
                    var a = _garmentScrapSourceRepository.Find(s => s.Code == c);
                    return a == null || a.Count < 1;
                }).WithMessage("Kode Jenis Barang sudah di input").When(s => s.Code != null);
                RuleFor(r => r.Code).NotNull().WithMessage("Kode Asal Barang Aval harus diisi");
                RuleFor(r => r.Name).Must((c) =>
                {
                    var a = _garmentScrapSourceRepository.Find(s => s.Name == c);
                    return a == null || a.Count < 1;
                }).WithMessage("Nama Jenis Barang sudah ada").When(s => s.Name != null);
                RuleFor(r => r.Name).NotNull().WithMessage("Nama Asal Barang Aval harus diisi");
            }

        }

    }
}
