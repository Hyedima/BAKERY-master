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
    public class salesmenController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: salesmen
        public ActionResult Index()
        {
            return View(db.salesmen.ToList());
        }

        // GET: salesmen/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salesman salesman = db.salesmen.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View(salesman);
        }

        // GET: salesmen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: salesmen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,firstname,lastname,othernames,phone,email,address,dob,regdate")] salesman salesman)
        {
            salesman.id = Guid.NewGuid().ToString();
            salesman.regdate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.salesmen.Add(salesman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesman);
        }

        // GET: salesmen/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salesman salesman = db.salesmen.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View(salesman);
        }

        // POST: salesmen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,firstname,lastname,othernames,phone,email,address,dob,regdate")] salesman salesman)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesman).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesman);
        }

        // GET: salesmen/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salesman salesman = db.salesmen.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View(salesman);
        }

        // POST: salesmen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            salesman salesman = db.salesmen.Find(id);
            db.salesmen.Remove(salesman);
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
