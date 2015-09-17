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
using WebInvoicing.ActiveDirectoryHelpers;

namespace WebBilling.Controllers
{
    public class InvoicesController : Controller
    {
        private CompXDbContext db = new CompXDbContext();

        // GET: /Invoices/
        public async Task<ActionResult> Index(string account, string time)
        {
            //only show your own invoices past 3 months, 90 days
            DateTime cutoffDate = DateTime.Now.Subtract(new TimeSpan(90, 0, 0, 0));
            var invoices = db.Invoices.Where(i => i.customerID.ToLower() == User.Identity.Name.ToLower()).Include(i => i.PDFFile);
            if (string.IsNullOrWhiteSpace(time))
            {
                //then use cutoff date
                invoices = invoices.Where(i => i.date > cutoffDate);
            }
            else
                ViewBag.time = "all";
            if (LDAPHelper.UserIsMemberOfGroupOC("Boss", User.Identity.Name) || LDAPHelper.UserIsMemberOfGroupOC("Admins", User.Identity.Name))
            {
                if (string.IsNullOrWhiteSpace(account))
                    invoices = db.Invoices.Include(i => i.PDFFile);
                else
                    invoices = db.Invoices.Where(i => i.customerID.ToLower() == account.ToLower()).Include(i => i.PDFFile);
            }
            return View(await invoices.ToListAsync());
        }

        // GET: /Invoices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoices invoices = await db.Invoices.FindAsync(id);
            if (invoices == null)
            {
                return HttpNotFound();
            }
            return View(invoices);
        }

        // GET: /Invoices/Create
        [AuthorizeAD(Groups = "Boss,Admins")]
        public ActionResult Create()
        {
            ViewBag.PDFFileID = new SelectList(db.PDFFiles, "ID", "ID");
           // List<UserAccount> customers = db.UserAccounts.ToList().Where(ua => ua.permissionGroup == "Customer").ToList();
            ViewBag.customerID = new SelectList(db.UserAccounts.ToList().Where(ua => ua.permissionGroup == "Customer"), "userName", "userName").OrderBy(c => c.Text);
            return View();
        }

        // POST: /Invoices/
        [HttpPost]
        public async Task<ActionResult> ChangePaid(int id, bool value)
        {
            Invoices invoices = await db.Invoices.FindAsync(id);
            if (invoices == null)
            {
                return Json(new { Success = false });
            }
            invoices.paid = value;
            db.Entry(invoices).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Json(new { Success = true });
        }

        // POST: /Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeAD(Groups = "Boss,Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,customerID,name,date,paid")] Invoices invoices, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0 && upload.FileName.EndsWith(".pdf"))
                {
                    var pdf = new PDFFile();
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        pdf.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    db.PDFFiles.Add(pdf);
                    invoices.PDFFileID = pdf.ID;
                }
                db.Invoices.Add(invoices);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PDFFileID = new SelectList(db.PDFFiles, "ID", "ID", invoices.PDFFileID);
            ViewBag.customerID = new SelectList(db.UserAccounts.ToList().Where(ua => ua.permissionGroup == "Customer"), "userName", "userName").OrderBy(c => c.Text);
            return View(invoices);
        }

        // GET: /File/
        public async Task<ActionResult> PDFFile(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoices invoices = await db.Invoices.FindAsync(id);
            if (invoices == null || invoices.PDFFileID == null)
            {
                return HttpNotFound();
            }
            if (LDAPHelper.UserIsMemberOfGroupOC("Boss", User.Identity.Name) || LDAPHelper.UserIsMemberOfGroupOC("Admins", User.Identity.Name) || invoices.customerID.ToLower() == User.Identity.Name.ToLower())
            {
                var fileToRetrieve = db.PDFFiles.Find(invoices.PDFFileID);
                if (fileToRetrieve == null)
                {
                    return HttpNotFound();
                }
                return File(fileToRetrieve.Content, "application/pdf", "Computrex_Invoice" + invoices.name + ".pdf");
            }
            else
                return HttpNotFound();
        }

        // GET: /Invoices/Edit/5
        [AuthorizeAD(Groups = "Boss,Admins")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoices invoices = await db.Invoices.FindAsync(id);
            if (invoices == null)
            {
                return HttpNotFound();
            }
            ViewBag.PDFFileID = new SelectList(db.PDFFiles, "ID", "ID", invoices.PDFFileID);
            return View(invoices);
        }

        // POST: /Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeAD(Groups = "Boss,Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ID,customerID,name,date,paid,PDFFileID")] Invoices invoices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoices).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PDFFileID = new SelectList(db.PDFFiles, "ID", "ID", invoices.PDFFileID);
            return View(invoices);
        }

        // GET: /Invoices/Delete/5
        [AuthorizeAD(Groups = "Boss,Admins")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoices invoices = await db.Invoices.FindAsync(id);
            if (invoices == null)
            {
                return HttpNotFound();
            }
            return View(invoices);
        }

        // POST: /Invoices/Delete/5
        [AuthorizeAD(Groups = "Boss,Admins")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Invoices invoices = await db.Invoices.FindAsync(id);
            db.Invoices.Remove(invoices);
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
