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
using System.Text;
using System.Security.Cryptography;
using WebInvoicing.Encryption;
using System.Web.Security;
using WebInvoicing.ActiveDirectoryHelpers;

namespace WebInvoicing.Controllers
{
    public class TechAccountController : Controller
    {
        /*private CompXDbContext db = new CompXDbContext();

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
                string url = Url.Action("Index", "Jobs");
                return Json(new { redirect = url });
            }

            return Json(new { Success = false, error = "The user name or password provided is incorrect." });
        }

        private bool validateUser(string userName, string time, string hash)
        {
            TechAccount acct = db.TechAccounts.Find(userName);
            if (acct == null)
                return false;
            string pass = EncryptionHelper.Decrypt(acct.hashPassword);
            string toHash = "Web_Billing" + time + pass;
            string ourHash = EncryptionHelper.sha256_hash(toHash);
            if (ourHash == hash)
                return true;
            else
                return false;
        }




        // GET: /TechAccount/
        //[AuthorizeAD(Groups = "Boss")]
        [AuthorizeAD(Groups = "Boss")]
        public async Task<ActionResult> Index()
        {
            return View(await db.TechAccounts.ToListAsync());
        }

        // GET: /TechAccount/Details/5
        [AuthorizeAD(Groups = "Boss")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechAccount techaccount = await db.TechAccounts.FindAsync(id);
            if (techaccount == null)
            {
                return HttpNotFound();
            }
            return View(techaccount);
        }

        // GET: /TechAccount/Create
        //[AuthorizeAD(Groups = "Boss")]
        [AuthorizeAD(Groups = "Boss")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /TechAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Boss")]
        public async Task<ActionResult> Create([Bind(Include="userName,firstName,lastName,initials,hashPassword,permissionGroup")] TechAccount techaccount)
        {
            if (ModelState.IsValid)
            {
                //lower case username
                techaccount.userName = techaccount.userName.ToLower();
                //hash the password with the apps secret key
                techaccount.hashPassword = EncryptionHelper.Encrypt(techaccount.hashPassword);
                db.TechAccounts.Add(techaccount);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(techaccount);
        }

        // GET: /TechAccount/Edit/5
        [AuthorizeAD(Groups = "Boss")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechAccount techaccount = await db.TechAccounts.FindAsync(id);
            if (techaccount == null)
            {
                return HttpNotFound();
            }
            return View(techaccount);
        }

        // POST: /TechAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Boss")]
        public async Task<ActionResult> Edit([Bind(Include="userName,firstName,lastName,initials,hashPassword,permissionGroup")] TechAccount techaccount)
        {
            if (ModelState.IsValid)
            {
                //lower case username
                techaccount.userName = techaccount.userName.ToLower();
                //hash the password with the apps secret key
                techaccount.hashPassword = EncryptionHelper.Encrypt(techaccount.hashPassword);
                db.Entry(techaccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(techaccount);
        }

        // GET: /TechAccount/Delete/5
        [AuthorizeAD(Groups = "Boss")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechAccount techaccount = await db.TechAccounts.FindAsync(id);
            if (techaccount == null)
            {
                return HttpNotFound();
            }
            return View(techaccount);
        }

        // POST: /TechAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Boss")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            TechAccount techaccount = await db.TechAccounts.FindAsync(id);
            db.TechAccounts.Remove(techaccount);
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
        }*/
    }
}
