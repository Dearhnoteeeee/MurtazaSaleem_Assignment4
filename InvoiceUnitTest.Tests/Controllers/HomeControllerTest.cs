using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MurtazaSaleem_Assignment4;
using MurtazaSaleem_Assignment4.Controllers;
using MurtazaSaleem_Assignment4.Models;
using MurtazaSaleem_Assignment4.Persistence;

namespace InvoiceUnitTest.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void TestInvoicePayment()
        {
            //arrange
            Invoice inv = new Invoice();

            //act
            inv.UnitPrice = 10;
            inv.ProdQuantity = 5;

            //assert
            Assert.AreEqual(50, inv.SubTotal, "Subtotal calculation is incorrect.");
            Assert.AreEqual(50 * Invoice.TAX_RATE, inv.Tax, $"Tax calculation is incorrect based on tax being {Invoice.TAX_RATE}.");
            Assert.AreEqual(50 + 50 * Invoice.TAX_RATE, inv.Total, "Total calculation is incorrect.");
        }

        [TestMethod]
        public void TestFreeProductInvoicePayment()
        {
            //arrange
            Invoice inv = new Invoice();

            //act
            inv.UnitPrice = 0;
            inv.ProdQuantity = 5;

            //assert
            Assert.AreEqual(0, inv.SubTotal, "Subtotal should be zero for a product with unit price zero");
            Assert.AreEqual(0, inv.Tax, "Tax should be zero for a product with unit price zero");
            Assert.AreEqual(0, inv.Total, "Total should be zero for a product with unit price zero");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestProductZeroQuantity()
        {
            //arrange
            Invoice inv = new Invoice();

            //act
            inv.ProdQuantity = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestProductNegativeQuantity()
        {
            //arrange
            Invoice inv = new Invoice();

            //act
            inv.ProdQuantity = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestProductNegativePrice()
        {
            //arrange
            Invoice inv = new Invoice();

            //act
            inv.UnitPrice = -5;
        }

        [TestMethod]
        public void TestProductShipDate()
        {
            //arrange
            Invoice inv = new Invoice();

            //act
            inv.ShipDate = DateTime.Today;

            //assert
            Assert.AreEqual(DateTime.Today, inv.ShipDate, "Incorrect ship date");
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAccessUninitializedShipDate()
        {
            //arrange
            Invoice inv = new Invoice();

            //act
            DateTime sd = inv.ShipDate;
        }

        [TestMethod]
        public void TestReceivablesTotal()
        {
            //Arrange
            //Create a mock repository of invoices
            Mock<IInvoiceRepository> mocInvoiceRepo = new Mock<IInvoiceRepository>();
            mocInvoiceRepo.Setup(m => m.ReceivableInvoiceList).Returns(
                new List<Invoice>
                {
                    new Invoice { InvoiceNo=1, ProdQuantity=1, UnitPrice=0.0m},
                    new Invoice { InvoiceNo=2, ProdQuantity=1, UnitPrice=10.50m},
                    new Invoice { InvoiceNo=3, ProdQuantity=1, UnitPrice=100.75m},
                    new Invoice { InvoiceNo=4, ProdQuantity=10, IsPaid=true, UnitPrice=100000.95m}
                });
            InvoiceBinder invBinder = new InvoiceBinder(mocInvoiceRepo.Object);

            //act
            //no additional actions recuired beyond setup
            decimal totalRec = invBinder.TotalReceivables;

            //assert
            Assert.AreEqual(111.25 * 1.1, (double)invBinder.TotalReceivables, 0.00001, "The calculation of total receivables is incorrect");
            Assert.IsTrue(invBinder.TotalReceivables < 100000, "The calculation of total receivables includes paid invoices");
        }

        [TestMethod]
        public void TestZeroReceivables()
        {
            //Arrange
            //Create a mock repository of invoices
            Mock<IInvoiceRepository> mocInvoiceRepo = new Mock<IInvoiceRepository>();
            mocInvoiceRepo.Setup(m => m.ReceivableInvoiceList).Returns(
                new List<Invoice>
                {
                    new Invoice { InvoiceNo=1, ProdQuantity=1, IsPaid=true, UnitPrice=0.0m},
                    new Invoice { InvoiceNo=2, ProdQuantity=1, IsPaid=true, UnitPrice=10.50m},
                    new Invoice { InvoiceNo=3, ProdQuantity=1, IsPaid=true, UnitPrice=100.75m}
                });
            InvoiceBinder invBinder = new InvoiceBinder(mocInvoiceRepo.Object);

            //act
            //no additional actions recuired beyond setup. Note that we do not want to set IsPaid to true
            //here in case that action of marking an invoice as paid is incorrect

            //assert
            Assert.AreEqual(0, invBinder.TotalReceivables, "Incorrect total receivables if all invoices in the binder are paid");
        }

        [TestMethod]
        public void TestEmptyInvoiceBinder()
        {
            //Arrange
            //Create a mock repository of invoices
            Mock<IInvoiceRepository> mocInvoiceRepo = new Mock<IInvoiceRepository>();
            mocInvoiceRepo.Setup(m => m.ReceivableInvoiceList).Returns(
                new List<Invoice>
                {
                });
            InvoiceBinder invBinder = new InvoiceBinder(mocInvoiceRepo.Object);

            //act
            //no additional actions recuired beyond setup. 

            //assert
            Assert.AreEqual(0, invBinder.TotalReceivables, "The total receivables for an invoice binder with no invoice is incorrect");
        }

        [TestMethod]
        public void TestReceivablesUpdateAfterAdd()
        {
            //create the backing collection of invoices that will be modifed
            List<Invoice> invoiceList = new List<Invoice>
            {
                new Invoice { InvoiceNo=1, ProdQuantity=1, UnitPrice=0.0m},
                new Invoice { InvoiceNo=2, ProdQuantity=1, UnitPrice=10.50m},
                new Invoice { InvoiceNo=3, ProdQuantity=1, UnitPrice=100.75m},
                new Invoice { InvoiceNo=4, ProdQuantity=1, IsPaid=true, UnitPrice=100000.95m}
            };

            //create the mock repository that uses the backing collection of invoices abo ve
            Mock<IInvoiceRepository> mocInvoiceRepo = new Mock<IInvoiceRepository>();
            mocInvoiceRepo.Setup(m => m.ReceivableInvoiceList).Returns(invoiceList);
            mocInvoiceRepo.Setup(m => m.InvoiceCount).Returns(4);
            mocInvoiceRepo.Setup(m => m.AddInvoice(It.IsAny<Invoice>())).Callback
               (
                   (Invoice newInvoice) => { invoiceList.Add(newInvoice); }
               );
            InvoiceBinder invBinder = new InvoiceBinder(mocInvoiceRepo.Object);
            decimal originalTotalReceivables = invBinder.TotalReceivables;

            //act
            invBinder.AddInvoice(new Invoice { InvoiceNo = 5, ProdQuantity = 1, UnitPrice = 500.75m });
            double newTotalReceivables = (double)invBinder.TotalReceivables;

            //assert
            Assert.AreEqual(612.00 * 1.1, newTotalReceivables, 0.00001, "The calculation of total receivables incorrect after adding an invoice");
        }

        [TestMethod]
        public void TestReceivablesUpdateAfterInvoicePaid()
        {
            //create the backing collection of invoices that will be modifed
            List<Invoice> invoiceList = new List<Invoice>
            {
                new Invoice { InvoiceNo=1, ProdQuantity=1, UnitPrice=0.0m},
                new Invoice { InvoiceNo=2, ProdQuantity=1, UnitPrice=10.50m},
                new Invoice { InvoiceNo=3, ProdQuantity=1, UnitPrice=100.75m},
            };

            //create the mock repository that uses the backing collection of invoices abo ve
            Mock<IInvoiceRepository> mocInvoiceRepo = new Mock<IInvoiceRepository>();
            mocInvoiceRepo.Setup(m => m.ReceivableInvoiceList).Returns(invoiceList);
            mocInvoiceRepo.Setup(m => m.FindInvoiceByNo(It.IsAny<int>())).Returns<int>(id => invoiceList.FirstOrDefault((inv => inv.InvoiceNo == id)));

            //create the binder using the mock defined above
            InvoiceBinder invBinder = new InvoiceBinder(mocInvoiceRepo.Object);
            decimal originalTotalReceivables = invBinder.TotalReceivables;

            //act
            invBinder.GetInvoice(3).IsPaid = true;

            //assert
            Assert.AreEqual(10.5 * 1.1, (double)invBinder.TotalReceivables, 0.00001, "The calculation of total receivables is incorrect after invoices are paid");
        }

        [TestMethod]
        public void TestAllReceivablesPaid()
        {
            Mock<IInvoiceRepository> mocInvoiceRepo = new Mock<IInvoiceRepository>();
            mocInvoiceRepo.Setup(m => m.ReceivableInvoiceList).Returns(
                new List<Invoice>
                {
                    new Invoice { InvoiceNo=1, ProdQuantity=1, UnitPrice=56.78m},
                    new Invoice { InvoiceNo=2, ProdQuantity=1, UnitPrice=10.50m},
                    new Invoice { InvoiceNo=3, ProdQuantity=1, UnitPrice=100.75m},
                });
            InvoiceBinder invBinder = new InvoiceBinder(mocInvoiceRepo.Object);
            decimal originalTotalReceivables = invBinder.TotalReceivables;

            //act
            //pay all receivables
            foreach (Invoice inv in invBinder.ReceivableInvoiceList)
            {
                inv.IsPaid = true;
            }

            //assert
            Assert.AreEqual(0, invBinder.TotalReceivables, "The calculation of total receivables is not zero after all receivables have been paid");
        }
    }
}
