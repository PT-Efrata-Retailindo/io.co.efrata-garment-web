using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentLoadings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentLoadings.Commands
{
    public class PlaceGarmentLoadingCommand : ICommand<GarmentLoading>
    {
        public string LoadingNo { get; set; }
        public Guid SewingDOId { get; set; }
        public string SewingDONo { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset LoadingDate { get; set; }
        public DateTimeOffset? SewingDODate { get; set; }
        public double Price { get; set; }

        public List<GarmentLoadingItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentLoadingCommandValidator : AbstractValidator<PlaceGarmentLoadingCommand>
    {
        public PlaceGarmentLoadingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.SewingDONo).NotNull().WithMessage("Nomor DO Tidak Boleh Kosong");
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.LoadingDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Loading Tidak Boleh Kosong");
            RuleFor(r => r.LoadingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Loading Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.LoadingDate).NotNull().GreaterThan(r => r.SewingDODate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Loading Tidak Boleh Kurang dari tanggal {r.SewingDODate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r=>r.SewingDODate!=null);
            RuleFor(r => r.Comodity).NotNull();

            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentLoadingItemValueObjectValidator());
        }
    }

    class GarmentLoadingItemValueObjectValidator : AbstractValidator<GarmentLoadingItemValueObject>
    {
        public GarmentLoadingItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.")
                .When(w => w.IsSave);

            RuleFor(r => r.Quantity)
                .LessThanOrEqualTo(r => r.SewingDORemainingQuantity)
                .OverridePropertyName("Quantity")
                .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.SewingDORemainingQuantity}'.")
                .When(w => w.IsSave == true);
        }
    }
}
