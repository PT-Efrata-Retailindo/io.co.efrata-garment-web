using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns.Commands
{
    public class PlaceGarmentFinishingInCommand : ICommand<GarmentFinishingIn>
    {
        public string FinishingInNo { get;  set; }
        public string FinishingInType { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public UnitDepartment UnitFrom { get;  set; }
        public string Article { get;  set; }
        public string RONo { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public DateTimeOffset? FinishingInDate { get;  set; }
        public DateTimeOffset? SewingOutDate { get; set; }
        public List<GarmentFinishingInItemValueObject> Items { get; set; }
        public double Price { get; set; }
        public long DOId { get; set; }
        public string DONo { get; set; }
        public string SubconType { get; set; }

    }

    public class PlaceGarmentFinishingInCommandValidator : AbstractValidator<PlaceGarmentFinishingInCommand>
    {
        public PlaceGarmentFinishingInCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.UnitFrom).NotNull();
            RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("UnitFrom").When(w => w.UnitFrom != null);
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.FinishingInDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal FinishingIn Tidak Boleh Kosong");
            RuleFor(r => r.FinishingInDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal FinishingIn Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.FinishingInDate).NotNull().GreaterThanOrEqualTo(r => r.SewingOutDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal FinishingIn Tidak Boleh Kurang dari tanggal {r.SewingOutDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");

            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentFinishingInItemValueObjectValidator());
        }
    }

    class GarmentFinishingInItemValueObjectValidator : AbstractValidator<GarmentFinishingInItemValueObject>
    {
        public GarmentFinishingInItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");

        }
    }
}
