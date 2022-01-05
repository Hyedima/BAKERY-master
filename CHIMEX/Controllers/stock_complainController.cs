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
    public class stock_complainController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: stock_complain
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var trans = db.stock_complain.Where(p => p.insertdate >= sdate && p.insertdate < edate);
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
                var trans = db.stock_complain.Where(p => p.insertdate >= sdate && p.insertdate < edate);
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
                var trans = db.stock_complain.Where(p => p.insertdate >= sdate && p.insertdate < edate && p.insertuser== id);
                return View(trans.ToList());
            }

        }

        // GET: stock_complain/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock_complain stock_complain = db.stock_complain.Find(id);
            if (stock_complain == null)
            {
                return HttpNotFound();
            }
            return View(stock_complain);
        }

        // GET: stock_complain/Create
        public ActionResult Create()
        {
            ViewBag.itemid = new SelectList(db.products, "id", "product_name");
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: stock_complain/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,itemid,itemname,qty,notes,insertuser,insertdate")] stock_complain stock_complain)
        {
            if (ModelState.IsValid)
            {
                db.stock_complain.Add(stock_complain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.itemid = new SelectList(db.products, "id", "product_name", stock_complain.itemid);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stock_complain.insertuser);
            return View(stock_complain);
        }

        // GET: stock_complain/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock_complain stock_complain = db.stock_complain.Find(id);
            if (stock_complain == null)
            {
                return HttpNotFound();
            }
            ViewBag.itemid = new SelectList(db.products, "id", "product_name", stock_complain.itemid);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stock_complain.insertuser);
            return View(stock_complain);
        }

        // POST: stock_complain/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,itemid,itemname,qty,notes,insertuser,insertdate")] stock_complain stock_complain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock_complain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.itemid = new SelectList(db.products, "id", "product_name", stock_complain.itemid);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stock_complain.insertuser);
            return View(stock_complain);
        }

        // GET: stock_complain/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock_complain stock_complain = db.stock_complain.Find(id);
            if (stock_complain == null)
            {
                return HttpNotFound();
            }
            return View(stock_complain);
        }

        // POST: stock_complain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            stock_complain stock_complain = db.stock_complain.Find(id);
            db.stock_complain.Remove(stock_complain);
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
