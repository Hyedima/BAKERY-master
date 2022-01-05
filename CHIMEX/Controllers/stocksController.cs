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
    public class stocksController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: stocks
        public ActionResult Index()
        {
            var stocks = db.stocks.Include(s => s.product).Include(s => s.useraccount);
            return View(stocks.ToList());
        }
        public ActionResult ManageStock()
        {
            var stocks = db.stocks.Include(s => s.product).Include(s => s.useraccount);
            return View(stocks.ToList());
        }

        public ActionResult FullsOwing()
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
        public ActionResult FullsOwing(string startdate, string enddate, string customerid)
        {
            if(customerid == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate);
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
                var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate && p.customerid == customerid);
                return View(trans.ToList());
            }
            
        }
        public ActionResult Supply()
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
        public ActionResult Supply(string startdate, string enddate)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var trans = db.transactions.Where(p => p.trnx_date >= sdate && p.trnx_date < edate);
            return View(trans.ToList());
        }
        // GET: stocks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: stocks/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.products, "id", "product_name");
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,qty,update_date,insertuser")] stock stock)
        {
            stock.id = Guid.NewGuid().ToString();
            stock.insertuser = Session["userid"].ToString();
            stock.update_date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.products, "id", "product_name", stock.product_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stock.insertuser);
            return View(stock);
        }

        // GET: stocks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", stock.product_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stock.insertuser);
            return View(stock);
        }

        // POST: stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,qty,update_date,insertuser")] stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", stock.product_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", stock.insertuser);
            return View(stock);
        }

        // GET: stocks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            stock stock = db.stocks.Find(id);
            db.stocks.Remove(stock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult newstock(string userid, string productid)
        {
            var newStock = new stock
            {
                id = Guid.NewGuid().ToString(),
                product_id = productid,
                qty = 0,
                update_date = DateTime.Now,
                insertuser = Session["userid"].ToString()
            };
            db.stocks.Add(newStock);
            db.SaveChanges();
            var stocks = db.stocks.Include(s => s.product).Include(s => s.useraccount);
            return RedirectToAction("Index","Stocks", stocks.ToList());
        }
        public ActionResult addstock(string stockid, string qty)
        {
            var stocks = db.stocks.FirstOrDefault(p=>p.id == stockid);
            stocks.qty += Convert.ToInt32(qty);

            var stockin = new stocks_added
            {
                id = Guid.NewGuid().ToString(),
                stock_id = stockid,
                qty_added = Convert.ToInt32(qty),
                insertdate = DateTime.Now,
                insertuser = Session["userid"].ToString()
            };
            db.stocks_added.Add(stockin);
            db.SaveChanges();

            var stocklist = db.stocks.Include(s => s.product).Include(s => s.useraccount);
            return RedirectToAction("Index", "Stocks", stocklist.ToList());
        }
        public ActionResult OpenStock()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            var open = db.stocks_opening.FirstOrDefault(p => p.update_date >= sdate && p.update_date < edate);
            if(open == null)
            {
                foreach (var item in db.stocks.ToList())
                {
                    db.stocks_opening.Add(new stocks_opening
                    {
                        id = Guid.NewGuid().ToString(),
                        product_id = item.product_id,
                        qty = item.qty,
                        update_date = DateTime.Now,
                        insertuser = Session["userid"].ToString(),
                        stockid = item.id
                    });
                    db.stocks_closing.Add(new stocks_closing
                    {
                        id = Guid.NewGuid().ToString(),
                        product_id = item.product_id,
                        qty = item.qty,
                        update_date = DateTime.Now.AddDays(-1),
                        insertuser = Session["userid"].ToString(),
                        stockid = item.id
                    });
                }
                db.SaveChanges();
            }
            //get todays opening stock
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var opening = db.stocks_opening.Where(p => p.update_date >= sdate && p.update_date < edate);

            return RedirectToActionPermanent("OpeningStock","Stocks", opening);
        }
        public ActionResult OpeningStock()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var opening = db.stocks_opening.Where(p => p.update_date >= sdate && p.update_date < edate);

            return View(opening);
        }
        [HttpPost]
        public ActionResult OpeningStock(string startdate, string enddate)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var opening = db.stocks_opening.Where(p => p.update_date >= sdate && p.update_date < edate);

            return View(opening);
        }
        public ActionResult ClosingStock()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var Closing = db.stocks_closing.Where(p => p.update_date >= sdate && p.update_date < edate);

            return View(Closing);
        }
        [HttpPost]
        public ActionResult ClosingStock(string startdate, string enddate)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var Closing = db.stocks_closing.Where(p => p.update_date >= sdate && p.update_date < edate);

            return View(Closing);
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
