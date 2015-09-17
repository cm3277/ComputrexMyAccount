using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebInvoicing.Models;
using CSGOSideLoungeMVCTest4.DAL;
using WebInvoicing.Encryption;
using System.Web.Security;
using WebInvoicing.ActiveDirectoryHelpers;

namespace WebBilling.Controllers
{
    public class UserAccountController : Controller
    {
        private CompXDbContext db = new CompXDbContext();

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string userName, string time, string hash)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(time) || string.IsNullOrWhiteSpace(hash))
            {
                return Json(new { Success = false, error = "Invalid input" });
            }
            userName = userName.ToLower();
            DateTime hashedTime = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(time));
            TimeSpan elapsedTime = new TimeSpan(DateTime.Now.Ticks - hashedTime.Ticks);
            if (elapsedTime.TotalSeconds > 30)
                return Json(new { Success = false, error = "Server took too long to log in. Please try again." });

            if (validateUser(userName, time, hash))
            {

                //Capitalize first letter
                string userNamestring = userName.Substring(0, 1).ToUpper() + userName.Substring(1);
                FormsAuthentication.SetAuthCookie(userNamestring, true);
                string url = Url.Action("Index", "Invoices");
                return Json(new { redirect = url });
            }

            return Json(new { Success = false, error = "The user name or password provided is incorrect." });
        }

        private bool validateUser(string userName, string time, string hash)
        {
            UserAccount acct = db.UserAccounts.Find(userName);
            if (acct == null)
                return false;
            string pass = EncryptionHelper.Decrypt(acct.hashPassword);
            string toHash = "Web_Invoicing" + time + pass;
            string ourHash = EncryptionHelper.sha256_hash(toHash);
            if (ourHash == hash)
                return true;
            else
                return false;
        }

        // GET: /UserAccount/
        [AuthorizeAD(Groups = "Boss,Admins")]
        public async Task<ActionResult> Index()
        {
            return View(await db.UserAccounts.ToListAsync());
        }

        // GET: /UserAccount/Details/5
        [AuthorizeAD(Groups = "Boss,Admins")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount useraccount = await db.UserAccounts.FindAsync(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            //make password viewable if customer
            if (useraccount.permissionGroup == "Customer")
            {
                useraccount.hashPassword = EncryptionHelper.Decrypt(useraccount.hashPassword);
            }
            return View(useraccount);
        }

        // GET: /UserAccount/Create
        [AuthorizeAD(Groups = "Boss,Admins")]
        public ActionResult Create(string type)
        {
            ViewBag.type = type;
            return View();
        }

        // POST: /UserAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeAD(Groups = "Boss,Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="userName,hashPassword,permissionGroup")] UserAccount useraccount)
        {
            if (ModelState.IsValid)
            {
                //check that the user has sufficient priveliges
                if (useraccount.permissionGroup == "Boss" || useraccount.permissionGroup == "Admins")
                {
                    if (!LDAPHelper.UserIsMemberOfGroupOC("Boss", User.Identity.Name))
                        RedirectToAction("Error", "ErrorPages");
                }
                else //auto generate a password for the customers
                {
                    useraccount.hashPassword = System.Web.Security.Membership.GeneratePassword(9, 2);
                }
                //hash the password with the apps secret key
                useraccount.hashPassword = EncryptionHelper.Encrypt(useraccount.hashPassword);
                db.UserAccounts.Add(useraccount);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(useraccount);
        }

        // GET: /UserAccount/Edit/5
        [AuthorizeAD(Groups = "Boss,Admins")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount useraccount = await db.UserAccounts.FindAsync(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            if (useraccount.permissionGroup == "Boss" && !LDAPHelper.UserIsMemberOfGroupOC("Boss", User.Identity.Name))
                return HttpNotFound();
            return View(useraccount);
        }

        // POST: /UserAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeAD(Groups = "Boss,Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="userName,hashPassword,permissionGroup")] UserAccount useraccount)
        {
            if (ModelState.IsValid)
            {
                UserAccount acct = await db.UserAccounts.FindAsync(useraccount.userName);
                if (acct == null)
                {
                    return HttpNotFound();
                }
                if (acct.permissionGroup == "Boss" && !LDAPHelper.UserIsMemberOfGroupOC("Boss", User.Identity.Name))
                    return HttpNotFound();
                //hash the password with the apps secret key
                useraccount.hashPassword = EncryptionHelper.Encrypt(useraccount.hashPassword);
                db.Entry(useraccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(useraccount);
        }

        // GET: /UserAccount/Delete/5
        [AuthorizeAD(Groups = "Boss,Admins")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount useraccount = await db.UserAccounts.FindAsync(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            if (useraccount.permissionGroup == "Boss" && !LDAPHelper.UserIsMemberOfGroupOC("Boss", User.Identity.Name))
                return HttpNotFound();
            return View(useraccount);
        }

        // POST: /UserAccount/Delete/5
        [AuthorizeAD(Groups = "Boss,Admins")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            UserAccount useraccount = await db.UserAccounts.FindAsync(id);
            if (useraccount.permissionGroup == "Boss" && !LDAPHelper.UserIsMemberOfGroupOC("Boss", User.Identity.Name))
                return HttpNotFound();
            db.UserAccounts.Remove(useraccount);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
