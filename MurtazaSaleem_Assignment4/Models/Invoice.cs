 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurtazaSaleem_Assignment4.Models
{
    /// <summary>
    /// Identifies the type of currency used to pay an invoice
    /// </summary>
    public enum Currency : byte
    {
        CAD = 1,
        USD = 2,
        EUR = 3
    }

    /// <summary>
    /// An invoice being tracked  by the system. This model class merges several concerns which could be
    /// split: client, product, receivable and payment. In a real-world design that abides by the principle
    /// of separation of concerns these models would have to exist separately from the invoice which would just
    /// group them together.
    /// </summary>
    public class Invoice
    {
        private int _id;

        private string _clientName;
        private string _clientAddress;

        private string _prodName;
        private int _prodQuantity;
        private decimal _unitPrice;

        private DateTime? _shipDate;
        private DateTime? _paymentDueDate;
        private Currency? _currency;

        public const decimal TAX_RATE = 0.1m;
        public const int ID_NONE = -1;

        /// <summary>
        /// Instance constructor for new invoices whose ID is -1. This constructor is chained
        /// with the one taking an ID as a parameter in order to reuse the initialization code
        /// </summary>
        public Invoice() : this(ID_NONE)
        {
            //chained constructor, not additional initialization needed
        }

        /// <summary>
        /// Instance constructor that initializes an invoice object with the given ID
        /// </summary>
        /// <param name="id">the ID of the invoice to be created</param>
        public Invoice(int id)
        {
            //initialize the ID of the invoice. IDs are tracked by the invoice binder or by a database if one exists.
            _id = id;

            //initialize client information
            _clientName = String.Empty;
            _clientAddress = String.Empty;

            //product information
            _prodName = String.Empty;
            _prodQuantity = 0;
            _unitPrice = 0;

            //initialize invoice payment details
            _shipDate = null;
            _paymentDueDate = null;
            _currency = null;
            IsPaid = false;
        }

        /// <summary>
        /// Provides read-write property to the ID of the invoice
        /// </summary>
        [Key]
        public int InvoiceNo
        {
            get { return _id; }
            set { _id = value; }
        }

        #region Client Information

        /// <summary>
        /// Provides read-write access to the client name. This property is required and cannot be set to null. 
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the client name is set to a null or empty value.</exception>
        /// <exception cref="NullReferenceException">Thrown if the client name has not been set and it is beeing requested.</exception> 
        public string ClientName
        {
            get
            {
                if (String.IsNullOrEmpty(_clientName))
                {
                    throw new NullReferenceException("The client name has not been set. Cannot return client name.");
                }

                return _clientName;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Please provide a client name for the invoice.");
                }

                _clientName = value;
            }
        }

        /// <summary>
        /// Provides read-write access to the client address. This property is required and cannot be set to null. 
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the client address is set to a null or empty value</exception>
        /// <exception cref="NullReferenceException">Thrown if the client address has not been set and it is beeing requested.</exception> 
        public string ClientAddress
        {
            get
            {
                if (String.IsNullOrEmpty(_clientAddress))
                {
                    throw new NullReferenceException("The client address has not been set. Cannot return client address.");
                }

                return _clientAddress;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Please provide a client address for the invoice.");
                }

                _clientAddress = value;
            }
        }
        #endregion

        #region Product Information

        /// <summary>
        /// Provides read-write access to the product name. This property is required and cannot be set to null. 
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the product name is set to a null or empty value</exception>
        /// <exception cref="NullReferenceException">Thrown if the product name has not been set and it is beeing requested.</exception> 
        public string ProductName
        {
            get
            {
                if (String.IsNullOrEmpty(_prodName))
                {
                    throw new NullReferenceException("The product name has not been set. Cannot return product name.");
                }

                return _prodName;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Please provide a product name for the invoice.");
                }

                _prodName = value;
            }
        }

        /// <summary>
        /// Provides read-write access to the product quantity. This property is required. 
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the product quantity is negative or zero</exception>
        public int ProdQuantity
        {
            get 
            {                
                return _prodQuantity;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Please specify the quantity as a whole number greater than 1");
                }

                _prodQuantity = value;
            }
        }

        /// <summary>
        /// Provides read-write access to the product unit price. This property is required. 
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the unit price is negative</exception>
        public decimal UnitPrice
        {
            get
            {
                return _unitPrice;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Please specify a positive number for the unit price");
                }

                _unitPrice = value;
            }
        }

        #endregion

        #region Receivables Information

        /// <summary>
        /// Provides read-write access to the shipment date. This property is required. 
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the ship date has not been initialized and the 
        /// property is being accessed. The underlying property is a nullable type that can be used to 
        /// check whether the data was set or not.
        /// </exception>
        public DateTime ShipDate
        {
            get
            {
                //use the nullable value to check whether a ship date has been set.
                if (_shipDate == null)
                {
                    throw new NullReferenceException("A ship date has not been set. Cannot return ship date");
                }

                return _shipDate.Value;
            }
            set
            {
                _shipDate = value;
            }
        }

        /// <summary>
        /// Provides read-write access to the payment due date. This property is required. 
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the payment due date has not been initialized and the 
        /// property is being accessed. The underlying property is a nullable type that can be used to 
        /// check whether the data was set or not.
        /// </exception>
        public DateTime PaymentDueDate
        {
            get
            {
                //use the nullable value to check whether a payment due date has been set.
                if (_paymentDueDate == null)
                {                  
                    throw new NullReferenceException("A payment due date has not been set. Cannot return ship date");
                }

                return _paymentDueDate.Value;
            }
            set
            {
                _paymentDueDate = value;
            }
        }

        /// <summary>
        /// Provides access to the currency used to pay for the invoice. 
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the currency has not been initialized and the 
        /// property is being accessed. The underlying property is a nullable type that can be used to 
        /// check whether the data was set or not.
        /// </exception>
        public Currency Currency
        {           
            get
            {
                //use the nullable value to check whether a ship date has been set.
                if (_currency == null)
                {                    
                    throw new NullReferenceException("A currency has not been set. Cannot return currency");
                }

                return _currency.Value;
            }
            set
            {
                _currency = value;
            }
        }

        #endregion

        #region Payment Information

        /// <summary>
        /// Derived property that calculates and provides access to the subtotal. 
        /// </summary>
        public decimal SubTotal
        {
            get
            {
                return _unitPrice * _prodQuantity;
            }
        }

        /// <summary>
        /// Derived property that calculates and provides access to the taxes that need to be paid. 
        /// </summary>
        public decimal Tax { get { return this.SubTotal * TAX_RATE; } }

        /// <summary>
        /// Derived property that calculates and provides access to the total amount to be paid. 
        /// </summary>
        public decimal Total { get { return this.SubTotal + this.Tax; } }

        /// <summary>
        /// Property to provide read/write access to whether the invoice is paid or not.
        /// </summary>
        /// <remarks>
        /// This is implemented as an automatic because at this point in the requirements
        /// not special business logic is associated with it. Should that change it would
        /// be transformed into a regular property that will implement the necessary
        /// business logic (e.g. error checking)
        ///</remarks>
        public bool IsPaid { get; set; }

        #endregion

        /// <summary>
        /// Updates the current invoice with the values of the given invoice object
        /// </summary>
        /// <param name="invoice">the invoice that provides the values to be set in this invoice</param>
        public void Update(Invoice invoice)
        {
            _id = invoice.InvoiceNo;

            _clientName = invoice.ClientName;
            _clientAddress = invoice.ClientAddress;

            _prodName = invoice.ProductName;
            _prodQuantity = invoice.ProdQuantity;

            _shipDate = invoice.ShipDate;
            _paymentDueDate = invoice.PaymentDueDate;
            _unitPrice = invoice.UnitPrice;
            _currency = invoice.Currency;

            this.IsPaid = invoice.IsPaid;
        }
    }
}
