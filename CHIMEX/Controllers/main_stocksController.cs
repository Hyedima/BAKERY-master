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
    public class main_stocksController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();
        
        // GET: main_stocks
        public ActionResult Index()
        {
            var main_stocks = db.main_stocks.Include(m => m.product).Include(m => m.useraccount);
            return View(main_stocks.ToList());
        }

        // GET: main_stocks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            main_stocks main_stocks = db.main_stocks.Find(id);
            if (main_stocks == null)
            {
                return HttpNotFound();
            }
            return View(main_stocks);
        }

        // GET: main_stocks/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.products, "id", "product_name");
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: main_stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,qty,update_date,insertuser")] main_stocks main_stocks)
        {
            if (ModelState.IsValid)
            {
                db.main_stocks.Add(main_stocks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.products, "id", "product_name", main_stocks.product_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", main_stocks.insertuser);
            return View(main_stocks);
        }

        // GET: main_stocks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            main_stocks main_stocks = db.main_stocks.Find(id);
            if (main_stocks == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", main_stocks.product_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", main_stocks.insertuser);
            return View(main_stocks);
        }

        // POST: main_stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,qty,update_date,insertuser")] main_stocks main_stocks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(main_stocks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.products, "id", "product_name", main_stocks.product_id);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", main_stocks.insertuser);
            return View(main_stocks);
        }

        // GET: main_stocks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            main_stocks main_stocks = db.main_stocks.Find(id);
            if (main_stocks == null)
            {
                return HttpNotFound();
            }
            return View(main_stocks);
        }

        // POST: main_stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            main_stocks main_stocks = db.main_stocks.Find(id);
            db.main_stocks.Remove(main_stocks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult newstock(string userid, string productid)
        {
            var newStock = new main_stocks
            {
                id = Guid.NewGuid().ToString(),
                product_id = productid,
                qty = 0,
                update_date = DateTime.Now,
                insertuser = Session["userid"].ToString()
            };
            db.main_stocks.Add(newStock);
            db.SaveChanges();
            var stocks = db.main_stocks.Include(s => s.product).Include(s => s.useraccount);
            return RedirectToAction("Index", "main_stocks", stocks.ToList());
        }
        public ActionResult addstock(string stockid, string qty)
        {
            var stocks = db.main_stocks.FirstOrDefault(p => p.id == stockid);
            stocks.qty += Convert.ToInt32(qty);

            var stockin = new main_stocks_Report
            {
                id = Guid.NewGuid().ToString(),
                main_stock_id = stockid,
                product_id = stocks.id,
                stock_Type = "INSTOCK",
                qty = Convert.ToInt32(qty),
                update_date = DateTime.Now,
                insertuser = Session["userid"].ToString()                
            };
            db.main_stocks_Report.Add(stockin);
            db.SaveChanges();

            var stocklist = db.main_stocks.Include(s => s.product).Include(s => s.useraccount);
            return RedirectToAction("Index", "main_stocks", stocklist.ToList());
        }

        //Supplies
        public ActionResult Supply()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Supply(string id, string userid, string qty)
        {
            var stock = db.main_stocks.FirstOrDefault(p=>p.id == id);
            if (Convert.ToInt32(qty) <= stock.qty)
            {
                db.main_stocks_Report_temp.Add(new main_stocks_Report_temp
                {
                    id = Guid.NewGuid().ToString(),
                    product_id = stock.product_id,
                    main_stock_id = id,
                    stock_Type = "OUTSTOCK",
                    qty = Convert.ToInt32(qty),
                    update_date = DateTime.Now,
                    insertuser = Session["userid"].ToString()
                });
                db.SaveChanges();
            }
            ViewBag.success = "";
            return View();
        }
        public ActionResult RemoveSupplyCart(string id)
        {
            var item = db.main_stocks_Report_temp.FirstOrDefault(p => p.id == id);
            //string transactionid = item.transaction_id;
            db.main_stocks_Report_temp.Remove(item);
            db.SaveChanges();
            return RedirectToActionPermanent("Supply");
        }
        public ActionResult SupplyReceipt(string ID)
        {
            //var trans = db.transactions.FirstOrDefault(p => p.id == ID);
            ViewBag.id = ID;
            return View();
        }
        [HttpPost]
        public ActionResult PostSupply(string id, string notes)
        {
            var sups = db.main_stocks_Report_temp.Where(p => p.insertuser == id);
            string batch_id = Guid.NewGuid().ToString();
            foreach (var item in sups)
            {
                db.main_stocks_Report.Add(new main_stocks_Report
                {
                    id = Guid.NewGuid().ToString(),
                    product_id = item.product_id,
                    main_stock_id = item.main_stock_id,
                    stock_Type = "OUTSTOCK",
                    qty = Convert.ToInt32(item.qty),
                    update_date = DateTime.Now,
                    insertuser = Session["userid"].ToString(),
                    notes = notes,
                    batch_id = batch_id
                });
                //reduces the quantity
                db.main_stocks.Find(item.main_stock_id).qty -= item.qty;
                //Remove the item from cart
                db.main_stocks_Report_temp.Remove(db.main_stocks_Report_temp.Find(item.id));
            }
            db.SaveChanges();
            return RedirectToActionPermanent("SupplyReceipt/" + batch_id, "main_stocks");
        }

        public ActionResult StockoutReport()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var sal = db.main_stocks_Report.Where(p => p.update_date >= sdate && p.update_date < edate && p.stock_Type != "STOCKOUT");
            return View(sal.ToList());
        }
        [HttpPost]
        public ActionResult StockoutReport(string startdate, string enddate, string userid)
        {
            DateTime sdate = Convert.ToDateTime(startdate).Date;
            DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);

            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            if (userid == "ALL")
            {
                var sal = db.main_stocks_Report.Where(p => p.update_date >= sdate && p.update_date < edate && p.stock_Type != "STOCKOUT");
                return View(sal.ToList());
            }
            else
            {
                var sal = db.main_stocks_Report.Where(p => p.update_date >= sdate && p.update_date < edate && p.insertuser == userid && p.stock_Type != "STOCKOUT");
                return View(sal.ToList());
            }

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
