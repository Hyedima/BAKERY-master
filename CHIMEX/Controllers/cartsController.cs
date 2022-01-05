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
    public class cartsController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: carts
        public ActionResult Index()
        {
            var carts = db.carts.Include(c => c.customer).Include(c => c.product).Include(c => c.stock).Include(c => c.transaction).Include(c => c.useraccount);
            return View(carts.ToList());
        }

        // GET: carts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: carts/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name");
            ViewBag.product_id = new SelectList(db.products, "id", "product_name");
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id");
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "name");
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,stock_id,product_name,customer_id,transaction_id,qty,unitprice,total_amount,date_sold,sold_by,insert_user")] cart cart)
        {
            if (ModelState.IsValid)
            {
                db.carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", cart.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", cart.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", cart.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "name", cart.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", cart.insert_user);
            return View(cart);
        }

        // GET: carts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", cart.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", cart.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", cart.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "name", cart.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", cart.insert_user);
            return View(cart);
        }

        // POST: carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,stock_id,product_name,customer_id,transaction_id,qty,unitprice,total_amount,date_sold,sold_by,insert_user")] cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", cart.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", cart.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", cart.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "name", cart.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", cart.insert_user);
            return View(cart);
        }

        // GET: carts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            cart cart = db.carts.Find(id);
            db.carts.Remove(cart);
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
