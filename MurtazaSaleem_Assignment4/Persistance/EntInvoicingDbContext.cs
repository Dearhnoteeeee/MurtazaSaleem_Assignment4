using MurtazaSaleem_Assignment4.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurtazaSaleem_Assignment4.Persistance
{
    class EntInvoicingDbContext : DbContext
    {
        /// <summary>
        /// Instance constructor that initializes the connection string to be used to access the invoicing database
        /// </summary>
        public EntInvoicingDbContext() : base("EntInvoicingDb")
        {
        }

        /// <summary>
        /// Provide access to the Users table in the database
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Provide access to the Invoice table in the database
        /// </summary>
        public DbSet<Invoice> Invoices { get; set; }
    }
}
