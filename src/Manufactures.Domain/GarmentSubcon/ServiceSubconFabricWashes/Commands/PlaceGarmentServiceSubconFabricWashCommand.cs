using Infrastructure.Domain.Commands;
using FluentValidation;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands
{
    public class PlaceGarmentServiceSubconFabricWashCommand : ICommand<GarmentServiceSubconFabricWash>
    {
        public string ServiceSubconFabricWashNo { get; set; }
        public DateTimeOffset? ServiceSubconFabricWashDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public List<GarmentServiceSubconFabricWashItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
    }
    public class PlaceGarmentServiceSubconFabricWashCommandValidator : AbstractValidator<PlaceGarmentServiceSubconFabricWashCommand>
    {
        public PlaceGarmentServiceSubconFabricWashCommandValidator()
        {
            RuleFor(r => r.ServiceSubconFabricWashDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Subcon Jasa Fabric Wash Tidak Boleh Kosong");
            // RuleFor(r => r.ServiceSubconSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconFabricWashItemValueObjectValidator());
        }
    }

    class GarmentServiceSubconFabricWashItemValueObjectValidator : AbstractValidator<GarmentServiceSubconFabricWashItemValueObject>
    {
        public GarmentServiceSubconFabricWashItemValueObjectValidator()
        {
            RuleFor(r => r.UnitSender).NotNull();
            RuleFor(r => r.UnitRequest).NotNull();
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Details");
            RuleFor(r => r.Details).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount");
            //RuleFor(r => r.Details.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            RuleForEach(r => r.Details).SetValidator(new GarmentServiceSubconFabricWashDetailValueObjectValidator());
        }
    }

    class GarmentServiceSubconFabricWashDetailValueObjectValidator : AbstractValidator<GarmentServiceSubconFabricWashDetailValueObject>
    {
        public GarmentServiceSubconFabricWashDetailValueObjectValidator()
        {
            /*RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.").When(r => r.IsSave == true);*/
        }
    }
}
