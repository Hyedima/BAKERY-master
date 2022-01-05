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
    public class Empties_BackloadController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: Empties_Backload
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var empties_Backload = db.Empties_Backload.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.Emptytype).Include(e => e.useraccount);
            return View(empties_Backload.ToList());
        }
        [HttpPost]
        public ActionResult Index(string startdate, string enddate)
        {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var empties_Backload = db.Empties_Backload.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.Emptytype).Include(e => e.useraccount);
                return View(empties_Backload.ToList());
        }

        // GET: Empties_Backload/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empties_Backload empties_Backload = db.Empties_Backload.Find(id);
            if (empties_Backload == null)
            {
                return HttpNotFound();
            }
            return View(empties_Backload);
        }

        // GET: Empties_Backload/Create
        public ActionResult Create()
        {
            ViewBag.emptytypeid = new SelectList(db.Emptytypes, "id", "name");
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: Empties_Backload/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,emptytypeid,qty,description,insertuser,insertdate")] Empties_Backload empties_Backload)
        {
            if (ModelState.IsValid)
            {
                db.Empties_Backload.Add(empties_Backload);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.emptytypeid = new SelectList(db.Emptytypes, "id", "name", empties_Backload.emptytypeid);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", empties_Backload.insertuser);
            return View(empties_Backload);
        }

        // GET: Empties_Backload/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empties_Backload empties_Backload = db.Empties_Backload.Find(id);
            if (empties_Backload == null)
            {
                return HttpNotFound();
            }
            ViewBag.emptytypeid = new SelectList(db.Emptytypes, "id", "name", empties_Backload.emptytypeid);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", empties_Backload.insertuser);
            return View(empties_Backload);
        }

        // POST: Empties_Backload/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,emptytypeid,qty,description,insertuser,insertdate")] Empties_Backload empties_Backload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empties_Backload).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.emptytypeid = new SelectList(db.Emptytypes, "id", "name", empties_Backload.emptytypeid);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", empties_Backload.insertuser);
            return View(empties_Backload);
        }

        // GET: Empties_Backload/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empties_Backload empties_Backload = db.Empties_Backload.Find(id);
            if (empties_Backload == null)
            {
                return HttpNotFound();
            }
            return View(empties_Backload);
        }

        // POST: Empties_Backload/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Empties_Backload empties_Backload = db.Empties_Backload.Find(id);
            db.Empties_Backload.Remove(empties_Backload);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Backload(string itemid, string qty, string description)
        {
            //if(itemid == "2")
           // {
           //     qty = (Convert.ToInt32(qty) / 2).ToString();
           // }
            db.Empties_Backload.Add(new Empties_Backload
            {
                id = Guid.NewGuid().ToString(),
                emptytypeid = itemid,
                qty = Convert.ToInt32(qty),
                description = description,
                insertuser = Session["userid"].ToString(),
                insertdate = DateTime.Now
            });
            db.SaveChanges();

            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var empties_Backload = db.Empties_Backload.Where(p => p.insertdate >= sdate && p.insertdate < edate).Include(e => e.Emptytype).Include(e => e.useraccount);
            return View("Index",empties_Backload.ToList());
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
