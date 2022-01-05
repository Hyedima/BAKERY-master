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
    public class stocks_addedController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();
        // GET: stocks_added
        public ActionResult Index()
        {
            //string today = DateTime.Now.ToShortDateString();
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var stocks_added = db.stocks_added.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(s => s.stock).Include(s => s.useraccount);
            return View(stocks_added.ToList());
        }

        [HttpPost]
        public ActionResult Index( string startdate, string enddate)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var stocks_added = db.stocks_added.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(s => s.stock).Include(s => s.useraccount);
            return View(stocks_added.ToList());
        }

        // GET: stocks_added/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stocks_added stocks_added = db.stocks_added.Find(id);
            if (stocks_added == null)
            {
                return HttpNotFound();
            }
            return View(stocks_added);
        }

        // GET: stocks_added/Create
        public ActionResult Create()
        {
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id");
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: stocks_added/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,stock_id,qty_added,insertdate,insertuser")] stocks_added stocks_added)
        {

            if (ModelState.IsValid)
            {
                db.stocks_added.Add(stocks_added);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", stocks_added.stock_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stocks_added.insertuser);
            return View(stocks_added);
        }

        // GET: stocks_added/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stocks_added stocks_added = db.stocks_added.Find(id);
            if (stocks_added == null)
            {
                return HttpNotFound();
            }
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", stocks_added.stock_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stocks_added.insertuser);
            return View(stocks_added);
        }

        // POST: stocks_added/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,stock_id,qty_added,insertdate,insertuser")] stocks_added stocks_added)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stocks_added).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", stocks_added.stock_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stocks_added.insertuser);
            return View(stocks_added);
        }

        // GET: stocks_added/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stocks_added stocks_added = db.stocks_added.Find(id);
            if (stocks_added == null)
            {
                return HttpNotFound();
            }
            return View(stocks_added);
        }

        // POST: stocks_added/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            stocks_added stocks_added = db.stocks_added.Find(id);
            db.stocks_added.Remove(stocks_added);
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
