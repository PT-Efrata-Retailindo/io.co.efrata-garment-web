using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentComodityPrices.Commands
{
    public class UpdateGarmentComodityPriceCommand : ICommand<List<GarmentComodityPrice>>
    {
        public Guid Identity { get; private set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset Date { get; set; }

        public List<GarmentComodityPriceItemValueObject> Items { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentComodityPriceCommandValidator : AbstractValidator<UpdateGarmentComodityPriceCommand>
    {
        public UpdateGarmentComodityPriceCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.Date).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Update Tidak Boleh Kosong");

            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(r=>new GarmentUpdateComodityPriceItemValueObjectValidator(r));
        }

    }
    public class GarmentUpdateComodityPriceItemValueObjectValidator : AbstractValidator<GarmentComodityPriceItemValueObject>
    {
        public GarmentUpdateComodityPriceItemValueObjectValidator(UpdateGarmentComodityPriceCommand updateGarmentComodityPriceCommand)
        {
            RuleFor(r => r.Date).Must((date) =>
            {
                var today = DateTimeOffset.Now;
                return updateGarmentComodityPriceCommand.Date.AddHours(7).Date==today.Date? true : updateGarmentComodityPriceCommand.Date.AddHours(7).Date > date.Date;
            }).When(r=>r.Price!=r.NewPrice)
            .WithMessage("Tanggal Update tidak boleh kurang dari atau sama dengan tanggal update terakhir")
            .OverridePropertyName("Comodity");

            RuleFor(r => r.Price)
                 .GreaterThan(0)
                 .WithMessage("'Tarif' harus lebih dari '0'.");
            RuleFor(r => r.NewPrice)
                 .GreaterThan(0)
                 .WithMessage("'Tarif Baru' harus lebih dari '0'.");
        }
    }
}
