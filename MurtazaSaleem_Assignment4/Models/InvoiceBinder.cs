using MurtazaSaleem_Assignment4.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurtazaSaleem_Assignment4.Models
{
    /// <summary>
    /// Collection of invoices that are being tracked by the system. The binder allocates IDs to invoices 
    /// tracked by the system and is responsible for providing access to them and for calculating receivable
    /// amounts.
    /// </summary>
    public class InvoiceBinder : IInvoiceBinder
    {
        /// <summary>
        /// Stores the invoice information making it accesible through a dictionary by the ID of the 
        /// invoice. When the application has a persitsence layer, the repository will be responsible
        /// for maintaining the invoice data which will come from a database.
        /// </summary>
        private IInvoiceRepository _invoiceRepository;

        /// <summary>
        /// Instance constructor that initializes each binder object created. The invoice repository is
        /// to be initialized through dependency injection when the project is created
        /// </summary>
        public InvoiceBinder(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        /// <summary>
        /// Property that hides the implementation of total receivables amount and provides
        /// read-only access to it
        /// </summary>
        public decimal TotalReceivables { get { return ReceivableInvoiceList.Sum(inv => inv.Total); } }

        /// <summary>
        /// Provides access to an enumerator of invoices to allow clients to iterate through
        /// the invoices the binder is tracking
        /// </summary>
        public IEnumerable<Invoice> ReceivableInvoiceList
        {
            get
            {
                return _invoiceRepository.ReceivableInvoiceList.Where(invoice => (invoice.IsPaid == false));
            }
        }

        /// <summary>
        /// Returns the invoice with the given ID from the binder.
        /// </summary>
        /// <param name="invoiceNo">the ID of the invoice to be returned</param>
        /// <returns>the invoice object</returns>
        /// <exception cref="ArgumentException">Thrown if an invoice with the given ID is not found</exception>
        public Invoice GetInvoice(int invoiceNo)
        {            
            Invoice foundInvoice = _invoiceRepository.FindInvoiceByNo(invoiceNo);
            if (foundInvoice == null)
            {
                throw new ArgumentException("There is no invoice with the given invoice number in the binder");
            }
            return foundInvoice;
        }

        /// <summary>
        /// Checks if an invoice with the given id exists in the binder
        /// </summary>
        /// <param name="invoiceNo">the ID of the invoice that is being checked</param>
        /// <returns></returns>
        public bool ContainsInvoice(int invoiceNo)
        {
            return _invoiceRepository.ContainsInvoice(invoiceNo);
        }

        /// <summary>
        /// Adds the given invoice to the invoice repository
        /// </summary>
        /// <param name="invoice">the invoice to be added</param>
        public void AddInvoice(Invoice invoice)
        {
            //add the invoice to the store
            _invoiceRepository.AddInvoice(invoice);
        }

        /// <summary>
        /// Locates the invoice object with the same ID as the invoice provided and updates all its
        /// fields to corresponds to the given invoice. The changes will be saved in the repository.
        /// </summary>
        /// <param name="invoice"></param>
        public void UpdateInvoice(Invoice invoice)
        {
            //obtain the invoice from the repository
            Invoice invoiceToUpdate = _invoiceRepository.FindInvoiceByNo(invoice.InvoiceNo);

            //update the invoice
            invoiceToUpdate.Update(invoice);

            //ensure changes are saved
            _invoiceRepository.SaveChanges();
        }

        /// <summary>
        /// Marks the invoice with the given invoice number as paid which removes it from the list
        /// of receivables. The informatoin is saved in the invoice repository
        /// </summary>
        /// <param name="invoiceNo">the invoice number of the invoice to be paid</param>
        public void PayInvoice(int invoiceNo)
        {
            //obtain the invoice from the repository
            Invoice invoiceToPay = _invoiceRepository.FindInvoiceByNo(invoiceNo);

            //mark the invoice as paid
            invoiceToPay.IsPaid = true;

            //ensure changes are saved
            _invoiceRepository.SaveChanges();
        }

    }
}
