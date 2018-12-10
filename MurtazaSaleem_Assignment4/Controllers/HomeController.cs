using MurtazaSaleem_Assignment4.Models;
using MurtazaSaleem_Assignment4.Persistance;
using MurtazaSaleem_Assignment4.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurtazaSaleem_Assignment4.Controllers
{
    public class HomeController : Controller
    {
        private IInvoiceRepository _invoiceRepository;

        private IUserRepository _userRepository;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Recievables for Invoice app
        /// </summary>
        /// <returns></returns>
        public ActionResult Recievables()
        {
            ViewBag.Message = "Your recievable page.";

            return View();
        }

        [HttpGet]
        public ActionResult AddInvoice()
        {
            ViewBag.InvoiceFormScope = InvoiceFormScope.Add;

            //ensure an empty invoice model object is passed  which has the ID property set to a default. 
            //withouth it the the framework will assume it to be an error that the ID was not set. The ID
            //cannot be set by the user since it is only displayed as a hidden field.
            return View("InvoiceForm", new InvoiceViewModel());
        }

        /// <summary>
        /// HTTP Post response to the request to add an invoice. Triggered when the user submits an actual invoice
        /// to be added to the system. The method will add the invoice to the invoice binder and will allow the
        /// user to add the next invoice
        /// </summary>
        /// <param name="newInvoice"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInvoice(InvoiceViewModel vmInvoice)
        {
            if (ModelState.IsValid)
            {
                //add the invoice to the invoice binder
                Invoice invoice = vmInvoice.Invoice;
                this.InvoiceBinder.AddInvoice(invoice);

                //allow the user to add the next invoice
                return RedirectToAction("AddInvoice");
            }
            else
            {
                //The invoice input data has errors, display and allow the user to resubmit
                ViewBag.InvoiceFormScope = InvoiceFormScope.Add;
                return View("InvoiceForm", vmInvoice);
            }
        }

        /// <summary>
        /// Action method that is available only for manager users and allows them to see the list of
        /// receivables and to manage them 
        /// </summary>
        /// <returns></returns>
        public ActionResult Receivables()
        {
            //check whether the current user is eligible for this operation. If not send the user to the login page
            if (AppUser.Role != UserRole.Manager)
            {
                TempData[MvcApplication.TEMP_OBJ_ERRMSG] = $"Unauthorized access by {AppUser.UserName} . The user is not a manager.";
                return RedirectToAction("Login", "UserAccounts");
            }

            return View("Receivables", this.InvoiceBinder);
        }

        [HttpGet]
        public ActionResult EditInvoice(int invoiceId)
        {
            //check whether the current user is eligible for this operation. If not send the user to the login page
            if (AppUser.Role != UserRole.Manager)
            {
                TempData[MvcApplication.TEMP_OBJ_ERRMSG] = $"Unauthorized access by {AppUser.UserName} . The user is not a manager.";
                return RedirectToAction("Login", "UserAccounts");
            }

            //access the invoice with the given ID from the invoice binder
            Invoice invoice = this.InvoiceBinder.GetInvoice(invoiceId);

            //display the invoice form for the given invoice in the edit form
            ViewBag.InvoiceFormScope = InvoiceFormScope.Edit;
            return View("InvoiceForm", new InvoiceViewModel(invoice));
        }

        [HttpPost]
        public ActionResult EditInvoice(InvoiceViewModel vmInvoice)
        {
            //check whether the current user is eligible for this operation. If not send the user to the login page
            if (AppUser.Role != UserRole.Manager)
            {
                TempData[MvcApplication.TEMP_OBJ_ERRMSG] = $"Unauthorized access by {AppUser.UserName} . The user is not a manager.";
                return RedirectToAction("Login", "UserAccounts");
            }

            //check the data received to make sure it is represents a valid invoice
            if (ModelState.IsValid)
            {
                //ask the repository to update the invoice represented by the invoice view model
                this.InvoiceBinder.UpdateInvoice(vmInvoice.Invoice);

                //display the receivables view
                return View("Receivables", this.InvoiceBinder);
            }
            else
            {
                //the invoice is not valid, allow the user to make further edits
                ViewBag.InvoiceFormScope = InvoiceFormScope.Edit;
                return View("EditInvoice", vmInvoice);
            }
        }

        [HttpPost]
        public ActionResult InvoicePaid(int invoiceId)
        {
            //check whether the current user is eligible for this operation. If not send the user to the login page
            if (AppUser.Role != UserRole.Manager)
            {
                TempData[MvcApplication.TEMP_OBJ_ERRMSG] = $"Unauthorized access by {AppUser.UserName} . The user is not a manager.";
                return RedirectToAction("Login", "UserAccounts");
            }

            //retrieve the invoice from the invoice binder
            this.InvoiceBinder.PayInvoice(invoiceId);

            //display the receivables page which should no longer include the given invoice
            return RedirectToAction("Receivables");
        }

        /// <summary>
        /// Provides access to the invoice binder using the cache state. If the information is not there
        /// the invoice binder will be created and stored in the cache for further access.
        /// </summary>
        private InvoiceBinder InvoiceBinder
        {
            get
            {
                //check if it exists in the cache and create and cache it if not
                InvoiceBinder invBinder = HttpContext.Cache["InvoiceBinderCache"] as InvoiceBinder;
                if (invBinder == null)
                {
                    //this is the first type the binder is being accessed or the cached information has timed out
                    //create an invoice binder and store it in the cache state
                    invBinder = new InvoiceBinder(_invoiceRepository);
                    HttpContext.Cache[MvcApplication.CACHE_OBJ_INVBINDER] = invBinder;
                }

                //return the invoice binder from the cache
                return invBinder;
            }
        }

        /// <summary>
        /// Implementation property to simplify the code to access the user. Named AppUser instead of User because
        /// MVC and the Controller base class already has a User property which is part of the mechanism we are implementing
        /// in this assignment manually. In other words when implementing authentication using the MVC mechanisms this would
        /// not be needed.
        /// </summary>
        private User AppUser
        {
            get { return Session[MvcApplication.SESSION_OBJ_USER] as User; }
        }
        /// <summary>
        /// Default action of this controller. Displays the login form to be filled by
        /// the user and allow the user to login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string loginType)
        {
            //check that the model is valid and display errors if it is not
            if (ModelState.IsValid)
            {
                //validate the user password
                if (AuthenticateUser(user) == false)
                {
                    //the password is incorrect, add an error to the model and return
                    ModelState.AddModelError("Password", "Invalid password.");

                    return View(user);
                }

                //determine the user role
                UserRole roleRequested = DetermineLoginType(loginType);

                //check the role that was requested. Only two manager users exist per requirements: Amanda and Ray
                if (CheckUserRole(user, roleRequested))
                {
                    //grant the role to the user
                    user.Role = roleRequested;
                }
                else
                {
                    //the user is not authorized for this role
                    TempData[MvcApplication.TEMP_OBJ_ERRMSG] = $"Unauthorized login attempt by {user.UserName} . The user is not a manager.";
                    return RedirectToAction("Login");
                }

                //save the user in the session so it can be verified later on when a specific action is requested
                Session[MvcApplication.SESSION_OBJ_USER] = user;

                //check the role and display the appropriate view. For regular users
                //allow them to add an invoice. For manager users display the list
                //of outstanding invoices                
                switch (user.Role)
                {
                    case UserRole.User:
                        //display the view to add invoice
                        return RedirectToAction("AddInvoice", "Invoicing");

                    case UserRole.Manager:
                        //display the view to manage receivables
                        return RedirectToAction("Receivables", "Invoicing");

                    default:
                        //unknown role name, display the default functionality
                        Debug.Assert(false, "Unknown user role, assume regular user and display the default functionality");
                        return RedirectToAction("AddInvoice", "Invoicing");
                }
            }
            else
            {
                //The user model is invalid, go back to the login to ask the user
                //to enter the correct information
                return View(user);
            }
        }

        private bool AuthenticateUser(User user)
        {
            //as a shortcut for user registration the first login of a new user is considered to be 
            //a registration as long as the password matches the business logic requirements of being
            //lower-case reversed value of the user name
            User userIdentity = _userRepository.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (userIdentity == null)
            {
                //there is no user with the given user name, register this as a new user if the password is valid
                if (user.CheckPassword())
                {
                    _userRepository.AddUser(user);
                    _userRepository.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return user.Password == userIdentity.Password;
            }
        }

        /// <summary>
        /// Internal controller method (not an action method) that is used to determine the role of 
        /// the user based on the login type provided in the request which is set through the submit
        /// button parameter name
        /// </summary>
        /// <param name="loginType"></param>
        /// <returns></returns>
        private UserRole DetermineLoginType(string loginType)
        {
            switch (loginType)
            {
                case "User Login":
                case null:
                default:
                    return UserRole.User;

                case "Manager Login":
                    return UserRole.Manager;
            }
        }

        /// <summary>
        /// Checks whether the given user belongs to the role that has been requested
        /// </summary>
        /// <param name="user">the user that is logging in</param>
        /// <param name="roleRequested">the role the user requested</param>
        /// <returns>
        ///     - true if the user is authorized to work in the given role
        ///     - false if the user is not authorized to work in the given role
        /// </returns>
        private bool CheckUserRole(User user, UserRole roleRequested)
        {
            User userIdentity = _userRepository.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (userIdentity != null)
            {
                switch (roleRequested)
                {
                    case UserRole.User:
                        return true;

                    case UserRole.Manager:
                        return userIdentity.Role == UserRole.Manager;

                    default:
                        Debug.Assert(false, "Unknown user role. Cannot check the role");
                        return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}