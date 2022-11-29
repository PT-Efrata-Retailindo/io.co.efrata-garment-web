using ExtCore.Data.Abstractions;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.Commands
{
    public class UpdateGarmentScrapSourceCommand : ICommand<GarmentScrapSource>
    {
        public Guid Identity { get; private set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
        public class UpdateGarmentScrapSourceCommandValidator : AbstractValidator<UpdateGarmentScrapSourceCommand>
        {
            public UpdateGarmentScrapSourceCommandValidator(IStorage storage)
            {
                IGarmentScrapSourceRepository _garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
                RuleFor(r => r.Code).Must((c) =>
                {
                    var a = _garmentScrapSourceRepository.Find(s => s.Code == c);
                    return a == null || a.Count < 1;
                }).WithMessage("Kode Asal Barang sudah ada").When(s => s.Code != null);
                RuleFor(r => r.Code).NotNull().WithMessage("Kode Asal Barang Aval harus diisi");
                RuleFor(r => r.Name).Must((c) =>
                {
                    var a = _garmentScrapSourceRepository.Find(s => s.Name == c);
                    return a == null || a.Count < 1;
                }).WithMessage("Nama Asal Barang sudah ada").When(s => s.Name != null);
                RuleFor(r => r.Name).NotNull().WithMessage("Nama Asal Barang Aval harus diisi");

            }
        }
    }
}
