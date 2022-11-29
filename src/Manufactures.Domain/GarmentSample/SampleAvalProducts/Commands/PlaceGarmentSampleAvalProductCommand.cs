using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Globalization;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands
{
    public class PlaceGarmentSampleAvalProductCommand : ICommand<GarmentSampleAvalProduct>
    {
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset? AvalDate { get; set; }
        public DateTimeOffset? PreparingDate { get; set; }
        public UnitDepartment Unit { get; set; }
        public List<GarmentSampleAvalProductItemValueObject> Items { get; set; }
    }
    public class PlaceGarmentSampleAvalProductCommandValidator : AbstractValidator<PlaceGarmentSampleAvalProductCommand>
    {
        public PlaceGarmentSampleAvalProductCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.RONo).NotEmpty().WithMessage("Nomor RO Tidak Boleh Kosong");
            RuleFor(r => r.AvalDate).NotNull().WithMessage("Tanggal Aval Tidak Boleh Kosong");
            RuleFor(r => r.AvalDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Aval Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.AvalDate).NotNull().GreaterThan(r => r.PreparingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Aval Tidak Boleh Kurang dari tanggal {r.PreparingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(a => a.PreparingDate != null);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item").WithMessage("Item Tidak Boleh Kosong");
            RuleForEach(r => r.Items).SetValidator(new GarmentSampleAvalProductItemValueObjectValidator());
        }
    }

    class GarmentSampleAvalProductItemValueObjectValidator : AbstractValidator<GarmentSampleAvalProductItemValueObject>
    {
        public GarmentSampleAvalProductItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah Aval' harus lebih dari '0'.");

            RuleFor(r => r.Quantity)
                 .LessThanOrEqualTo(r => r.PreparingQuantity)
                 .WithMessage(x => $"'Jumlah Aval' tidak boleh lebih dari '{x.PreparingQuantity}'.");
        }
    }
}
