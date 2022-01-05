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
    public class transactionsController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: transactions
        public ActionResult Index()
        {
            var transactions = db.transactions.Include(t => t.Branch).Include(t => t.customer).Include(t => t.useraccount);
            return View(transactions.ToList());
        }

        // GET: transactions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaction transaction = db.transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: transactions/Create
        public ActionResult Create()
        {
            ViewBag.branchid = new SelectList(db.Branches, "id", "name");
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name");
            ViewBag.userid = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,userid,trnx_date,paymenttype,amount,description,branchid,customerid")] transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.branchid = new SelectList(db.Branches, "id", "name", transaction.branchid);
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", transaction.customerid);
            ViewBag.userid = new SelectList(db.useraccounts, "id", "branchid", transaction.userid);
            return View(transaction);
        }

        // GET: transactions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaction transaction = db.transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.branchid = new SelectList(db.Branches, "id", "name", transaction.branchid);
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", transaction.customerid);
            ViewBag.userid = new SelectList(db.useraccounts, "id", "branchid", transaction.userid);
            return View(transaction);
        }

        // POST: transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,userid,trnx_date,paymenttype,amount,description,branchid,customerid")] transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.branchid = new SelectList(db.Branches, "id", "name", transaction.branchid);
            ViewBag.customerid = new SelectList(db.customers, "id", "customer_name", transaction.customerid);
            ViewBag.userid = new SelectList(db.useraccounts, "id", "branchid", transaction.userid);
            return View(transaction);
        }

        // GET: transactions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaction transaction = db.transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            transaction transaction = db.transactions.Find(id);
            db.transactions.Remove(transaction);
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
