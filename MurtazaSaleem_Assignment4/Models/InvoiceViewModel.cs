
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurtazaSaleem_Assignment4.Models
{
    /// <summary>
    /// Represents the view model for the invoice with attributes and constraints that are being
    /// verified by the view and the MVC engine. By separating this view model from the Invoice
    /// domain model we can separate the presentation concerns like error messages displayed in
    /// the view and whether properties are initialized or not from those of the domain where
    /// all fields must have correct values
    /// </summary>
    public class InvoiceViewModel
    {
        /// <summary>
        /// Default constructor required by the view and controller to create an empty model object
        /// </summary>
        public InvoiceViewModel()
        {
            this.Id = Invoice.ID_NONE;
        }

        /// <summary>
        /// Constructor that initializes a invoice view model from an actual view domain object.
        /// </summary>
        /// <param name="invoice"></param>
        public InvoiceViewModel(Invoice invoice)
        {
            this.Id = invoice.InvoiceNo;
            this.ClientName = invoice.ClientName;
            this.ClientAddress = invoice.ClientAddress;
            this.ProductName = invoice.ProductName;
            this.ProdQuantity = invoice.ProdQuantity;
            this.UnitPrice = invoice.UnitPrice;
            this.ShipDate = invoice.ShipDate.ToString("yyyy-MM-dd");
            this.PaymentDueDate = invoice.PaymentDueDate.ToString("yyyy-MM-dd");
            this.Currency = invoice.Currency;
            this.SubTotal = invoice.SubTotal;
            this.Tax = invoice.Tax;
            this.Total = invoice.Total;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a client name for the invoice.")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Please provide a client address for the invoice.")]
        public string ClientAddress { get; set; }

        [Required(ErrorMessage = "Please provide a product name for the invoice.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please specify how many units of product are being shipped")]
        [Range(1, int.MaxValue, ErrorMessage = "Please specify the quantity as a whole number greater than 1")]
        public int? ProdQuantity { get; set; }

        [Required(ErrorMessage = "Please specify the unit price for the product ordered")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please specify a positive number for the unit price")]
        public decimal? UnitPrice { get; set; }

        [Required(ErrorMessage = "Please specify a valid shipment date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string ShipDate { get; set; }

        [Required(ErrorMessage = "Please specify a valid payment date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public string PaymentDueDate { get; set; }

        [Required(ErrorMessage = "Please select a currency")]
        [DataType(DataType.Currency)]
        public Currency? Currency { get; set; }

        public decimal SubTotal { get; }

        public decimal Tax { get; }

        public decimal Total { get; }

        public Invoice Invoice
        {
            get
            {
                //create a domain invoice using the view model information
                Invoice invoice = new Invoice(this.Id);
                UpdateInvoice(invoice);
                return invoice;
            }
        }

        public void UpdateInvoice(Invoice invToUpdate)
        {
            //create a domain invoice using the view model information
            invToUpdate.ClientName = this.ClientName;
            invToUpdate.ClientAddress = this.ClientAddress;
            invToUpdate.ProductName = this.ProductName;
            invToUpdate.ProdQuantity = this.ProdQuantity.Value;
            invToUpdate.UnitPrice = this.UnitPrice.Value;
            invToUpdate.ShipDate = DateTime.Parse(this.ShipDate);
            invToUpdate.PaymentDueDate = DateTime.Parse(this.PaymentDueDate);
            invToUpdate.Currency = this.Currency.Value;
        }
    }
}