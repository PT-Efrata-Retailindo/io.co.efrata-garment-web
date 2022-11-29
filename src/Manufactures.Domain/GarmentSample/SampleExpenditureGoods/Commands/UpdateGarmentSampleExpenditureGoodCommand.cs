using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands
{
    public class UpdateGarmentSampleExpenditureGoodCommand : ICommand<GarmentSampleExpenditureGood>
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
        public List<GarmentSampleExpenditureGoodItemValueObject> Items { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentSampleExpenditureGoodCommandValidator : AbstractValidator<UpdateGarmentSampleExpenditureGoodCommand>
    {
        public UpdateGarmentSampleExpenditureGoodCommandValidator()
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
            //RuleForEach(r => r.Items).SetValidator(new GarmentSampleExpenditureGoodItemValueObjectValidator());
        }
    }
}
