using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebInvoicing.Models;

namespace WebInvoicing.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {

        public ManageController()
        {
        }
    }
}