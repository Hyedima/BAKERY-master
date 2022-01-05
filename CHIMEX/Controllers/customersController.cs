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
    public class customersController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: customers
        public ActionResult Index()
        {
            return View(db.customers.ToList());
        }
        public ActionResult CustomerAccountsReport()
        {
            return View(db.customers.ToList());
        }
        public ActionResult CustomerAccounts()
        {
            return View(db.customers.ToList());
        }
        [HttpPost]
        public ActionResult updateAccount(string amount, string userid, string customerid, string transactiontype, string paymenttype, string receivedfrom, string description, string username  )
        {
            //create transaction id
            string trnxid = Guid.NewGuid().ToString();
            //get customers current account balace
            var bal = db.customer_ledger.Where(p => p.customerid == customerid).OrderByDescending(p => p.insertdate).FirstOrDefault();

            //get logged in user
            var usr = db.useraccounts.FirstOrDefault(p=>p.id == userid);

            decimal credit = 0;
            decimal debit = 0;
            decimal balance = 0;
            string status = ""; //Credited, Debited, Pending or Paid

            if (transactiontype == "DEBITED")
            {
                debit = Convert.ToDecimal(amount);
                status = "DEBITED";
                if (bal != null)
                {
                    balance = Convert.ToDecimal(bal.balance - debit);
                }
                
            }
            //else { }

            if (transactiontype == "CREDITED")
            {
                credit = Convert.ToDecimal(amount);
                status = "CREDITED";
                if(bal != null)
                {
                    balance = Convert.ToDecimal(bal.balance + credit);
                }
            }
            //else { }

            db.transactions.Add(new transaction
            {
                id = trnxid,
                branchid = usr.branchid,
                name = transactiontype,
                userid = userid,
                trnx_date = DateTime.Now,
                paymenttype = paymenttype,
                amount = Convert.ToDecimal(amount),
                customerid = customerid,
                description = description,
                status = status
            });
            db.customer_ledger.Add(new customer_ledger
            {
                id = Guid.NewGuid().ToString(),
                transactionid = trnxid,
                customerid = customerid,
                receivedfrom = receivedfrom,
                description = description,
                paymenttype = paymenttype,
                credit = credit,
                debit = debit,
                balance = balance,
                insertdate = DateTime.Now,
                insertuser = userid,
                username = usr.firstname + " "+ usr.lastname + " "+ usr.othernames
            });
            db.SaveChanges();
            return RedirectToAction("Details/"+customerid);
        }

        // GET: customers/Details/5
        public ActionResult Details(string id)
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        public ActionResult Details(string id, string startdate, string enddate)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,customer_name,phone,email,address,insert_date,customertype, insertuser")] customer customer)
        {
            customer.insert_date = DateTime.Now;
            customer.insertuser = Session["userid"].ToString();
            if (ModelState.IsValid)
            {
                db.customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,customer_name,phone,email,address,insert_date,customertype,insertuser")] customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
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
