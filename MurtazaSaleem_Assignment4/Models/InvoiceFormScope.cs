using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MurtazaSaleem_Assignment4.Models
{
    /// <summary>
    /// Represents the model the invoice form operates in. As we are reusing the same view
    /// for both add and edit operations the view needs to differentiate between the different
    /// scopes in which it is usd
    /// </summary>
    public enum InvoiceFormScope
    {
        Add,
        Edit
    }
}