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
    public class salesController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: sales
        public ActionResult Index()
        {
            var sales = db.sales.Include(s => s.customer).Include(s => s.product).Include(s => s.stock).Include(s => s.transaction).Include(s => s.useraccount);
            return View(sales.ToList());
        }

        // GET: sales/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: sales/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name");
            ViewBag.product_id = new SelectList(db.products, "id", "product_name");
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id");
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "name");
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,stock_id,product_name,customer_id,transaction_id,qty,unitprice,total_amount,date_sold,sold_by,insert_user")] sale sale)
        {
            if (ModelState.IsValid)
            {
                db.sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", sale.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", sale.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", sale.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "name", sale.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", sale.insert_user);
            return View(sale);
        }

        // GET: sales/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", sale.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", sale.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", sale.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "name", sale.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", sale.insert_user);
            return View(sale);
        }

        // POST: sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,stock_id,product_name,customer_id,transaction_id,qty,unitprice,total_amount,date_sold,sold_by,insert_user")] sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "customer_name", sale.customer_id);
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", sale.product_id);
            ViewBag.stock_id = new SelectList(db.stocks, "id", "product_id", sale.stock_id);
            ViewBag.transaction_id = new SelectList(db.transactions, "id", "name", sale.transaction_id);
            ViewBag.insert_user = new SelectList(db.useraccounts, "id", "branchid", sale.insert_user);
            return View(sale);
        }

        // GET: sales/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            sale sale = db.sales.Find(id);
            db.sales.Remove(sale);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Sales()
        {
            return View();
        }
        //load customer to sales dashboard
        public ActionResult loadcustomer(string customerid)
        {
            Session["customerid"] = customerid;
            return RedirectPermanent("Sales");
        }
        //create new Customer
        public ActionResult NewCustomer( string name, string phone, string email, string address)
        {
            var newCustomer = new customer
            {
                id = Guid.NewGuid().ToString(),
                customer_name = name,
                phone = phone,
                email = email,
                address = address,
                insert_date = DateTime.Now
            };
            db.customers.Add(newCustomer);
            db.SaveChanges();

            return RedirectToActionPermanent("Sales");
        }
        public ActionResult Addcart(string stockid, string productid, string customerid, string qty, string unitprice, string userid)
        {
            var stck = db.stocks.FirstOrDefault(p=>p.id == stockid);
            var cart = db.carts.FirstOrDefault(p => p.stock_id == stockid && p.insert_user == userid);
            if (stck.qty < Convert.ToInt32(qty))
            {
                ViewBag.msg = "Please quantity must not be greater than the quantity at hand.";
            }
            else
            {

                if (cart != null)
                {
                    cart.unitprice = Convert.ToDecimal(unitprice);
                    cart.qty += Convert.ToInt32(qty);
                    db.SaveChanges();
                }
                else
                {
                    var newcart = new cart
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
                        insert_user = Session["userid"].ToString()
                    };
                    db.carts.Add(newcart);
                    db.SaveChanges();
                }
            }

            return RedirectToActionPermanent("Sales");
        }
      
        public ActionResult DeleteCart(string id)
        {
            var crt = db.carts.FirstOrDefault(p => p.id == id);
            db.carts.Remove(crt);
            db.SaveChanges();
            return RedirectToActionPermanent("Sales");
        }
       
        [HttpPost]
        public ActionResult SaleSaveTransaction(FormCollection form)
        {
            string id = Setup.GenerateID.GetID();
            string UserID = form["userid"];
            var User = db.useraccounts.FirstOrDefault(p => p.id == UserID);
            string custid = form["customer"];
            string type = form["paymenttype"];
            string transactiontype = form["transactiontype"];
            string description = form["description"];
            decimal totalamount = Convert.ToDecimal(form["totalamount"]);
            decimal amountpaid = Convert.ToDecimal(form["amountpaid"]);
            string status = "Paid";
            if(transactiontype == "CREDIT")
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

            List<cart> list = this.db.carts.Where<cart>(c => c.insert_user == UserID).ToList<cart>();
            if (list == null)
            {
                ViewBag.Message = "Ooops, no item on cart, Please select items.";
                return RedirectToActionPermanent("Sales");
            }
            foreach (cart cart in list)
            {
                cart row = cart;
                var stock = db.stocks.FirstOrDefault(p => p.product_id == cart.product_id);
                this.db.sales.Add(new sale()
                {
                    id = Setup.GenerateID.GetID(),
                    product_id = row.product_id,
                    stock_id = stock.id,
                    product_name = stock.product.product_name,
                    customer_id  = custid,
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
                this.db.carts.Remove(this.db.carts.Find(row.id));
                stock.qty = Convert.ToInt32(stock.qty - row.qty);
            }

            //get customer Account balance
            var bal = db.customer_ledger.Where(p => p.customerid == custid).OrderByDescending(p => p.insertdate).FirstOrDefault();
            decimal newbal = 0;
            if(bal != null)
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
        public ActionResult Receipt(string ID)
        {
            var trans = db.transactions.FirstOrDefault(p => p.id == ID);
            return View(trans);
        }

        public ActionResult PreviewTranx(string ID)
        {
            var trans = db.transactions.FirstOrDefault(p => p.id == ID);
            return View(trans);
        }
        public ActionResult Salespayment(string ID)
        {
            var trans = db.transactions.FirstOrDefault(p => p.id == ID);
            return View(trans);
        }
        [HttpPost]
        public ActionResult Salespayment(string transactionid, string customerid, string amount, string paymenttype, string notes)
        {
            /*db.salespayments.Add(new salespayment
            {
                id = Guid.NewGuid().ToString(),
                transactionid = transactionid,
                customerid = customerid,
                reason = notes,
                amount = Convert.ToDecimal(amount),
                paymenttype = paymenttype,
                insertdate = DateTime.Now,
                insertuser = Session["userid"].ToString()
            });
            db.SaveChanges();
            */
            var trans = db.transactions.FirstOrDefault(p => p.id == transactionid);
            return RedirectToActionPermanent("Receipt/"+transactionid, "Sales");
        }
        //Supplies
        public ActionResult Supply(string ID)
        {
            var trans = db.transactions.FirstOrDefault(p => p.id == ID);
            return View(trans);
        }
        [HttpPost]
        public ActionResult Supply(string id, string transactionid, string qty)
        {
            var sal = db.sales.FirstOrDefault(p => p.id == id && p.transaction_id == transactionid);
            //var sup_tem = db.supplies_temp.FirstOrDefaultAsync(p=>p.transaction_id )
            if(Convert.ToInt32(qty) <= sal.qty)
            {
                db.supplies_temp.Add(new supplies_temp
                {
                    id = Guid.NewGuid().ToString(),
                    product_id = sal.product_id,
                    stock_id = sal.stock_id,
                    product_name = sal.product_name,
                    customer_id = sal.customer_id,
                    transaction_id = sal.transaction_id,
                    qty = Convert.ToInt32(qty),
                    unitprice = Convert.ToDecimal(sal.unitprice),
                    total_amount = Convert.ToDecimal(Convert.ToInt32(qty) * Convert.ToDecimal(sal.unitprice)),
                    date_sold = sal.date_sold,
                    sold_by = sal.sold_by,
                    insert_user = Session["userid"].ToString()
                });
                db.SaveChanges();
            }
           
            return RedirectToActionPermanent("Supply/"+transactionid, "Sales");
        }
        public ActionResult RemoveSupplyCart(string id)
        {
            var item = db.supplies_temp.FirstOrDefault(p => p.id == id);
            string transactionid = item.transaction_id;
            db.supplies_temp.Remove(item);
            db.SaveChanges();
            return RedirectToActionPermanent("Supply/" + transactionid, "Sales");
        }
        public ActionResult SupplyReceipt(string ID)
        {
            var trans = db.transactions.FirstOrDefault(p => p.id == ID);
            return View(trans);
        }
        public ActionResult PostSupply(string id)
        {
            var sups = db.supplies_temp.Where(p => p.transaction_id == id);
            foreach(var item in sups)
            {
                db.supplies.Add(new supply
                {
                    id = Guid.NewGuid().ToString(),
                    product_id = item.product_id,
                    stock_id = item.stock_id,
                    product_name = item.product_name,
                    customer_id = item.customer_id,
                    transaction_id = item.transaction_id,
                    qty = item.qty,
                    unitprice = item.unitprice,
                    total_amount = item.total_amount,
                    date_sold = DateTime.Now,
                    sold_by = item.sold_by,
                    insert_user = Session["userid"].ToString()
                });
            }
            db.SaveChanges();

            return RedirectToActionPermanent("SupplyReceipt/" + id, "Sales");
        }

        public ActionResult Customers()
        {
            return View(db.customers.ToList());
        }
        public ActionResult Customertrnx()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate);
            return View(trans.ToList());
        }
        [HttpPost]
        public ActionResult Customertrnx(string startdate, string enddate,string customerid)
        {
            if (customerid == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate && p.status != "Canceled");
                return View(trans.ToList());
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate && p.customerid == customerid && p.status != "Canceled");
                return View(trans.ToList());
            }
        }
        public ActionResult ViewCustomertrnx()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate);
            return View(trans.ToList());
        }
        [HttpPost]
        public ActionResult ViewCustomertrnx(string startdate, string enddate, string customerid)
        {
            if (customerid == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate && p.status != "Canceled");
                return View(trans.ToList());
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate && p.customerid == customerid && p.status != "Canceled");
                return View(trans.ToList());
            }

        }
        public ActionResult SalesReport()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.transaction.status != "Canceled");
            return View(sal.ToList());
        }
        [HttpPost]
        public ActionResult SalesReport(string startdate, string enddate, string userid)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
            
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            if (userid == "ALL")
            {
                var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.transaction.status != "Canceled");
                return View(sal.ToList());
            }
            else
            {
                var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.insert_user ==  userid && p.transaction.status != "Canceled");
                return View(sal.ToList());
            }

        }
        public ActionResult UserSalesReport()
        {
            var userid = Session["userid"].ToString();
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.insert_user == userid && p.transaction.status != "Canceled");
            return View(sal.ToList());
        }
        [HttpPost]
        public ActionResult UserSalesReport(string startdate, string enddate, string userid)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            if (userid == "ALL")
            {
                var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.insert_user == userid && p.transaction.status != "Canceled");
                return View(sal.ToList());
            }
            else
            {
                var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.insert_user == userid && p.transaction.status != "Canceled");
                return View(sal.ToList());
            }

        }
        public ActionResult SalesAdminReport()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.transaction.status != "Canceled");
            return View(sal.ToList());
        }
        [HttpPost]
        public ActionResult SalesAdminReport(string startdate, string enddate, string userid)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            if (userid == "ALL")
            {
                var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.transaction.status != "Canceled");
                return View(sal.ToList());
            }
            else
            {
                var sal = db.sales.Where(p => p.date_sold >= sdate && p.date_sold < edate && p.insert_user == userid && p.transaction.status != "Canceled");
                return View(sal.ToList());
            }

        }

        public ActionResult CancelTransction(string id)
        {
            return View(db.transactions.Find(id));
        }
        public ActionResult cancelSales(string ID)
        {
            var trans = db.transactions.FirstOrDefault(p => p.id == ID);
            return View(trans);
        }
        [HttpPost]
        public ActionResult cancelSales(string id, string qty)
        {
            string userid = Session["userid"].ToString();
            var item = db.sales.FirstOrDefault(p => p.id == id);
            var user_ac = db.useraccounts.FirstOrDefault(p => p.id == userid);
            int qt = Convert.ToInt32(qty);
            item.qty = item.qty - qt;
            item.total_amount = item.qty * item.unitsellingprice;
            string tranID = item.transaction_id;
            //if(qt <= item.qty)
            //{
                db.sales_canceled.Add(new sales_canceled
                {
                    id = Guid.NewGuid().ToString(),
                    product_id = item.product_id,
                    stock_id = item.stock_id,
                    product_name = item.product_name,
                    customer_id = item.customer_id,
                    transaction_id = item.transaction_id,
                    qty = qt,
                    unitprice = item.unitprice,
                    unitcostprice = item.unitcostprice,
                    unitsellingprice = item.unitsellingprice,
                    total_amount = item.unitsellingprice * qt,
                    date_sold = item.date_sold,
                    sold_by = item.sold_by,
                    insert_user = user_ac.id,
                    canceled_by = userid,
                    canceled_on = DateTime.Now
                });
                //credit customers Account
                var bal = db.customer_ledger.Where(p => p.customerid == item.customer_id).OrderByDescending(p => p.insertdate).FirstOrDefault();
                decimal balance = Convert.ToDecimal(bal.balance);
                db.customer_ledger.Add(new customer_ledger
                {
                    id = Guid.NewGuid().ToString(),
                    transactionid = item.transaction_id,
                    customerid = item.customer_id,
                    receivedfrom = item.customer.customer_name,
                    description = "Cancelled transaction (Sales)",
                    paymenttype = "CASH",
                    credit = item.unitsellingprice * qt,
                    debit = 0,
                    balance = balance-(item.unitsellingprice * qt),
                    insertdate = DateTime.Now,
                    insertuser = userid,
                    username = user_ac.firstname + " " + user_ac.lastname + " " + user_ac.othernames
                });
                db.sales.Remove(item);
                db.SaveChanges();
           // }
            return RedirectToAction("cancelSales/" + tranID, "Sales");
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
