using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.Commands
{
    public class PlaceGarmentCuttingOutCommand : ICommand<GarmentCuttingOut>
    {
        public string CutOutNo { get; set; }
        public string CuttingOutType { get; set; }

        public UnitDepartment UnitFrom { get; set; }
        public DateTimeOffset? CuttingOutDate { get; set; }
        public DateTimeOffset? CuttingInDate { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public GarmentComodity Comodity { get; set; }
        public List<GarmentCuttingOutItemValueObject> Items { get; set; }
        public double Price { get; set; }
        public double PriceSewing { get; set; }
        public bool IsUsed { get; set; }
    }

    public class PlaceGarmentCuttingOutCommandValidator : AbstractValidator<PlaceGarmentCuttingOutCommand>
    {
        public PlaceGarmentCuttingOutCommandValidator()
        {
            RuleFor(r => r.UnitFrom).NotNull();
            RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("UnitFrom").When(w => w.UnitFrom != null);

            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);

            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");

            RuleFor(r => r.PriceSewing).GreaterThan(0).WithMessage(x=>$"`Tarif komoditi Sewing '{x.Unit.Name}' belum ada`").When(w => w.Unit != null);
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.CuttingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.CuttingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Cutting Out Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.CuttingOutDate).NotNull().GreaterThan(r => r.CuttingInDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Cutting Out Tidak Boleh Kurang dari tanggal {r.CuttingInDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}").When(r=>r.CuttingInDate!=null);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s=>s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentCuttingOutItemValueObjectValidator());
        }
    }

    class GarmentCuttingOutItemValueObjectValidator : AbstractValidator<GarmentCuttingOutItemValueObject>
    {
        public GarmentCuttingOutItemValueObjectValidator()
        {
            RuleFor(r => r.Details).NotEmpty().WithMessage("Detail Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(w => w.IsSave == true);

            RuleFor(r => r.TotalCuttingOutQuantity)
               .LessThanOrEqualTo(r => r.TotalCuttingOut)
               .WithMessage(x => $"'Jumlah Potong' tidak boleh lebih dari '{x.TotalCuttingOut}'.").When(w => w.IsSave == true);

            RuleForEach(r => r.Details).SetValidator(new GarmentCuttingOutDetailValueObjectValidator()).When(w => w.IsSave == true);
        }
    }

    class GarmentCuttingOutDetailValueObjectValidator : AbstractValidator<GarmentCuttingOutDetailValueObject>
    {
        public GarmentCuttingOutDetailValueObjectValidator()
        {
            // not supported at this time: FluentValidation 8.0.100 but some libs are incompatible
            //When(w => w.IsSave, () =>
            //{
            //    //RuleFor(r => r.PreparingUom).NotNull();
            //    //RuleFor(r => r.PreparingUom.Unit).NotEmpty().OverridePropertyName("PreparingUom").When(w => w.PreparingUom != null);
            //    ////RuleFor(r => r.PreparingUom).Must(m => !string.IsNullOrWhiteSpace(m.Unit)).When(w => w.PreparingUom != null);

            //    RuleFor(r => r.CuttingInQuantity).GreaterThan(0);

            //    RuleFor(r => r.PreparingQuantity).LessThanOrEqualTo(r => r.PreparingRemainingQuantity);
            //});

            RuleFor(r => r.Size).NotNull();
            RuleFor(r => r.Size.Id).NotEmpty().OverridePropertyName("Size").When(w => w.Size != null);
            RuleFor(r => r.Color).NotNull().WithMessage("'Warna' harus diisi.");
            RuleFor(r => r.Color).NotEmpty().WithMessage("'Warna' harus diisi.");
            RuleFor(r => r.CuttingOutQuantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah Potong' harus lebih dari '0'.");

           
        }
    }
}