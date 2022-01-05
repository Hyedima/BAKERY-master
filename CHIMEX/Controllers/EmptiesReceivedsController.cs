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
    public class EmptiesReceivedsController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: EmptiesReceiveds
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var emptiesReceiveds = db.EmptiesReceiveds.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.customer).Include(e => e.Emptytype);
            return View(emptiesReceiveds.ToList());
        }
        [HttpPost]
        public ActionResult Index(string startdate, string enddate, string customerid)
        {
            if (customerid == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var emptiesReceiveds = db.EmptiesReceiveds.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.customer).Include(e => e.Emptytype);
                return View(emptiesReceiveds.ToList());
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var emptiesReceiveds = db.EmptiesReceiveds.Where(p => p.insertdate >= sdate && p.insertdate < edate && p.customerid == customerid).Include(e => e.customer).Include(e => e.Emptytype);
                return View(emptiesReceiveds.ToList());
            }
        }
        [HttpPost]
        public ActionResult Received(string itemid, string customerid, string qty, string description)
        {
            db.EmptiesReceiveds.Add(new EmptiesReceived
            {
                id = Guid.NewGuid().ToString(),
                customerid = customerid,
                itemid = itemid,
                amount = 0,
                qty = Convert.ToInt32(qty),
                notes = description,
                insertdate = DateTime.Now,
                insertuser = Session["userid"].ToString()
            });
            db.SaveChanges();

            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var emptiesReceiveds = db.EmptiesReceiveds.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.customer).Include(e => e.Emptytype);
            return RedirectToAction("Index", emptiesReceiveds.ToList());
        }

        // GET: EmptiesReceiveds/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmptiesReceived emptiesReceived = db.EmptiesReceiveds.Find(id);
            if (emptiesReceived == null)
            {
                return HttpNotFound();
            }
            return View(emptiesReceived);
        }

        // GET: EmptiesReceiveds/Create
        public ActionResult Create()
        {
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name");
            ViewBag.itemid = new SelectList(db.Emptytypes, "id", "name");
            return View();
        }

        // POST: EmptiesReceiveds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,emptcustid,customerid,itemid,qty,amount,notes,insertdate,insertuser")] EmptiesReceived emptiesReceived)
        {
            if (ModelState.IsValid)
            {
                db.EmptiesReceiveds.Add(emptiesReceived);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", emptiesReceived.customerid);
            ViewBag.itemid = new SelectList(db.Emptytypes, "id", "name", emptiesReceived.itemid);
            return View(emptiesReceived);
        }

        // GET: EmptiesReceiveds/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmptiesReceived emptiesReceived = db.EmptiesReceiveds.Find(id);
            if (emptiesReceived == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", emptiesReceived.customerid);
            ViewBag.itemid = new SelectList(db.Emptytypes, "id", "name", emptiesReceived.itemid);
            return View(emptiesReceived);
        }

        // POST: EmptiesReceiveds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,emptcustid,customerid,itemid,qty,amount,notes,insertdate,insertuser")] EmptiesReceived emptiesReceived)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emptiesReceived).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", emptiesReceived.customerid);
            ViewBag.itemid = new SelectList(db.Emptytypes, "id", "name", emptiesReceived.itemid);
            return View(emptiesReceived);
        }

        // GET: EmptiesReceiveds/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmptiesReceived emptiesReceived = db.EmptiesReceiveds.Find(id);
            if (emptiesReceived == null)
            {
                return HttpNotFound();
            }
            return View(emptiesReceived);
        }

        // POST: EmptiesReceiveds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EmptiesReceived emptiesReceived = db.EmptiesReceiveds.Find(id);
            db.EmptiesReceiveds.Remove(emptiesReceived);
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
