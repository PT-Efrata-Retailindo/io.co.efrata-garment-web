using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands
{
    public class UpdateGarmentServiceSubconFabricWashCommand : ICommand<GarmentServiceSubconFabricWash>
    {
        public Guid Identity { get; private set; }
        public string ServiceSubconFabricWashNo { get; set; }
        public DateTimeOffset? ServiceSubconFabricWashDate { get; set; }
        public string Remark { get; set; }
        public bool IsUsed { get; set; }
        public int QtyPacking { get; set; }
        public string UomUnit { get; set; }
        public List<GarmentServiceSubconFabricWashItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentServiceSubconFabricWashCommandValidator : AbstractValidator<UpdateGarmentServiceSubconFabricWashCommand>
    {
        public UpdateGarmentServiceSubconFabricWashCommandValidator()
        {
            RuleFor(r => r.ServiceSubconFabricWashDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Subcon Jasa Fabric Wash Tidak Boleh Kosong");
            //RuleFor(r => r.ServiceSubconSewingDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Subcon Jasa Sewing Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleForEach(r => r.Items).SetValidator(new GarmentServiceSubconFabricWashItemValueObjectValidator());
        }
    }
}
