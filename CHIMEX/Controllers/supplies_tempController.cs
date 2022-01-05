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
    public class supplies_tempController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: supplies_temp
        public ActionResult Index()
        {
            var supplies_temp = db.supplies_temp.Include(s => s.customer).Include(s => s.product);
            return View(supplies_temp.ToList());
        }

        // GET: supplies_temp/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplies_temp supplies_temp = db.supplies_temp.Find(id);
            if (supplies_temp == null)
            {
                return HttpNotFound();
            }
            return View(supplies_temp);
        }

        // GET: supplies_temp/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name");
            ViewBag.product_id = new SelectList(db.products, "id", "product_name");
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id");
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: supplies_temp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,stock_id,product_name,customer_id,transaction_id,qty,unitprice,total_amount,date_sold,sold_by,insert_user")] supplies_temp supplies_temp)
        {
            if (ModelState.IsValid)
            {
                db.supplies_temp.Add(supplies_temp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", supplies_temp.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", supplies_temp.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", supplies_temp.stock_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", supplies_temp.insert_user);
            return View(supplies_temp);
        }

        // GET: supplies_temp/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplies_temp supplies_temp = db.supplies_temp.Find(id);
            if (supplies_temp == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", supplies_temp.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", supplies_temp.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", supplies_temp.stock_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", supplies_temp.insert_user);
            return View(supplies_temp);
        }

        // POST: supplies_temp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,stock_id,product_name,customer_id,transaction_id,qty,unitprice,total_amount,date_sold,sold_by,insert_user")] supplies_temp supplies_temp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplies_temp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", supplies_temp.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", supplies_temp.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", supplies_temp.stock_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", supplies_temp.insert_user);
            return View(supplies_temp);
        }

        // GET: supplies_temp/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplies_temp supplies_temp = db.supplies_temp.Find(id);
            if (supplies_temp == null)
            {
                return HttpNotFound();
            }
            return View(supplies_temp);
        }

        // POST: supplies_temp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            supplies_temp supplies_temp = db.supplies_temp.Find(id);
            db.supplies_temp.Remove(supplies_temp);
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
