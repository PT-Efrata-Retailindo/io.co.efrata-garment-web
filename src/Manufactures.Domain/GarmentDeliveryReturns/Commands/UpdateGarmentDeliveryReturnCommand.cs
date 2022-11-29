using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Manufactures.Domain.GarmentDeliveryReturns.Commands
{
    public class UpdateGarmentDeliveryReturnCommand : ICommand<GarmentDeliveryReturn>
    {
        public void SetId(Guid id) { Identity = id; }
        public Guid Identity { get; private set; }
        public string DRNo { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public int UnitDOId { get; set; }
        public string UnitDONo { get; set; }
        public int UENId { get; set; }
        public string PreparingId { get; set; }
        public DateTimeOffset? ReturnDate { get; set; }
        public string ReturnType { get; set; }
        public UnitDepartment Unit { get; set; }
        public Storage Storage { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentDeliveryReturnItemValueObject> Items { get; set; }
    }
    public class UpdateGarmentDeliveryReturnCommandValidator : AbstractValidator<UpdateGarmentDeliveryReturnCommand>
    {
        public UpdateGarmentDeliveryReturnCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull().WithMessage("Unit Tidak Boleh Kosong");
            RuleFor(r => r.UnitDONo).NotNull().WithMessage("No Unit DO Tidak Boleh Kosong");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Storage).NotNull().WithMessage("Gudang Tidak Boleh Kosong");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentDeliveryReturnItemValueObjectValidator());
        }
    }
}