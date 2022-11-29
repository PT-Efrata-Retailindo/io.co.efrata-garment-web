using ExtCore.Data.Abstractions;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Manufactures.Domain.GarmentScrapSources.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.Commands
{
	public class UpdateGarmentScrapTransactionCommand : ICommand<GarmentScrapTransaction>
	{
		public Guid Identity { get; set; }
		public string TransactionNo { get; set; }
		public DateTimeOffset TransactionDate { get; set; }
		public string TransactionType { get; set; }
		public Guid ScrapSourceId { get; set; }
		public string ScrapSourceName { get; set; }
		public Guid ScrapDestinationId { get; set; }
		public string ScrapDestinationName { get; set; }
		public List<GarmentScrapTransactionItemValueObject> Items { get; set; }
		public void SetIdentity(Guid id)
		{
			Identity = id;
		}
		public class UpdateGarmentScrapTransactionCommandValidator : AbstractValidator<UpdateGarmentScrapTransactionCommand>
		{
			public UpdateGarmentScrapTransactionCommandValidator(IStorage storage)
			{
				IGarmentScrapTransactionRepository _GarmentScrapTransactionRepository = storage.GetRepository<IGarmentScrapTransactionRepository>();
				RuleFor(r => r.TransactionDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Transaksi harus diisi");
                RuleFor(r => r.TransactionDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Tidak Lebih dari Hari Ini");
                RuleForEach(r => r.Items).SetValidator(new GarmentScrapTransactionItemValueObjectValidator());
			}
		}

		class GarmentScrapTransactionItemValueObjectValidator : AbstractValidator<GarmentScrapTransactionItemValueObject>
		{
			public GarmentScrapTransactionItemValueObjectValidator()
			{
				RuleFor(r => r.Quantity)
					.GreaterThan(0)
					.WithMessage("'Jumlah' harus lebih dari '0'.").When(s => s.IsEdit == false || s.TransactionType != "OUT");

				RuleFor(r => r.Quantity)
					.LessThanOrEqualTo(r => r.RemainingQuantity)
					.OverridePropertyName("Quantity")
					.WithMessage(x => $"'Jumlah' tidak boleh lebih dari '{x.RemainingQuantity}'.").When(s=>s.TransactionType =="OUT");
			}
		}
	}
}
