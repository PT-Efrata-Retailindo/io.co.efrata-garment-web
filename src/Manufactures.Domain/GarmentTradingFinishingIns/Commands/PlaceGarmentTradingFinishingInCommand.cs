using ExtCore.Data.Abstractions;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentTradingFinishingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Manufactures.Domain.GarmentTradingFinishingIns.Commands
{
    public class PlaceGarmentTradingFinishingInCommand : ICommand<GarmentFinishingIn>
    {
        public string FinishingInNo { get; set; }
        public string FinishingInType { get; set; }
        public UnitDepartment Unit { get; set; }
        public string Article { get; set; }
        public string RONo { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset? FinishingInDate { get; set; }
        public DateTimeOffset? CuttingOutDate { get; set; }
        public List<GarmentTradingFinishingInItemValueObject> Items { get; set; }
        public long DOId { get; set; }
        public string DONo { get; set; }
        public object Supplier { get; set; }
        public double? TotalQuantity { get; set; }
        public string SubconType { get; set; }
    }

    public class PlaceGarmentTradingFinishingInCommandValidator : AbstractValidator<PlaceGarmentTradingFinishingInCommand>
    {
        public PlaceGarmentTradingFinishingInCommandValidator(IStorage storage)
        {
            IGarmentFinishingInRepository garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();

            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.FinishingInDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal FinishingIn Tidak Boleh Kosong");
            RuleFor(r => r.FinishingInDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal FinishingIn Tidak Boleh Lebih dari Hari Ini");
            //RuleFor(r => r.FinishingInDate).NotNull().GreaterThan(r => r.CuttingOutDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal FinishingIn Tidak Boleh Kurang dari tanggal {r.CuttingOutDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.Comodity).NotNull();
            RuleFor(r => r.Supplier).NotNull();
            RuleFor(r => r.DONo).NotNull().When(w => w.Supplier != null);
            RuleFor(r => r.DOId).NotEmpty().When(w => w.Supplier != null);
            RuleFor(r => r.RONo).NotNull().When(w => w.DONo != null);
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r).Must(m =>
            {
                var existingByDONoRONo = garmentFinishingInRepository.Query.Count(f => f.DOId == m.DOId && f.RONo == m.RONo);
                return existingByDONoRONo == 0;
            })
            .WithMessage(m => "SJ dan No RO sudah ada Finishing In Trading")
            .OverridePropertyName("RONo")
            .When(w => w.DOId > 0 && w.RONo != null);

            RuleFor(r => r.TotalQuantity)
                .NotNull()
                .Equal(0).WithMessage("Sisa Jumlah SJ harus sama dengan 0")
                .When(w => w.RONo != null);

            RuleFor(r => r.Items).NotNull().OverridePropertyName("Item");
            RuleFor(r => r.Items.Where(w => w.IsSave)).NotEmpty().OverridePropertyName("Item").When(w => w.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentTradingFinishingInItemValueObjectValidator());
        }
    }

    class GarmentTradingFinishingInItemValueObjectValidator : AbstractValidator<GarmentTradingFinishingInItemValueObject>
    {
        public GarmentTradingFinishingInItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity).GreaterThan(0).WithMessage("'Jumlah' harus lebih dari '0'.").When(w => w.IsSave);

            RuleFor(r => r.Product).NotNull().When(w => w.IsSave);
            RuleFor(r => r.Size).NotNull().When(w => w.IsSave);

        }
    }
}
