using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands
{
    public class UpdateGarmentSubconCustomsInCommand : ICommand<GarmentSubconCustomsIn>
    {
        public Guid Identity { get; private set; }
        public string BcNo { get; set; }
        public DateTimeOffset? BcDate { get; set; }
        public string BcType { get; set; }
        public string SubconType { get; set; }
        public Guid SubconContractId { get; set; }
        public string SubconContractNo { get; set; }
        public Supplier Supplier { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentSubconCustomsInItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentSubconCustomsInCommandValidator : AbstractValidator<UpdateGarmentSubconCustomsInCommand>
    {
        public UpdateGarmentSubconCustomsInCommandValidator()
        {
            RuleFor(r => r.BcNo).NotNull();
            RuleFor(r => r.BcDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.BcType).NotNull();
            RuleFor(r => r.SubconType).NotNull();
            RuleFor(r => r.SubconContractId).NotNull();
            RuleFor(r => r.SubconContractNo).NotNull();
            RuleFor(r => r.Supplier.Id).NotEmpty().OverridePropertyName("Supplier").When(w => w.Supplier != null);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Data Belum Ada yang dipilih").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconCustomsInItemValueObjectValidator());
        }
    }
}
