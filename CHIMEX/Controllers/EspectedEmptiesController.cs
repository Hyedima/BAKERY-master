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
    public class EspectedEmptiesController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: EspectedEmpties
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var espectedEmpties = db.EspectedEmpties.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.customer).Include(e => e.Emptytype);
            return View(espectedEmpties.ToList());
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
                var espectedEmpties = db.EspectedEmpties.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.customer).Include(e => e.Emptytype);
                return View(espectedEmpties.ToList());
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var espectedEmpties = db.EspectedEmpties.Where(p => p.insertdate >= sdate && p.insertdate < edate && p.customerid == customerid).Include(e => e.customer).Include(e => e.Emptytype);
                return View(espectedEmpties.ToList());
            }
        }
        [HttpPost]
        public ActionResult Owing(string itemid, string customerid, string qty, string description)
        {
            db.EspectedEmpties.Add(new EspectedEmpty
            {
                id = Guid.NewGuid().ToString(),
                customerid = customerid,
                itemid = itemid,
                qty = Convert.ToInt32(qty),
                amount = 0,
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
            var espectedEmpties = db.EspectedEmpties.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.customer).Include(e => e.Emptytype);
            return RedirectToAction("Index", espectedEmpties.ToList());
        }
        // GET: EspectedEmpties/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EspectedEmpty espectedEmpty = db.EspectedEmpties.Find(id);
            if (espectedEmpty == null)
            {
                return HttpNotFound();
            }
            return View(espectedEmpty);
        }

        // GET: EspectedEmpties/Create
        public ActionResult Create()
        {
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name");
            ViewBag.itemid = new SelectList(db.Emptytypes, "id", "name");
            return View();
        }

        // POST: EspectedEmpties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,emptcustid,customerid,itemid,qty,amount,notes,insertdate,insertuser")] EspectedEmpty espectedEmpty)
        {
            if (ModelState.IsValid)
            {
                db.EspectedEmpties.Add(espectedEmpty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", espectedEmpty.customerid);
            ViewBag.itemid = new SelectList(db.Emptytypes, "id", "name", espectedEmpty.itemid);
            return View(espectedEmpty);
        }

        // GET: EspectedEmpties/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EspectedEmpty espectedEmpty = db.EspectedEmpties.Find(id);
            if (espectedEmpty == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", espectedEmpty.customerid);
            ViewBag.itemid = new SelectList(db.Emptytypes, "id", "name", espectedEmpty.itemid);
            return View(espectedEmpty);
        }

        // POST: EspectedEmpties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,emptcustid,customerid,itemid,qty,amount,notes,insertdate,insertuser")] EspectedEmpty espectedEmpty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(espectedEmpty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", espectedEmpty.customerid);
            ViewBag.itemid = new SelectList(db.Emptytypes, "id", "name", espectedEmpty.itemid);
            return View(espectedEmpty);
        }

        // GET: EspectedEmpties/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EspectedEmpty espectedEmpty = db.EspectedEmpties.Find(id);
            if (espectedEmpty == null)
            {
                return HttpNotFound();
            }
            return View(espectedEmpty);
        }

        // POST: EspectedEmpties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EspectedEmpty espectedEmpty = db.EspectedEmpties.Find(id);
            db.EspectedEmpties.Remove(espectedEmpty);
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
