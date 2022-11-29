using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands
{
    public class PlaceGarmentSampleDeliveryReturnCommand : ICommand<GarmentSampleDeliveryReturn>
    {
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
        public List<GarmentSampleDeliveryReturnItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentSampleDeliveryReturnCommandValidator : AbstractValidator<PlaceGarmentSampleDeliveryReturnCommand>
    {
        public PlaceGarmentSampleDeliveryReturnCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull().WithMessage("Unit Tidak Boleh Kosong");
            RuleFor(r => r.UnitDONo).NotNull().WithMessage("No Unit DO Tidak Boleh Kosong");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Storage).NotNull().WithMessage("Gudang Tidak Boleh Kosong");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentSampleDeliveryReturnItemValueObjectValidator());
        }
    }
    class GarmentSampleDeliveryReturnItemValueObjectValidator : AbstractValidator<GarmentSampleDeliveryReturnItemValueObject>
    {
        public GarmentSampleDeliveryReturnItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("Jumlah harus lebih besar dari 0")
                .When(w => w.IsSave == true);

            RuleFor(r => r.Quantity).LessThanOrEqualTo(r => r.QuantityUENItem).WithMessage(r => $"Jumlah tidak boleh Lebih Besar dari {r.QuantityUENItem}").When(w => w.Product.Name != "FABRIC" && w.IsSave == true);
            RuleFor(r => r.Quantity).LessThanOrEqualTo(r => r.RemainingQuantityPreparingItem).WithMessage(r => $"Jumlah tidak boleh Lebih Besar dari {r.RemainingQuantityPreparingItem}").When(w => w.Product.Name == "FABRIC" && w.IsSave == true);
        }
    }
}
