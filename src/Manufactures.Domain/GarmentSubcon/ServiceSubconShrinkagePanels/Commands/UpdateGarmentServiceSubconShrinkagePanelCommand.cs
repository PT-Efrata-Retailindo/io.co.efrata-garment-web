using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands
{
    public class UpdateGarmentServiceSubconShrinkagePanelCommand : ICommand<GarmentServiceSubconShrinkagePanel>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconShrinkagePanelNo { get; set; }
        public DateTimeOffset? ServiceSubconShrinkagePanelDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public List<GarmentServiceSubconShrinkagePanelItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentServiceSubconShrinkagePanelCommandValidator : AbstractValidator<UpdateGarmentServiceSubconShrinkagePanelCommand>
    {
        public UpdateGarmentServiceSubconShrinkagePanelCommandValidator()
        {
            RuleFor(r => r.ServiceSubconShrinkagePanelDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Subcon Jasa Shrinkage Panel Tidak Boleh Kosong");
            //RuleFor(r => r.ServiceSubconSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconShrinkagePanelItemValueObjectValidator());
        }
    }
}
