using MurtazaSaleem_Assignment4.Models;
using MurtazaSaleem_Assignment4.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurtazaSaleem_Assignment4.Controllers
{
    public class UserAccountController : Controller
    {
        private IUserRepository _userRepository;

        public UserAccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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