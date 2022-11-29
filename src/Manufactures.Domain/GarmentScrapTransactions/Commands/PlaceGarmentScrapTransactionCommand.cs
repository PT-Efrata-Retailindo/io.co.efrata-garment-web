using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapSources.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.Commands
{
	public class PlaceGarmentScrapTransactionCommand : ICommand<GarmentScrapTransaction>
	{
		public string TransactionNo { get;  set; }
		public DateTimeOffset TransactionDate { get;  set; }
		public string TransactionType { get;  set; }
		public Guid ScrapSourceId { get;  set; }
		public string ScrapSourceName { get;  set; }
		public Guid ScrapDestinationId { get;  set; }
		public string ScrapDestinationName { get;  set; }
	
		public List<GarmentScrapTransactionItemValueObject> Items { get; set; }

	}

	public class PlaceGarmentScrapTransactionCommandValidator : AbstractValidator<PlaceGarmentScrapTransactionCommand>
	{
		public PlaceGarmentScrapTransactionCommandValidator()
		{
			
			RuleFor(r => r.TransactionDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal harus diisi");
            RuleFor(r => r.TransactionDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Lebih dari Hari Ini");

            RuleFor(r => r.ScrapSourceName).NotEmpty().WithMessage("Asal Barang harus diisi").When(s=>s.TransactionType == "IN");
			RuleFor(r => r.ScrapDestinationName).NotEmpty().WithMessage("Tujuan Barang harus diisi").When(s=>s.TransactionType =="IN");
			RuleFor(r => r.ScrapDestinationName).NotEmpty().WithMessage("Asal Barang harus diisi").When(s => s.TransactionType == "OUT");
			RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
			RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount");
			RuleFor(r => r.Items).NotEmpty().WithMessage("Item Tidak Boleh Kosong").OverridePropertyName("ItemsCount").When(s => s.Items != null);
			RuleForEach(r => r.Items).SetValidator(new GarmentScrapTransactionItemValueObjectValidator());
		}
	}

	class GarmentScrapTransactionItemValueObjectValidator : AbstractValidator<GarmentScrapTransactionItemValueObject>
	{
		public GarmentScrapTransactionItemValueObjectValidator()
		{
			RuleFor(r => r.Quantity)
			  .LessThanOrEqualTo(r => r.RemainingQuantity)
			  .OverridePropertyName("Quantity")
			  .WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.RemainingQuantity}'.").When(s=>s.TransactionType == "OUT");

		}
	}
}
