using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands
{
    public class UpdateGarmentSubconCustomsOutCommand : ICommand<GarmentSubconCustomsOut>
    {
        public Guid Identity { get; private set; }
        public string CustomsOutNo { get; set; }
        public DateTimeOffset CustomsOutDate { get; set; }
        public string CustomsOutType { get; set; }
        public string SubconType { get; set; }
        public Guid SubconContractId { get; set; }
        public string SubconContractNo { get; set; }
        public Supplier Supplier { get; set; }
        public string Remark { get; set; }
        public double TotalQty { get; set; }
        public double UsedQty { get; set; }
        public string SubconCategory { get; set; }
        public virtual List<GarmentSubconCustomsOutItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
    public class UpdateGarmentSubconCustomsOutCommandValidator : AbstractValidator<UpdateGarmentSubconCustomsOutCommand>
    {
        public UpdateGarmentSubconCustomsOutCommandValidator()
        {
            RuleFor(r => r.SubconContractId).NotNull();
            RuleFor(r => r.SubconContractNo).NotNull();
            RuleFor(r => r.SubconCategory).NotNull();
            RuleFor(r => r.CustomsOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item tidak boleh kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconCustomsOutItemValueObjectValidator()).When(s => s.Items != null);
            //RuleFor(r => r.TotalQty)
            //     .LessThanOrEqualTo(r => r.UsedQty)
            //     .WithMessage(x => $"'Jumlah Total' tidak boleh lebih dari '{x.UsedQty}'.");
        }
    }
}
