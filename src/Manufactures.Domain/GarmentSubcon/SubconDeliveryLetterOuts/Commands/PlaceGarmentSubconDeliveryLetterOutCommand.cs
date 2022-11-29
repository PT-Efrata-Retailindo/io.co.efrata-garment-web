using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands
{
    public class PlaceGarmentSubconDeliveryLetterOutCommand : ICommand<GarmentSubconDeliveryLetterOut>
    {
        public string DLNo { get; set; }
        public string DLType { get; set; }
        public string ContractType { get; set; }
        public string ServiceType { get; set; }
        public DateTimeOffset DLDate { get; set; }

        public int UENId { get; set; }
        public string UENNo { get; set; }

        public string PONo { get; set; }
        public int EPOItemId { get; set; }

        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public double TotalQty { get; set; }
        public double UsedQty { get; set; }
        public string SubconCategory { get; set; }
        public int EPOId { get; set; }
        public string EPONo { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }

        public List<GarmentSubconDeliveryLetterOutItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentSubconDeliveryLetterOutCommandValidator : AbstractValidator<PlaceGarmentSubconDeliveryLetterOutCommand>
    {
        public PlaceGarmentSubconDeliveryLetterOutCommandValidator()
        {
            //RuleFor(r => r.SubconContractId).NotNull();
            //RuleFor(r => r.ContractNo).NotNull();
            RuleFor(r => r.EPOId).NotNull();
            RuleFor(r => r.EPONo).NotNull();
            RuleFor(r => r.UENId).NotEmpty().When(r => r.SubconCategory == "SUBCON CUTTING SEWING");
            RuleFor(r => r.DLDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.UENNo).NotNull().When(r=>r.SubconCategory == "SUBCON CUTTING SEWING");
            RuleFor(r => r.PONo).NotNull().When(r => r.SubconCategory == "SUBCON CUTTING SEWING");
            RuleFor(r => r.EPOItemId).NotNull().When(r => r.SubconCategory == "SUBCON CUTTING SEWING");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item tidak boleh kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconDeliveryLetterOutItemValueObjectValidator()).When(r => r.SubconCategory == "SUBCON CUTTING SEWING");
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconDeliveryLetterOutCuttingItemValueObjectValidator()).When(r => r.SubconCategory == "SUBCON SEWING");
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconDeliveryLetterOutServiceItemValueObjectValidator()).When(r => r.ContractType == "SUBCON JASA" || r.ContractType == "SUBCON BAHAN BAKU");
            RuleFor(r => r.TotalQty)
                 .LessThanOrEqualTo(r => r.UsedQty)
                 .WithMessage(x => $"'Jumlah Total' tidak boleh lebih dari '{x.UsedQty}'.");
        }
    }

    public class GarmentSubconDeliveryLetterOutItemValueObjectValidator : AbstractValidator<GarmentSubconDeliveryLetterOutItemValueObject>
    {
        public GarmentSubconDeliveryLetterOutItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");
        }
    }

    public class GarmentSubconDeliveryLetterOutCuttingItemValueObjectValidator : AbstractValidator<GarmentSubconDeliveryLetterOutItemValueObject>
    {
        public GarmentSubconDeliveryLetterOutCuttingItemValueObjectValidator()
        {

            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");

            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.POSerialNumber).NotNull();
            RuleFor(r => r.SubconId).NotNull();
            RuleFor(r => r.SubconNo).NotNull();
        }
    }

    public class GarmentSubconDeliveryLetterOutServiceItemValueObjectValidator: AbstractValidator<GarmentSubconDeliveryLetterOutItemValueObject>
    {
        public GarmentSubconDeliveryLetterOutServiceItemValueObjectValidator()
        {

            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");

            RuleFor(r => r.SubconId).NotNull();
            RuleFor(r => r.SubconNo).NotNull();
        }
    }
}
