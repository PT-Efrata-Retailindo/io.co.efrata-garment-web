using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.Commands
{
    public class UpdateGarmentExpenditureGoodCommand : ICommand<GarmentExpenditureGood>
    {
        public Guid Identity { get; private set; }
        public string ExpenditureGoodNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string ExpenditureType { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public Buyer Buyer { get; set; }
        public DateTimeOffset ExpenditureDate { get; set; }
        public string Invoice { get; set; }
        public int PackingListId { get; set; }
        public string ContractNo { get; set; }
        public double Carton { get; set; }
        public string Description { get; set; }
        public bool IsReceived { get; set; }
        public List<GarmentExpenditureGoodItemValueObject> Items { get; set; }
		public int InvoiceId { get; set; }
		public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentExpenditureGoodCommandValidator : AbstractValidator<UpdateGarmentExpenditureGoodCommand>
    {
        public UpdateGarmentExpenditureGoodCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.ExpenditureDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Tidak Boleh Kosong");
            RuleFor(r => r.ExpenditureDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Invoice).NotEmpty().When(w => w.ExpenditureType == "EXPORT");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            //RuleForEach(r => r.Items).SetValidator(new GarmentExpenditureGoodItemValueObjectValidator());
        }
    }
}
