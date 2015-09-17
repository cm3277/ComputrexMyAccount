using System.Web;
using System.Web.Mvc;

namespace WebInvoicing
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //requires authorization for entire site
            filters.Add(new AuthorizeAttribute());
        }
    }
}
