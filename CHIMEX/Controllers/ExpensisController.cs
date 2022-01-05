using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CHIMEX.Models;
using CHIMEX.Setup;

namespace CHIMEX.Controllers
{
    [CheckAuthentication]
    public class ExpensisController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: Expensis
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var bankDeposits = db.Expensis.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(b => b.useraccount).Include(b => b.useraccount1);
            return View(bankDeposits.ToList());
        }
        [HttpPost]
        public ActionResult Index(string startdate, string enddate, string userid)
        {
            if (userid == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var bankDeposits = db.Expensis.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(b => b.useraccount).Include(b => b.useraccount1);
                return View(bankDeposits.ToList());
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var bankDeposits = db.Expensis.Where(p => p.insertdate >= sdate && p.insertdate < edate && p.accountant == userid).Include(b => b.useraccount).Include(b => b.useraccount1);
                return View(bankDeposits.ToList());
            }

        }
        public ActionResult ExpensisCash()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var bankDeposits = db.Expensis.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(b => b.useraccount).Include(b => b.useraccount1);
            return View(bankDeposits.ToList());
        }
        [HttpPost]
        public ActionResult ExpensisCash(string startdate, string enddate, string userid)
        {
            if (userid == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var bankDeposits = db.Expensis.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(b => b.useraccount).Include(b => b.useraccount1);
                return View(bankDeposits.ToList());
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var bankDeposits = db.Expensis.Where(p => p.insertdate >= sdate && p.insertdate < edate && p.expensistype == userid).Include(b => b.useraccount).Include(b => b.useraccount1);
                return View(bankDeposits.ToList());
            }

        }

        // GET: Expensis/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expensi expensi = db.Expensis.Find(id);
            if (expensi == null)
            {
                return HttpNotFound();
            }
            return View(expensi);
        }
        public ActionResult PostExpensis()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostExpensis([Bind(Include = "id,head,description,amount,accountant,cashier,status,insertdate,paymentdate,paymenttype,expensistype")] Expensi expensi)
        {
            string id = Guid.NewGuid().ToString();
            expensi.id = id;
            expensi.insertdate = DateTime.Now;
            expensi.status = "POSTED";
            expensi.accountant = Session["userid"].ToString();
            //expensi.
            if (ModelState.IsValid)
            {
                db.Expensis.Add(expensi);
                db.SaveChanges();
                // return RedirectToAction("ExpensisCash");
                return RedirectToActionPermanent("Details/" + id, "Expensis");
            }
            ViewBag.accountant = new SelectList(db.useraccounts, "id", "branchid", expensi.accountant);
            ViewBag.cashier = new SelectList(db.useraccounts, "id", "branchid", expensi.cashier);
            return View(expensi);
        }

        // GET: Expensis/Create
        public ActionResult Create()
        {
            ViewBag.accountant = new SelectList(db.useraccounts, "id", "branchid");
            ViewBag.cashier = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: Expensis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,head,description,amount,accountant,cashier,status,insertdate,paymentdate,paymenttype,expensistype")] Expensi expensi)
        {
            string id = Guid.NewGuid().ToString();
            expensi.id = id;
            expensi.insertdate = DateTime.Now;
            expensi.status = "POSTED";
            expensi.accountant = Session["userid"].ToString();
            //expensi.
            if (ModelState.IsValid)
            {
                db.Expensis.Add(expensi);
                db.SaveChanges();
               // return RedirectToAction("ExpensisCash");
                return RedirectToActionPermanent("Details/" + id, "Expensis");
            }
            ViewBag.accountant = new SelectList(db.useraccounts, "id", "branchid", expensi.accountant);
            ViewBag.cashier = new SelectList(db.useraccounts, "id", "branchid", expensi.cashier);
            return View(expensi);
        }

        // GET: Expensis/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expensi expensi = db.Expensis.Find(id);
            if (expensi == null)
            {
                return HttpNotFound();
            }
            ViewBag.accountant = new SelectList(db.useraccounts, "id", "branchid", expensi.accountant);
            ViewBag.cashier = new SelectList(db.useraccounts, "id", "branchid", expensi.cashier);
            return View(expensi);
        }

        // POST: Expensis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,head,description,amount,accountant,cashier,status,insertdate,paymentdate,paymenttype,expensistype")] Expensi expensi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expensi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.accountant = new SelectList(db.useraccounts, "id", "branchid", expensi.accountant);
            ViewBag.cashier = new SelectList(db.useraccounts, "id", "branchid", expensi.cashier);
            return View(expensi);
        }

        // GET: Expensis/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expensi expensi = db.Expensis.Find(id);
            if (expensi == null)
            {
                return HttpNotFound();
            }
            return View(expensi);
        }

        // POST: Expensis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Expensi expensi = db.Expensis.Find(id);
            db.Expensis.Remove(expensi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Approve(string id)
        {
            var expens = db.Expensis.Find(id);
            expens.cashier = Session["userid"].ToString();
            expens.paymentdate = DateTime.Now;
            expens.status = "Paid";
            db.SaveChanges();
            return RedirectToActionPermanent("ExpensisCash");
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
