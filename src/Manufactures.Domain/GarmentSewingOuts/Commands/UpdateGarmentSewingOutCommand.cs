using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.Commands
{
    public class UpdateGarmentSewingOutCommand : ICommand<GarmentSewingOut>
    {
        public Guid Identity { get; private set; }
        public string SewingOutNo { get; set; }
        public Buyer Buyer { get; set; }
        public UnitDepartment Unit { get; set; }
        public string SewingTo { get; set; }
        public UnitDepartment UnitTo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? SewingOutDate { get; set; }
        public DateTimeOffset? SewingInDate { get; set; }
        public bool IsDifferentSize { get; set; }
        public List<GarmentSewingOutItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentSewingOutCommandValidator : AbstractValidator<UpdateGarmentSewingOutCommand>
    {
        public UpdateGarmentSewingOutCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.UnitTo).NotNull();
            RuleFor(r => r.UnitTo.Id).NotEmpty().OverridePropertyName("UnitTo").When(w => w.UnitTo != null);
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.SewingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.SewingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sewing Out Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.SewingOutDate).NotNull().GreaterThan(r => r.SewingInDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Sewing Out Tidak Boleh Kurang dari tanggal {r.SewingInDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSewingOutItemValueObjectValidator());
        }
    }
}
