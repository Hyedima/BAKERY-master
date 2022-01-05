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
    public class bonusController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: bonus
        public ActionResult Index()
        {
            var bonus = db.bonus.Include(b => b.staff);
            return View(bonus.ToList());
        }

        // GET: bonus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bonu bonu = db.bonus.Find(id);
            if (bonu == null)
            {
                return HttpNotFound();
            }
            return View(bonu);
        }

        // GET: bonus/Create
        public ActionResult Create()
        {
            ViewBag.staffid = new SelectList(db.staffs, "id", "name");
            return View();
        }

        // POST: bonus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,staffid,walfare,insertdate,status,decription")] bonu bonu)
        {
            if (ModelState.IsValid)
            {
                db.bonus.Add(bonu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.staffid = new SelectList(db.staffs, "id", "name", bonu.staffid);
            return View(bonu);
        }

        // GET: bonus/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bonu bonu = db.bonus.Find(id);
            if (bonu == null)
            {
                return HttpNotFound();
            }
            ViewBag.staffid = new SelectList(db.staffs, "id", "name", bonu.staffid);
            return View(bonu);
        }

        // POST: bonus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,staffid,walfare,insertdate,status,decription")] bonu bonu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bonu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.staffid = new SelectList(db.staffs, "id", "name", bonu.staffid);
            return View(bonu);
        }

        // GET: bonus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bonu bonu = db.bonus.Find(id);
            if (bonu == null)
            {
                return HttpNotFound();
            }
            return View(bonu);
        }

        // POST: bonus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            bonu bonu = db.bonus.Find(id);
            db.bonus.Remove(bonu);
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
