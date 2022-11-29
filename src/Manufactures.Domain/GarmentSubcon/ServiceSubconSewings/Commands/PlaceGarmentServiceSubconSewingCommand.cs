using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands
{
    public class PlaceGarmentServiceSubconSewingCommand : ICommand<GarmentServiceSubconSewing>
    {
        public string ServiceSubconSewingNo { get; set; }
        public DateTimeOffset? ServiceSubconSewingDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentServiceSubconSewingItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
        public Buyer Buyer { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }


    }

    public class PlaceGarmentServiceSubconSewingCommandValidator : AbstractValidator<PlaceGarmentServiceSubconSewingCommand>
    {
        public PlaceGarmentServiceSubconSewingCommandValidator()
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

    class GarmentServiceSubconSewingItemValueObjectValidator : AbstractValidator<GarmentServiceSubconSewingItemValueObject>
    {
        public GarmentServiceSubconSewingItemValueObjectValidator()
        {
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Details");
            RuleFor(r => r.Details).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount");
            RuleFor(r => r.Details.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            RuleForEach(r => r.Details).SetValidator(new GarmentServiceSubconSewingDetailValueObjectValidator());
        }
    }

    class GarmentServiceSubconSewingDetailValueObjectValidator : AbstractValidator<GarmentServiceSubconSewingDetailValueObject>
    {
        public GarmentServiceSubconSewingDetailValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.").When(r => r.IsSave == true);
            RuleFor(r => r.Quantity)
               .LessThanOrEqualTo(r => r.SewingInQuantity)
               .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.SewingInQuantity}'.").When(w => w.IsSave == true);
        }
    }
}
