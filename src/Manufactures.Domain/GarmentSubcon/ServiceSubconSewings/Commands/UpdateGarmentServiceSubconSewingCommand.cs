using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands
{
    public class UpdateGarmentServiceSubconSewingCommand : ICommand<GarmentServiceSubconSewing>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconSewingNo { get; set; }
        public DateTimeOffset? ServiceSubconSewingDate { get; set; }
        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public List<GarmentServiceSubconSewingItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentServiceSubconSewingCommandValidator : AbstractValidator<UpdateGarmentServiceSubconSewingCommand>
    {
        public UpdateGarmentServiceSubconSewingCommandValidator()
        {
            //RuleFor(r => r.Unit).NotNull();
            //RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.ServiceSubconSewingDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Kosong");
            RuleFor(r => r.ServiceSubconSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Buyer).NotNull();
            RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconSewingItemValueObjectValidator());
        }
    }


}

