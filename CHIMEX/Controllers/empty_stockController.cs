using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CHIMEX.Models;

namespace CHIMEX.Controllers
{
    public class empty_stockController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: empty_stock
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var trans = db.empty_stock.Where(p => p.insertdate >= sdate && p.insertdate < edate);
            return View(trans.ToList());
        }
        [HttpPost]
        public ActionResult Index(string startdate, string enddate, string id)
        {
            if (id == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var trans = db.empty_stock.Where(p => p.insertdate >= sdate && p.insertdate < edate);
                return View(trans.ToList());
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var trans = db.empty_stock.Where(p => p.insertdate >= sdate && p.insertdate < edate && p.insertuser == id);
                return View(trans.ToList());
            }

        }

        // GET: empty_stock/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empty_stock empty_stock = db.empty_stock.Find(id);
            if (empty_stock == null)
            {
                return HttpNotFound();
            }
            return View(empty_stock);
        }

        // GET: empty_stock/Create
        public ActionResult Create()
        {
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: empty_stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,openingstock,empty_received,plastic_recieved,backload,notes,insertuser,insertdate")] empty_stock empty_stock)
        {
            if (ModelState.IsValid)
            {
                db.empty_stock.Add(empty_stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", empty_stock.insertuser);
            return View(empty_stock);
        }

        // GET: empty_stock/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empty_stock empty_stock = db.empty_stock.Find(id);
            if (empty_stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", empty_stock.insertuser);
            return View(empty_stock);
        }

        // POST: empty_stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,openingstock,empty_received,plastic_recieved,backload,notes,insertuser,insertdate")] empty_stock empty_stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empty_stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", empty_stock.insertuser);
            return View(empty_stock);
        }

        // GET: empty_stock/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empty_stock empty_stock = db.empty_stock.Find(id);
            if (empty_stock == null)
            {
                return HttpNotFound();
            }
            return View(empty_stock);
        }

        // POST: empty_stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            empty_stock empty_stock = db.empty_stock.Find(id);
            db.empty_stock.Remove(empty_stock);
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
