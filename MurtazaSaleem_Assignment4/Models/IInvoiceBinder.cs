using System.Collections.Generic;

namespace MurtazaSaleem_Assignment4.Models
{
    public interface IInvoiceBinder
    {
        IEnumerable<Invoice> ReceivableInvoiceList { get; }

        decimal TotalReceivables { get; }

        void AddInvoice(Invoice invoice);

        bool ContainsInvoice(int invoiceId);

        Invoice GetInvoice(int invoiceId);
    }
}