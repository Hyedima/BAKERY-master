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
    public class SalariesController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: Salaries
        public ActionResult Index()
        {
            var salaries = db.Salaries.Include(s => s.staff);
            return View(salaries.ToList());
        }

        // GET: Salaries/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salary salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }
            return View(salary);
        }

        // GET: Salaries/Create
        public ActionResult Create()
        {
            ViewBag.staffid = new SelectList(db.staffs, "id", "name");
            return View();
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,staffid,salary1,insertdate,status,decription")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                db.Salaries.Add(salary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.staffid = new SelectList(db.staffs, "id", "name", salary.staffid);
            return View(salary);
        }

        // GET: Salaries/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salary salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }
            ViewBag.staffid = new SelectList(db.staffs, "id", "name", salary.staffid);
            return View(salary);
        }

        // POST: Salaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,staffid,salary1,insertdate,status,decription")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.staffid = new SelectList(db.staffs, "id", "name", salary.staffid);
            return View(salary);
        }

        // GET: Salaries/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salary salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }
            return View(salary);
        }

        // POST: Salaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Salary salary = db.Salaries.Find(id);
            db.Salaries.Remove(salary);
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
