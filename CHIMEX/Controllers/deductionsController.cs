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
    public class deductionsController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: deductions
        public ActionResult Index()
        {
            var deductions = db.deductions.Include(d => d.staff);
            return View(deductions.ToList());
        }

        // GET: deductions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deduction deduction = db.deductions.Find(id);
            if (deduction == null)
            {
                return HttpNotFound();
            }
            return View(deduction);
        }

        // GET: deductions/Create
        public ActionResult Create()
        {
            ViewBag.staffid = new SelectList(db.staffs, "id", "name");
            return View();
        }

        // POST: deductions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,staffid,item,insertdate,status,decription")] deduction deduction)
        {
            if (ModelState.IsValid)
            {
                db.deductions.Add(deduction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.staffid = new SelectList(db.staffs, "id", "name", deduction.staffid);
            return View(deduction);
        }

        // GET: deductions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deduction deduction = db.deductions.Find(id);
            if (deduction == null)
            {
                return HttpNotFound();
            }
            ViewBag.staffid = new SelectList(db.staffs, "id", "name", deduction.staffid);
            return View(deduction);
        }

        // POST: deductions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,staffid,item,insertdate,status,decription")] deduction deduction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deduction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.staffid = new SelectList(db.staffs, "id", "name", deduction.staffid);
            return View(deduction);
        }

        // GET: deductions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deduction deduction = db.deductions.Find(id);
            if (deduction == null)
            {
                return HttpNotFound();
            }
            return View(deduction);
        }

        // POST: deductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            deduction deduction = db.deductions.Find(id);
            db.deductions.Remove(deduction);
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
