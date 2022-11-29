using FluentValidation;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using Infrastructure.Domain.Commands;
using System.Globalization;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands
{
    public class UpdateGarmentSampleAvalProductCommand : ICommand<GarmentSampleAvalProduct>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public DateTimeOffset? AvalDate { get; set; }
        public DateTimeOffset? PreparingDate { get; set; }
        public UnitDepartment Unit { get; set; }
        public List<GarmentSampleAvalProductItemValueObject> Items { get; set; }
    }

    public class UpdateGarmentSampleAvalProductCommandValidator : AbstractValidator<UpdateGarmentSampleAvalProductCommand>
    {
        public UpdateGarmentSampleAvalProductCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.RONo).NotEmpty().WithMessage("Nomor RO Tidak Boleh Kosong");
            RuleFor(r => r.AvalDate).NotNull().WithMessage("Tanggal Aval Tidak Boleh Kosong");
            RuleFor(r => r.AvalDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Aval Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.AvalDate).NotNull().GreaterThan(r => r.PreparingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Cutting Out Tidak Boleh Kurang dari tanggal {r.PreparingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong");
        }
    }
}
