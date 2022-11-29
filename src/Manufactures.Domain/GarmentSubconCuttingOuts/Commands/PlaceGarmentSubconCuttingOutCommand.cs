using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;
using Manufactures.Domain.GarmentSubconCuttingOuts.ValueObjects;
using FluentValidation;
using System.Linq;
using System.Globalization;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.Commands
{
    public class PlaceGarmentSubconCuttingOutCommand : ICommand<GarmentSubconCuttingOut>
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

        public long EPOId { get; set; }
        public long EPOItemId { get; set; }
        public string POSerialNumber { get; set; }
        public double PlanPORemainingQuantity { get; set; }
        public double TotalQty { get; set; }
        public double Price { get; set; }
        public bool IsUsed { get; set; }
        public List<GarmentSubconCuttingOutItemValueObject> Items { get; set; }
    }

    public class PlaceGarmentCuttingOutCommandValidator : AbstractValidator<PlaceGarmentSubconCuttingOutCommand>
    {
        public PlaceGarmentCuttingOutCommandValidator()
        {
            RuleFor(r => r.UnitFrom).NotNull();
            RuleFor(r => r.UnitFrom.Id).NotEmpty().OverridePropertyName("UnitFrom").When(w => w.Unit != null);
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.CuttingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.CuttingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.CuttingOutDate).NotNull().GreaterThan(r => r.CuttingInDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Tidak Boleh Kurang dari tanggal {r.CuttingInDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
			//29april 2022 , permintaan user kustanti, karena buyer JJ dihilangkan validasi 
            //RuleFor(r => r.TotalQty)
            //   .LessThanOrEqualTo(r => r.PlanPORemainingQuantity)
            //   .WithMessage(x => $"'Total Jumlah Potong' tidak boleh lebih dari '{x.PlanPORemainingQuantity}'.");

            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconCuttingOutItemValueObjectValidator());
        }
    }

    class GarmentSubconCuttingOutItemValueObjectValidator : AbstractValidator<GarmentSubconCuttingOutItemValueObject>
    {
        public GarmentSubconCuttingOutItemValueObjectValidator()
        {
            RuleFor(r => r.Details).NotEmpty().WithMessage("Detail Tidak Boleh Kosong").OverridePropertyName("DetailsCount").When(w => w.IsSave == true);

            RuleFor(r => r.TotalCuttingOutQuantity)
               .LessThanOrEqualTo(r => r.TotalCuttingOut)
               .WithMessage(x => $"'Jumlah Potong' tidak boleh lebih dari '{x.TotalCuttingOut}'.").When(w => w.IsSave == true);

            RuleForEach(r => r.Details).SetValidator(new GarmentSubconCuttingOutDetailValueObjectValidator()).When(w => w.IsSave == true);
        }
    }

    class GarmentSubconCuttingOutDetailValueObjectValidator : AbstractValidator<GarmentSubconCuttingOutDetailValueObject>
    {
        public GarmentSubconCuttingOutDetailValueObjectValidator()
        {

            RuleFor(r => r.Size).NotNull();
            RuleFor(r => r.Size.Id).NotEmpty().OverridePropertyName("Size").When(w => w.Size != null);

            RuleFor(r => r.Remark).NotEmpty().OverridePropertyName("Remark");

            RuleFor(r => r.CuttingOutQuantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah Potong' harus lebih dari '0'.");


        }
    }
}
