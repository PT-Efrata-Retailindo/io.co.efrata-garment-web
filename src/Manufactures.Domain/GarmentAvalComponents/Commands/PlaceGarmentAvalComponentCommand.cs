using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalComponents.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Manufactures.Domain.GarmentAvalComponents.Commands
{
    public class PlaceGarmentAvalComponentCommand : ICommand<GarmentAvalComponent>
    {
        public UnitDepartment Unit { get; set; }
        public string AvalComponentType { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? CuttingDate { get; set; }
        public DateTimeOffset? SewingDate { get; set; }
        public decimal Price { get; set; }
        public bool IsReceived { get; set; }
        public List<PlaceGarmentAvalComponentItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentAvalComponentCommandValidator : AbstractValidator<PlaceGarmentAvalComponentCommand>
    {
        public PlaceGarmentAvalComponentCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.Unit.Code).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null && w.Unit.Id > 0);

            RuleFor(r => r.AvalComponentType).NotNull();
            RuleFor(r => r.RONo).NotNull();

            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");

            RuleFor(r => r.Comodity).NotNull().When(w => w.AvalComponentType == "SEWING");
            RuleFor(r => r.Comodity.Id).NotEmpty().OverridePropertyName("Comodity").When(w => w.AvalComponentType == "SEWING" && w.Comodity != null);

            RuleFor(r => r.Date).NotEmpty();

            RuleFor(r => r.Date).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Date).NotNull().GreaterThan(r => r.SewingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Tidak Boleh Kurang dari tanggal {r.SewingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r => r.AvalComponentType == "SEWING" && r.SewingDate!=null);
            RuleFor(r => r.Date).NotNull().GreaterThan(r => r.CuttingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Tidak Boleh Kurang dari tanggal {r.CuttingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r => r.AvalComponentType == "CUTTING" && r.CuttingDate!=null);

            RuleFor(r => r.Items).NotNull().OverridePropertyName("Item");
            RuleFor(r => r.Items.Where(w => w.IsSave)).NotEmpty().OverridePropertyName("Item").When(w => w.Items != null);

            RuleForEach(r => r.Items).SetValidator(command => new PlaceGarmentAvalComponentItemValidator(command));
        }
    }

    class PlaceGarmentAvalComponentItemValidator : AbstractValidator<PlaceGarmentAvalComponentItemValueObject>
    {
        public PlaceGarmentAvalComponentItemValidator(PlaceGarmentAvalComponentCommand placeGarmentAvalComponentCommand)
        {
            RuleFor(r => r.Product).NotNull().When(w => w.IsSave);

            RuleFor(r => r.Quantity).GreaterThan(0).When(w => w.IsSave);
            RuleFor(r => r.Quantity).LessThanOrEqualTo(r => r.SourceQuantity).When(w => w.IsSave);
        }
    }
}
