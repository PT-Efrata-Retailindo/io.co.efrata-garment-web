using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns.Commands
{
    public class PlaceGarmentCuttingInCommand : ICommand<GarmentCuttingIn>
    {
        public string CutInNo { get; set; }
        public string CuttingType { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset? CuttingInDate { get; set; }
        public DateTimeOffset? PreparingDate { get; set; }
        public double FC { get; set; }
        public List<GarmentCuttingInItemValueObject> Items { get; set; }
        public string CuttingFrom { get; set; }
        public double Price { get; set; }
    }

    public class PlaceGarmentCuttingInCommandValidator : AbstractValidator<PlaceGarmentCuttingInCommand>
    {
        public PlaceGarmentCuttingInCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.CuttingInDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.CuttingInDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Cutting In Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.CuttingInDate).NotNull().GreaterThan(r => r.PreparingDate.GetValueOrDefault().Date).WithMessage(r=>$"Tanggal Cutting In Tidak Boleh Kurang dari tanggal {r.PreparingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.FC).GreaterThan(0);
            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentCuttingInItemValueObjectValidator());
        }
    }

    class GarmentCuttingInItemValueObjectValidator : AbstractValidator<GarmentCuttingInItemValueObject>
    {
        public GarmentCuttingInItemValueObjectValidator()
        {
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Detail");
            RuleForEach(r => r.Details).SetValidator(new GarmentCuttingInDetailValueObjectValidator());
        }
    }

    class GarmentCuttingInDetailValueObjectValidator : AbstractValidator<GarmentCuttingInDetailValueObject>
    {
        public GarmentCuttingInDetailValueObjectValidator()
        {
            // not supported at this time: FluentValidation 8.0.100 but some libs are incompatible
            //When(w => w.IsSave, () =>
            //{
            //    //RuleFor(r => r.PreparingUom).NotNull();
            //    //RuleFor(r => r.PreparingUom.Unit).NotEmpty().OverridePropertyName("PreparingUom").When(w => w.PreparingUom != null);
            //    ////RuleFor(r => r.PreparingUom).Must(m => !string.IsNullOrWhiteSpace(m.Unit)).When(w => w.PreparingUom != null);

            //    RuleFor(r => r.CuttingInQuantity).GreaterThan(0);

            //    RuleFor(r => r.PreparingQuantity).LessThanOrEqualTo(r => r.PreparingRemainingQuantity);
            //});

            RuleFor(r => r.CuttingInQuantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah Potong' harus lebih dari '0'.")
                .When(w => w.IsSave);

            RuleFor(r => r.PreparingQuantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah Preparing Out' harus lebih dari '0'.")
                .When(w => w.IsSave);

            RuleFor(r => r.PreparingQuantity)
                .LessThanOrEqualTo(r => r.PreparingRemainingQuantity)
                .WithMessage(x=>$"'Jumlah Preparing Out' tidak boleh lebih dari '{x.PreparingRemainingQuantity}'.")
                .When(w => w.IsSave);

        }
    }
}
