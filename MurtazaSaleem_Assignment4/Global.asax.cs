using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MurtazaSaleem_Assignment4
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string CACHE_OBJ_INVBINDER = "InvoiceBinderCache";
        public const string SESSION_OBJ_USER = "User";
        public const string TEMP_OBJ_ERRMSG = "Error";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
