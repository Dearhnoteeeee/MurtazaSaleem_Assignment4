using MurtazaSaleem_Assignment4.Models;
using MurtazaSaleem_Assignment4.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurtazaSaleem_Assignment4.Persistence
{
    /// <summary>
    /// Defines the operations required to manage invoices. An application of of the interface seggregation 
    /// principle (ISP). Derives from IRepository to inherit all general methods such as SaveChanges.
    /// </summary>
    public interface IInvoiceRepository : IRepository
    {
        /// <summary>
        /// Returns the list of invoices in the repository
        /// </summary>
        IEnumerable<Invoice> InvoiceList { get; }

        /// <summary>
        /// Returns the number of invoices in the repository
        /// </summary>
        int InvoiceCount { get; }

        /// <summary>
        /// Returns the list of invoices in the repository that have not yet been paid
        /// </summary>
        IEnumerable<Invoice> ReceivableInvoiceList { get; }

        /// <summary>
        /// Finds an invoice object in the repository that has the given ID
        /// </summary>
        /// <param name="invoiceNo">the invoice number of the invoice to obtain</param>
        /// <returns>The invoice object with the given invoice</returns>
        Invoice FindInvoiceByNo(int invoiceNo);

        /// <summary>
        /// Searches the repository for a given invoice
        /// </summary>
        /// <param name="invoiceNo">the invoice number being searched</param>
        /// <returns>
        ///    - true: the repository contains an invoice with the given number
        ///    - false: the repository does not contain an invoice with the given number
        /// </returns>
        bool ContainsInvoice(int invoiceNo);

        /// <summary>
        /// Adds a new invoice to the repository
        /// </summary>
        /// <param name="invoice">the invoice to be added</param>
        void AddInvoice(Invoice invoice);
    }
}
