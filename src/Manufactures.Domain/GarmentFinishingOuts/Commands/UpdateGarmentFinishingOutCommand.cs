using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts.Commands
{
    public class UpdateGarmentFinishingOutCommand : ICommand<GarmentFinishingOut>
    {
        public Guid Identity { get; private set; }
        public string FinishingOutNo { get; set; }
        public UnitDepartment Unit { get; set; }
        public string FinishingTo { get; set; }
        public UnitDepartment UnitTo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? FinishingOutDate { get; set; }
        public DateTimeOffset? FinishingInDate { get; set; }
        public bool IsDifferentSize { get; set; }
        public List<GarmentFinishingOutItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentFinishingOutCommandValidator : AbstractValidator<UpdateGarmentFinishingOutCommand>
    {
        public UpdateGarmentFinishingOutCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.UnitTo).NotNull();
            RuleFor(r => r.UnitTo.Id).NotEmpty().OverridePropertyName("UnitTo").When(w => w.UnitTo != null);
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.FinishingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.FinishingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Finishing Out Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.FinishingOutDate).NotNull().GreaterThan(r => r.FinishingInDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Finishing Out Tidak Boleh Kurang dari tanggal {r.FinishingInDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentFinishingOutItemValueObjectValidator());
        }
    }
}
