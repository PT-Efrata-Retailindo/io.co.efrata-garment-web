using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.Commands
{
    public class PlaceGarmentExpenditureGoodCommand : ICommand<GarmentExpenditureGood>
    {
        public string ExpenditureGoodNo { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string ExpenditureType { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public Buyer Buyer { get;  set; }
        public DateTimeOffset ExpenditureDate { get;  set; }
        public string Invoice { get;  set; }
        public int PackingListId { get; set; }
        public string ContractNo { get;  set; }
        public double Carton { get;  set; }
        public string Description { get;  set; }
        public List<GarmentExpenditureGoodItemValueObject> Items { get;  set; }
        public double Price { get; set; }
        public bool IsReceived { get; set; }
		public int InvoiceId { get; set; }
		public int ExpenditureGoodId { get; set; }
		public double TotalQty { get; set; }
    }

    public class PlaceGarmentExpenditureGoodCommandValidator : AbstractValidator<PlaceGarmentExpenditureGoodCommand>
    {
        public PlaceGarmentExpenditureGoodCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.ExpenditureDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Tidak Boleh Kosong");
            RuleFor(r => r.ExpenditureDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Invoice).NotEmpty().When(w=>w.ExpenditureType=="EXPORT" || w.ExpenditureType=="LOKAL");
            RuleFor(r => r.Carton)
                .GreaterThanOrEqualTo(0)
                .WithMessage("'Karton' harus lebih dari atau sama dengan '0'.");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.isSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentExpenditureGoodItemValueObjectValidator());
        }
    }

    class GarmentExpenditureGoodItemValueObjectValidator : AbstractValidator<GarmentExpenditureGoodItemValueObject>
    {
        public GarmentExpenditureGoodItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.")
                .When(a=>a.isSave);

            RuleFor(r => r.Quantity)
               .LessThanOrEqualTo(r => r.StockQuantity)
               .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.StockQuantity}'.").When(w => w.isSave == true);

        }
    }
}
