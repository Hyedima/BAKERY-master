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
    public class order_cartController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: order_cart
        public ActionResult Index()
        {
            var order_cart = db.order_cart.Include(o => o.customer).Include(o => o.Order).Include(o => o.product).Include(o => o.stock).Include(o => o.transaction).Include(o => o.useraccount);
            return View(order_cart.ToList());
        }

        // GET: order_cart/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_cart order_cart = db.order_cart.Find(id);
            if (order_cart == null)
            {
                return HttpNotFound();
            }
            return View(order_cart);
        }

        // GET: order_cart/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name");
            ViewBag.order_id = new SelectList(db.Orders, "id", "customerid");
            ViewBag.product_id = new SelectList(db.products, "id", "product_name");
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id");
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "branchid");
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: order_cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,stock_id,product_name,customer_id,transaction_id,qty,unitprice,total_amount,date_sold,sold_by,insert_user,order_id")] order_cart order_cart)
        {
            if (ModelState.IsValid)
            {
                db.order_cart.Add(order_cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", order_cart.customer_id);
            ViewBag.order_id = new SelectList(db.Orders, "id", "customerid", order_cart.order_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", order_cart.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", order_cart.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "branchid", order_cart.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", order_cart.insert_user);
            return View(order_cart);
        }

        // GET: order_cart/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_cart order_cart = db.order_cart.Find(id);
            if (order_cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", order_cart.customer_id);
            ViewBag.order_id = new SelectList(db.Orders, "id", "customerid", order_cart.order_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", order_cart.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", order_cart.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "branchid", order_cart.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", order_cart.insert_user);
            return View(order_cart);
        }

        // POST: order_cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,stock_id,product_name,customer_id,transaction_id,qty,unitprice,total_amount,date_sold,sold_by,insert_user,order_id")] order_cart order_cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", order_cart.customer_id);
            ViewBag.order_id = new SelectList(db.Orders, "id", "customerid", order_cart.order_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", order_cart.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", order_cart.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "branchid", order_cart.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", order_cart.insert_user);
            return View(order_cart);
        }

        // GET: order_cart/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_cart order_cart = db.order_cart.Find(id);
            if (order_cart == null)
            {
                return HttpNotFound();
            }
            return View(order_cart);
        }

        // POST: order_cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            order_cart order_cart = db.order_cart.Find(id);
            db.order_cart.Remove(order_cart);
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
