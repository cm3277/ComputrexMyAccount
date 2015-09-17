using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebInvoicing.Controllers
{
    public class ErrorPagesController : Controller
    {
        //
        // GET: /ErrorPages/
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
	}
}