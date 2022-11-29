using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.Commands
{
    public class PlaceGarmentSewingOutCommand : ICommand<GarmentSewingOut>
    {
        public string SewingOutNo { get;  set; }
        public Buyer Buyer { get;  set; }
        public UnitDepartment Unit { get;  set; }
        public string SewingTo { get;  set; }
        public UnitDepartment UnitTo { get;  set; }
        public string RONo { get;  set; }
        public string Article { get;  set; }
        public GarmentComodity Comodity { get;  set; }
        public DateTimeOffset? SewingOutDate { get;  set; }
        public DateTimeOffset? SewingInDate { get; set; }
        public bool IsDifferentSize { get;  set; }
        public bool IsUsed { get; set; }
        public List<GarmentSewingOutItemValueObject> Items { get; set; }
        public bool IsSave { get; set; }
        public double Price { get; set; }

    }

    public class PlaceGarmentSewingOutCommandValidator : AbstractValidator<PlaceGarmentSewingOutCommand>
    {
        public PlaceGarmentSewingOutCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.UnitTo).NotNull().WithMessage("Unit Tujuan Tidak Boleh Kosong");
            RuleFor(r => r.UnitTo.Id).NotEmpty().WithMessage("Unit Tujuan Tidak Boleh Kosong").OverridePropertyName("UnitTo").When(w => w.UnitTo != null);

            RuleFor(r => r.UnitTo.Code).NotEqual(r=>r.Unit.Code).WithMessage("Unit Tujuan dan Unit Tidak Boleh Sama").OverridePropertyName("UnitTo").When(a=> a.SewingTo=="SEWING" && a.Unit!=null && a.UnitTo!=null);
            RuleFor(r => r.Price).GreaterThan(0).WithMessage("Tarif komoditi belum ada");
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.SewingOutDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Sewing Out Tidak Boleh Kosong");
            RuleFor(r => r.SewingOutDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Sewing Out Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.SewingOutDate).NotNull().GreaterThan(r => r.SewingInDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Sewing Out Tidak Boleh Kurang dari tanggal {r.SewingInDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
            RuleFor(r => r.Items.Where(s => s.IsSave == true)).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
            RuleForEach(r => r.Items).SetValidator(new GarmentSewingOutItemValueObjectValidator());
        }
    }

    class GarmentSewingOutItemValueObjectValidator : AbstractValidator<GarmentSewingOutItemValueObject>
    {
        public GarmentSewingOutItemValueObjectValidator()
        {
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.").When(r => r.IsSave == true);
            RuleFor(r => r.Quantity)
               .LessThanOrEqualTo(r => r.SewingInQuantity)
               .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.SewingInQuantity}'.").When(w => w.IsDifferentSize == false && w.IsSave == true);

            RuleFor(r => r.TotalQuantity)
               .LessThanOrEqualTo(r => r.SewingInQuantity)
               .WithMessage(x => $"'Jumlah Total Detail' tidak boleh lebih dari '{x.SewingInQuantity}'.").When(w => w.IsDifferentSize == true && w.IsSave == true);

            RuleFor(r => r.Details).NotEmpty().OverridePropertyName("Detail").When(r=>r.IsDifferentSize==true && r.IsSave == true);
            RuleForEach(r => r.Details).SetValidator(new GarmentSewingOutDetailValueObjectValidator()).When(r => r.IsDifferentSize == true && r.IsSave == true);
            
        }
    }

    class GarmentSewingOutDetailValueObjectValidator : AbstractValidator<GarmentSewingOutDetailValueObject>
    {
        public GarmentSewingOutDetailValueObjectValidator()
        {

            RuleFor(r => r.Size).NotNull();
            RuleFor(r => r.Size.Id).NotEmpty().OverridePropertyName("Size").When(w => w.Size != null);
            RuleFor(r => r.Quantity)
                .GreaterThan(0)
                .WithMessage("'Jumlah' harus lebih dari '0'.");

        }
    }
}
