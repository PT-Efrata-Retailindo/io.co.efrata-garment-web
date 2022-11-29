using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentCuttingAdjustments.ValueObjects;
using FluentValidation;
using System.Linq;

namespace Manufactures.Domain.GarmentCuttingAdjustments.Commands
{
    public class PlaceGarmentCuttingAdjustmentCommand : ICommand<GarmentCuttingAdjustment>
    {
        public string AdjustmentNo { get; set; }
        public string CutInNo { get; set; }
        public Guid CutInId { get; set; }
        public string RONo { get; set; }
        public decimal TotalFC { get; set; }
        public decimal TotalActualFC { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalActualQuantity { get; set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset? AdjustmentDate { get; set; }
        public string AdjustmentDesc { get; set; }
        public List<GarmentCuttingAdjustmentItemValueObject> Items { get; set; }

    }
    public class PlaceGarmentCuttingAdjustmentCommandValidator : AbstractValidator<PlaceGarmentCuttingAdjustmentCommand>
    {
        public PlaceGarmentCuttingAdjustmentCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.AdjustmentDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.TotalActualFC).GreaterThan(0).WithMessage("Actual FC harus lebih dari 0");
            RuleFor(r => r.TotalActualQuantity).GreaterThan(0).WithMessage("Jumlah Aktual harus lebih dari 0");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentCuttingAdjustmentItemValueObjectValidator());
        }
    }

    class GarmentCuttingAdjustmentItemValueObjectValidator : AbstractValidator<GarmentCuttingAdjustmentItemValueObject>
    {
        public GarmentCuttingAdjustmentItemValueObjectValidator()
        {
            RuleFor(r => r.ActualQuantity)
                .LessThanOrEqualTo(r => r.Quantity)
                .WithMessage(x => $"'Jumlah Aktual' tidak boleh lebih dari '{x.Quantity}'.")
                .When(w => w.IsSave);
        }
    }
}
