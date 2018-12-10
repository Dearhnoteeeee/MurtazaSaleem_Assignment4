using MurtazaSaleem_Assignment4.Persistence;
using MurtazaSaleem_Assignment4.Models;
using System.Collections.Generic;
using System.Linq;

namespace MurtazaSaleem_Assignment4.Persistance
{
    /// <summary>
    /// Database repository that stores invoices in a SQL database using the Entity Framework. Implements
    /// several interfaces as an example of the interface seggregation principles which requires the separation
    /// of concerns by client functionality (in this case Users and Invoices)
    /// </summary>
    class InvoicingDbRepository : IInvoiceRepository, IUserRepository
    {
        /// <summary>
        /// The entity framework context used to communicate with the database
        /// </summary>
        private EntInvoicingDbContext _dbContext;

        /// <summary>
        /// Instance constructor that creates the database context
        /// </summary>
        public InvoicingDbRepository()
        {
            _dbContext = new EntInvoicingDbContext();
        }

        /// <summary>
        /// Provides access to an enumerator of users stored in the repository
        /// </summary>
        public IEnumerable<User> Users
        {
            get
            {
                return _dbContext.Users;
            }
        }

        /// <summary>
        /// Provides access to an enumerator of invoices stored in the repository
        /// </summary>
        public IEnumerable<Invoice> InvoiceList
        {
            get
            {
                return _dbContext.Invoices;
            }
        }

        /// <summary>
        /// Returns the list of invoices in the repository that have not yet been paid
        /// </summary>
        public IEnumerable<Invoice> ReceivableInvoiceList
        {
            get
            {
                return _dbContext.Invoices.Where(inv => (inv.IsPaid == false));
            }
        }

        /// <summary>
        /// Returns the number of invoices in the repository
        /// </summary>
        public int InvoiceCount
        {
            get
            {
                return _dbContext.Invoices.Count();
            }
        }

        /// <summary>
        /// Adds a new invoice to the repository
        /// </summary>
        /// <param name="invoice">the invoice to be added</param>
        public void AddInvoice(Invoice invoice)
        {
            _dbContext.Invoices.Add(invoice);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Searches the repository for a given invoice
        /// </summary>
        /// <param name="invoiceNo">the invoice number being searched</param>
        /// <returns>
        ///    - true: the repository contains an invoice with the given number
        ///    - false: the repository does not contain an invoice with the given number
        /// </returns>
        public bool ContainsInvoice(int invoiceId)
        {
            return _dbContext.Invoices.Any(inv => inv.InvoiceNo == invoiceId);
        }

        /// <summary>
        /// Finds an invoice object in the repository that has the given ID
        /// </summary>
        /// <param name="invoiceNo">the invoice number of the invoice to obtain</param>
        /// <returns>The invoice object with the given invoice</returns>
        public Invoice FindInvoiceByNo(int invoiceId)
        {
            return _dbContext.Invoices.FirstOrDefault(inv => inv.InvoiceNo == invoiceId);
        }

        /// <summary>
        /// Adds a new user in the Users table of the database to be used for authentication
        /// </summary>
        /// <param name="user">the user to register</param>
        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
        }

        /// <summary>
        /// Saves the changes performed on the repository. Must be called when repository items
        /// have been added, updated or deleted
        /// </summary>
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

    }
}
