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
    public class BankDepositsController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: BankDeposits
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var bankDeposits = db.BankDeposits.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(b => b.useraccount);
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
                var bankDeposits = db.BankDeposits.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(b => b.useraccount);
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
                var bankDeposits = db.BankDeposits.Where(p => p.insertdate >= sdate && p.insertdate < edate && p.insertuser == userid).Include(b => b.useraccount);
                return View(bankDeposits.ToList());
            }

        }

        // GET: BankDeposits/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankDeposit bankDeposit = db.BankDeposits.Find(id);
            if (bankDeposit == null)
            {
                return HttpNotFound();
            }
            return View(bankDeposit);
        }

        // GET: BankDeposits/Create
        public ActionResult Create()
        {
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: BankDeposits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,deposited_by,bank,description,amount,insertdate,insertuser")] BankDeposit bankDeposit)
        {
            bankDeposit.insertuser = Session["userid"].ToString();
            bankDeposit.insertdate = DateTime.Now;
            bankDeposit.id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                db.BankDeposits.Add(bankDeposit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", bankDeposit.insertuser);
            return View(bankDeposit);
        }

        // GET: BankDeposits/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankDeposit bankDeposit = db.BankDeposits.Find(id);
            if (bankDeposit == null)
            {
                return HttpNotFound();
            }
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", bankDeposit.insertuser);
            return View(bankDeposit);
        }

        // POST: BankDeposits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,deposited_by,bank,description,amount,insertdate,insertuser")] BankDeposit bankDeposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankDeposit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", bankDeposit.insertuser);
            return View(bankDeposit);
        }

        // GET: BankDeposits/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankDeposit bankDeposit = db.BankDeposits.Find(id);
            if (bankDeposit == null)
            {
                return HttpNotFound();
            }
            return View(bankDeposit);
        }

        // POST: BankDeposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BankDeposit bankDeposit = db.BankDeposits.Find(id);
            db.BankDeposits.Remove(bankDeposit);
            db.SaveChanges();
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
