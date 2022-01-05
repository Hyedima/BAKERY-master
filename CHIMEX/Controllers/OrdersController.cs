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
    public class OrdersController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,customerid,orderdate,ordertime,insertdate,insertuser,notes")] Order order, string orderdate, string ordertime, string notes, string customerid)
        {
            if (ModelState.IsValid)
            {
                order.id = Guid.NewGuid().ToString();
                order.orderdate = Convert.ToDateTime(orderdate);
                order.ordertime = ordertime;
                order.insertuser = Session["userid"].ToString();
                order.insertdate = DateTime.Now;
                order.notes = notes;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,customerid,orderdate,ordertime,insertdate,insertuser,notes")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //new order
        public ActionResult OrderItems(string id)
        {
            ViewBag.id = id;
            var order = db.Orders.FirstOrDefault(p => p.id == id);
            if(order.status == "Supplied")
            {
                return RedirectToActionPermanent("Receipt/" + id + "_Order", "Sales");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Addcart(string orderid, string stockid, string productid, string customerid, string qty, string unitprice, string userid)
        {
            var stck = db.stocks.FirstOrDefault(p => p.id == stockid);
            var cart = db.order_cart.FirstOrDefault(p => p.stock_id == stockid && p.insert_user == userid);
            /*if (stck.qty < Convert.ToInt32(qty))
            {
                ViewBag.msg = "Please quantity must not be greater than the quantity at hand.";
            }
            else
            {
               
            }*/
            if (cart != null)
            {
                cart.unitprice = Convert.ToDecimal(unitprice);
                cart.qty += Convert.ToInt32(qty);
                db.SaveChanges();
            }
            else
            {
                var newcart = new order_cart
                {
                    id = Guid.NewGuid().ToString(),
                    product_id = productid,
                    stock_id = stockid,
                    product_name = stck.product.product_name,
                    //customer_id = customerid,
                    //transaction_id = ""
                    qty = Convert.ToInt32(qty),
                    unitprice = Convert.ToDecimal(unitprice),
                    total_amount = Convert.ToInt32(qty) * Convert.ToDecimal(unitprice),
                    date_sold = DateTime.Now,
                    sold_by = Session["userid"].ToString(),
                    insert_user = Session["userid"].ToString(),
                    order_id = orderid
                };
                db.order_cart.Add(newcart);
                db.SaveChanges();
            }

            return RedirectToActionPermanent("OrderItems/"+orderid, "Orders");
        }
        [HttpPost]
        public ActionResult DeleteCart(string id, string orderid)
        {
            var crt = db.order_cart.FirstOrDefault(p => p.id == id);
            db.order_cart.Remove(crt);
            db.SaveChanges();
            return RedirectToActionPermanent("OrderItems/"+orderid, "Orders");
        }
        [HttpPost]
        public ActionResult PostOrder(string id)
        {
            var ord = db.Orders.FirstOrDefault(p => p.id == id);
            ord.status = "Posted";
            db.SaveChanges();
            return View(ord);
        }

        [HttpPost]
        public ActionResult SaleSaveTransaction(FormCollection form)
        {
            string order_id = form["order_id"];
            string id = order_id+"_Order"; //Setup.GenerateID.GetID();
            string UserID = form["userid"];
            var User = db.useraccounts.FirstOrDefault(p => p.id == UserID);
            string custid = form["customer"];
            string type = form["paymenttype"];
            string transactiontype = form["transactiontype"];
            string description = form["description"];
            decimal totalamount = Convert.ToDecimal(form["totalamount"]);
            decimal amountpaid = Convert.ToDecimal(form["amountpaid"]);
            string status = "Paid";

            var order = db.Orders.FirstOrDefault(p => p.id == order_id);
            order.status = "Supplied";


            if (transactiontype == "CREDIT")
            {
                //status = "Pending";
            }

            var cust = db.customers.Find(custid);
            var usr = db.useraccounts.Find(UserID);
            this.db.transactions.Add(new transaction()
            {
                id = id,
                name = "Sales",
                userid = UserID,
                branchid = User.branchid,
                paymenttype = type,
                customerid = custid,
                description = description,
                trnx_date = DateTime.Now,
                status = status
            });
            this.db.SaveChanges();
            //

            List<order_cart> list = this.db.order_cart.Where<order_cart>(c => c.insert_user == UserID).ToList<order_cart>();
            if (list == null)
            {
                ViewBag.Message = "Ooops, no item on cart, Please select items.";
                return RedirectToActionPermanent("Sales");
            }
            foreach (order_cart cart in list)
            {
                order_cart row = cart;
                var stock = db.stocks.FirstOrDefault(p => p.product_id == cart.product_id);
                this.db.sales.Add(new sale()
                {
                    id = Setup.GenerateID.GetID(),
                    product_id = row.product_id,
                    stock_id = stock.id,
                    product_name = stock.product.product_name,
                    customer_id = custid,
                    transaction_id = id,
                    qty = row.qty,
                    unitprice = row.unitprice,
                    unitcostprice = stock.product.cost_price,
                    unitsellingprice = stock.product.selling_price,
                    total_amount = row.qty * row.unitprice,
                    date_sold = DateTime.Now,
                    sold_by = User.id,
                    insert_user = UserID
                });
                this.db.order_cart.Remove(this.db.order_cart.Find(row.id));
                stock.qty = Convert.ToInt32(stock.qty - row.qty);
            }

            //get customer Account balance
            var bal = db.customer_ledger.Where(p => p.customerid == custid).OrderByDescending(p => p.insertdate).FirstOrDefault();
            decimal newbal = 0;
            if (bal != null)
            {
                newbal = Convert.ToDecimal(bal.balance - totalamount);
            }
            //debit customer Account for sales
            db.customer_ledger.Add(new customer_ledger
            {
                id = Guid.NewGuid().ToString(),
                customerid = custid,
                transactionid = id,
                receivedfrom = cust.customer_name,
                description = description,
                paymenttype = type,
                debit = totalamount,
                credit = 0,
                balance = newbal,
                insertdate = DateTime.Now.Date,
                insertuser = UserID,
                username = usr.firstname + " " + usr.lastname + " " + usr.othernames
            });
            //Credit Custimer Account for Payment
            db.customer_ledger.Add(new customer_ledger
            {
                id = Guid.NewGuid().ToString(),
                customerid = custid,
                transactionid = id,
                receivedfrom = cust.customer_name,
                description = description,
                paymenttype = type,
                debit = 0,
                credit = amountpaid,
                balance = newbal + amountpaid,
                insertdate = DateTime.Now.Date,
                insertuser = UserID,
                username = usr.firstname + " " + usr.lastname + " " + usr.othernames
            });
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToActionPermanent("Receipt/" + id, "Sales");
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
