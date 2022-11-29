using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
namespace Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands
{
    public class UpdateGarmentSubconInvoicePackingListCommand : ICommand<SubconInvoicePackingList>
    {
        public Guid Identity { get; private set; }
        public string InvoiceNo { get; set; }
        public string BCType { get; set; }
        public DateTimeOffset Date { get; set; }
        public Supplier Supplier { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string ContractNo { get; set; }
        public double NW { get; set; }
        public double GW { get; set; }
        public string Remark { get; set; }
        public List<SubconInvoicePackingListItemValueObject> Items { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }
    public class UpdateGarmentSubconInvoicePackingListCommandValidator : AbstractValidator<UpdateGarmentSubconInvoicePackingListCommand>
    {
        public UpdateGarmentSubconInvoicePackingListCommandValidator()
        {
            RuleFor(r => r.BCType).NotNull();
            RuleFor(r => r.Date).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Tidak Boleh Kosong");
            RuleFor(r => r.Supplier).NotNull();
            RuleFor(r => r.Supplier.Id).NotEmpty().OverridePropertyName("Supplier").When(w => w.Supplier != null);
            //RuleFor(r => r.ContractNo).NotNull();
            RuleFor(r => r.NW).NotNull();
            RuleFor(r => r.GW).NotNull();


        }
    }
}
