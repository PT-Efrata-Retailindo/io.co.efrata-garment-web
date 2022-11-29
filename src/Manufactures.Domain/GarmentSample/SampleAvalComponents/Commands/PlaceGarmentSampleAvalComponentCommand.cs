using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands
{
    public class PlaceGarmentSampleAvalComponentCommand : ICommand<GarmentSampleAvalComponent>
    {
        public UnitDepartment Unit { get; set; }
        public string SampleAvalComponentType { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? CuttingDate { get; set; }
        public DateTimeOffset? SewingDate { get; set; }
        public decimal Price { get; set; }
        public bool IsReceived { get; set; }

        public List<PlaceGarmentSampleAvalComponentItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentSampleAvalComponentCommandValidator : AbstractValidator<PlaceGarmentSampleAvalComponentCommand>
    {
        public PlaceGarmentSampleAvalComponentCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.Unit.Code).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null && w.Unit.Id > 0);

            RuleFor(r => r.SampleAvalComponentType).NotNull();
            RuleFor(r => r.RONo).NotNull();

            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");

            RuleFor(r => r.Comodity).NotNull().When(w => w.SampleAvalComponentType == "SEWING");
            RuleFor(r => r.Comodity.Id).NotEmpty().OverridePropertyName("Comodity").When(w => w.SampleAvalComponentType == "SEWING" && w.Comodity != null);

            RuleFor(r => r.Date).NotEmpty();

            RuleFor(r => r.Date).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Date).NotNull().GreaterThan(r => r.SewingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Tidak Boleh Kurang dari tanggal {r.SewingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r => r.SampleAvalComponentType == "SEWING" && r.SewingDate != null);
            RuleFor(r => r.Date).NotNull().GreaterThan(r => r.CuttingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Tidak Boleh Kurang dari tanggal {r.CuttingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r => r.SampleAvalComponentType == "CUTTING" && r.CuttingDate != null);

            RuleFor(r => r.Items).NotNull().OverridePropertyName("Item");
            RuleFor(r => r.Items.Where(w => w.IsSave)).NotEmpty().OverridePropertyName("Item").When(w => w.Items != null);

            RuleForEach(r => r.Items).SetValidator(command => new PlaceGarmentSampleAvalComponentItemValidator(command));
        }
    }

    class PlaceGarmentSampleAvalComponentItemValidator : AbstractValidator<PlaceGarmentSampleAvalComponentItemValueObject>
    {
        public PlaceGarmentSampleAvalComponentItemValidator(PlaceGarmentSampleAvalComponentCommand placeGarmentSampleAvalComponentCommand)
        {
            RuleFor(r => r.Product).NotNull().When(w => w.IsSave);

            RuleFor(r => r.Quantity).GreaterThan(0).When(w => w.IsSave);
            RuleFor(r => r.Quantity).LessThanOrEqualTo(r => r.SourceQuantity).When(w => w.IsSave);
        }
    }
}
