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
    public class BalancesController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: Balances
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var bankDeposits = db.Balances.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(b => b.useraccount);
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
                var bankDeposits = db.Balances.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(b => b.useraccount);
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
                var bankDeposits = db.Balances.Where(p => p.insertdate >= sdate && p.insertdate < edate && p.insertuser == userid).Include(b => b.useraccount);
                return View(bankDeposits.ToList());
            }

        }

        // GET: Balances/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Balance balance = db.Balances.Find(id);
            if (balance == null)
            {
                return HttpNotFound();
            }
            return View(balance);
        }

        // GET: Balances/Create
        public ActionResult Create()
        {
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: Balances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,balancetype,description,amount,insertuser,insertdate")] Balance balance)
        {
            balance.id = Guid.NewGuid().ToString();
            balance.insertdate = DateTime.Now;
            balance.insertuser = Session["userid"].ToString();
            if (ModelState.IsValid)
            {
                db.Balances.Add(balance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", balance.insertuser);
            return View(balance);
        }

        // GET: Balances/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Balance balance = db.Balances.Find(id);
            if (balance == null)
            {
                return HttpNotFound();
            }
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", balance.insertuser);
            return View(balance);
        }

        // POST: Balances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,balancetype,description,amount,insertuser,insertdate")] Balance balance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(balance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", balance.insertuser);
            return View(balance);
        }

        // GET: Balances/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Balance balance = db.Balances.Find(id);
            if (balance == null)
            {
                return HttpNotFound();
            }
            return View(balance);
        }

        // POST: Balances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Balance balance = db.Balances.Find(id);
            db.Balances.Remove(balance);
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
