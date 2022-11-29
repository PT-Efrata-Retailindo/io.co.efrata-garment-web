using ExtCore.Data.Abstractions;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapDestinations.Repositories;
using System;

namespace Manufactures.Domain.GarmentScrapDestinations.Commands
{
    public class UpdateGarmentScrapDestinationCommand : ICommand<GarmentScrapDestination>
    {
        public Guid Identity { get; private set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
        public class UpdateGarmentScrapDestinationCommandValidator : AbstractValidator<UpdateGarmentScrapDestinationCommand>
        {
            public UpdateGarmentScrapDestinationCommandValidator(IStorage storage)
            {
                IGarmentScrapDestinationRepository _garmentScrapDestinationRepository = storage.GetRepository<IGarmentScrapDestinationRepository>();
                RuleFor(r => r.Code).Must((c) =>
                {
                    var a = _garmentScrapDestinationRepository.Find(s => s.Code == c);
                    return a == null || a.Count < 1;
                }).WithMessage("Kode Tujuan Barang sudah ada").When(s => s.Code != null);
                RuleFor(r => r.Code).NotNull().WithMessage("Kode Tujuan Barang Aval harus diisi");
                RuleFor(r => r.Name).Must((c) =>
                {
                    var a = _garmentScrapDestinationRepository.Find(s => s.Name == c);
                    return a == null || a.Count < 1;
                }).WithMessage("Nama Tujuan Barang sudah ada").When(s => s.Name != null);
                RuleFor(r => r.Name).NotNull().WithMessage("Nama Tujuan Barang Aval harus diisi");

            }
        }
    }
}