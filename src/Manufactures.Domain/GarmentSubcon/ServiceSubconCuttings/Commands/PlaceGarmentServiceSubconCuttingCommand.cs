using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands
{
    public class PlaceGarmentServiceSubconCuttingCommand : ICommand<GarmentServiceSubconCutting>
    {
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset? SubconDate { get; set; }
        public string SubconNo { get; set; }
        public string SubconType { get; set; }

        public bool IsUsed { get; set; }
        public Buyer Buyer { get; set; }
        public Uom Uom { get; set; }
        public int QtyPacking { get; set; }
        public List<GarmentServiceSubconCuttingItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentServiceSubconCuttingCommandValidator : AbstractValidator<PlaceGarmentServiceSubconCuttingCommand>
    {
        public PlaceGarmentServiceSubconCuttingCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.SubconDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.Uom).NotNull();
            RuleFor(r => r.Uom.Id).NotEmpty().OverridePropertyName("Uom").When(w => w.Uom != null);
            RuleFor(r => r.QtyPacking).NotEmpty().WithMessage("Jumlah Packing tidak boleh kosong!").OverridePropertyName("QtyPacking");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Data Belum Ada yang dipilih").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleFor(r => r.Buyer).NotNull();
            RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconCuttingItemValueObjectValidator());
        }
    }

    class GarmentServiceSubconCuttingItemValueObjectValidator : AbstractValidator<GarmentServiceSubconCuttingItemValueObject>
    {
        public GarmentServiceSubconCuttingItemValueObjectValidator()
        {

            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull() ;
            RuleFor(r => r.Comodity).NotNull();

            RuleFor(r => r.Comodity.Id).NotEmpty().OverridePropertyName("Comodity").When(w => w.Comodity != null);

            RuleFor(r => r.Details.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Detail data harus diisi").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Detail");
            RuleForEach(r => r.Details).SetValidator(new GarmentServiceSubconCuttingDetailValueObjectValidator());
        }
    }

    class GarmentServiceSubconCuttingDetailValueObjectValidator : AbstractValidator<GarmentServiceSubconCuttingDetailValueObject>
    {
        public GarmentServiceSubconCuttingDetailValueObjectValidator()
        {
            RuleFor(r => r.CuttingInQuantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah Potong' harus lebih dari '0'.")
                .When(w => w.IsSave);

            RuleFor(r => r.Quantity)
                .LessThanOrEqualTo(r => r.CuttingInQuantity)
                .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.CuttingInQuantity}'.")
                .When(w => w.IsSave);

            RuleFor(r => r.Sizes).NotEmpty().OverridePropertyName("Size").When(w => w.IsSave);
            RuleForEach(r => r.Sizes).SetValidator(new GarmentServiceSubconCuttingSizeValueObjectValidator());
        }
    }

    class GarmentServiceSubconCuttingSizeValueObjectValidator : AbstractValidator<GarmentServiceSubconCuttingSizeValueObject>
    {
        public GarmentServiceSubconCuttingSizeValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");
            RuleFor(r => r.Color).NotEmpty();
            RuleFor(r => r.Size).NotNull();
            RuleFor(r => r.Uom).NotNull();
        }
    }
}

