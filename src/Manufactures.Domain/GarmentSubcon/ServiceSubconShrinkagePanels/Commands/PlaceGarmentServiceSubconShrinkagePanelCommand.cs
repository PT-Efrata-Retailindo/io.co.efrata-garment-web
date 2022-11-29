using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands
{
    public class PlaceGarmentServiceSubconShrinkagePanelCommand : ICommand<GarmentServiceSubconShrinkagePanel>
    {
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset? ServiceSubconShrinkagePanelDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public List<GarmentServiceSubconShrinkagePanelItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
    }
    public class PlaceGarmentServiceSubconShrinkagePanelCommandValidator : AbstractValidator<PlaceGarmentServiceSubconShrinkagePanelCommand>
    {
        public PlaceGarmentServiceSubconShrinkagePanelCommandValidator()
        {
            RuleFor(r => r.ServiceSubconShrinkagePanelDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Kosong");
            // RuleFor(r => r.ServiceSubconSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconShrinkagePanelItemValueObjectValidator());
        }
    }

    class GarmentServiceSubconShrinkagePanelItemValueObjectValidator : AbstractValidator<GarmentServiceSubconShrinkagePanelItemValueObject>
    {
        public GarmentServiceSubconShrinkagePanelItemValueObjectValidator()
        {
            RuleFor(r => r.UnitSender).NotNull();
            RuleFor(r => r.UnitRequest).NotNull();
            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Details");
            RuleFor(r => r.Details).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount");
            RuleFor(r => r.Details.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Details Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(s => s.Details != null);
            RuleForEach(r => r.Details).SetValidator(new GarmentServiceSubconShrinkagePanelDetailValueObjectValidator());
        }
    }

    class GarmentServiceSubconShrinkagePanelDetailValueObjectValidator : AbstractValidator<GarmentServiceSubconShrinkagePanelDetailValueObject>
    {
        public GarmentServiceSubconShrinkagePanelDetailValueObjectValidator()
        {
            /*RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.").When(r => r.IsSave == true);*/
        }
    }
}
