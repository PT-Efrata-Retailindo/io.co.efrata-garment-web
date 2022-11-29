using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingDOs.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Manufactures.Domain.GarmentSewingDOs.Commands
{
    public class PlaceGarmentSewingDOCommand : ICommand<GarmentSewingDO>
    {
        public string SewingDONo { get; set; }
        public Guid CuttingOutId { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? SewingDODate { get; set; }
        public List<GarmentSewingDOItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentSewingDOCommandValidator : AbstractValidator<PlaceGarmentSewingDOCommand>
    {
        public PlaceGarmentSewingDOCommandValidator()
        {
            RuleFor(r => r.UnitFrom).NotNull();
            RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("UnitFrom").When(w => w.Unit != null);

            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.SewingDODate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            //RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSewingDOItemValueObjectValidator());
        }
    }

    class GarmentSewingDOItemValueObjectValidator : AbstractValidator<GarmentSewingDOItemValueObject>
    {
        public GarmentSewingDOItemValueObjectValidator()
        {
        }
    }
}