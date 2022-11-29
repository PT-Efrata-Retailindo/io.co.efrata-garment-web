using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentReturGoodReturns.Commands
{
    public class PlaceGarmentExpenditureGoodReturnCommand : ICommand<GarmentExpenditureGoodReturn>
    {
        public string ReturGoodNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string ReturType { get; set; }
        public string ExpenditureNo { get; set; }
        public string DONo { get; set; }
        public string URNNo { get; set; }
        public string BCNo { get; set; }
        public string BCType { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public Buyer Buyer { get; set; }
        public DateTimeOffset? ReturDate { get; set; }
        public DateTimeOffset? ExpenditureDate { get; set; }
        public string Invoice { get; set; }
        public string ReturDesc { get; set; }
        public List<GarmentExpenditureGoodReturnItemValueObject> Items { get; set; }
        public double Price { get; set; }
    }

    public class PlaceGarmentReturGoodCommandValidator : AbstractValidator<PlaceGarmentExpenditureGoodReturnCommand>
    {
        public PlaceGarmentReturGoodCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.ExpenditureNo).NotNull();
            RuleFor(r => r.DONo).NotNull();
            RuleFor(r => r.URNNo).NotNull();
            RuleFor(r => r.BCNo).NotNull();
            RuleFor(r => r.BCType).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.ReturDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Tidak Boleh Kosong");
            RuleFor(r => r.ReturDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.ReturDate).NotNull().GreaterThan(r => r.ExpenditureDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Tidak Boleh Kurang dari tanggal {r.ExpenditureDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Invoice).NotEmpty();
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.isSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentExpenditureGoodItemValueObjectValidator());
        }
    }

    class GarmentExpenditureGoodItemValueObjectValidator : AbstractValidator<GarmentExpenditureGoodReturnItemValueObject>
    {
        public GarmentExpenditureGoodItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.")
                .When(a => a.isSave);

            RuleFor(r => r.Quantity)
               .LessThanOrEqualTo(r => r.StockQuantity)
               .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.StockQuantity}'.").When(w => w.isSave == true);

        }
    }
}
