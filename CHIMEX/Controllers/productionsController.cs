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
    public class productionsController : Controller
    {

        private chimexerpEntities db = new chimexerpEntities();

        // GET: productions
        public ActionResult Index()
        {
            var productions = db.productions.Include(p => p.stock).Include(p => p.useraccount);
            return View(productions.ToList());
        }

        // GET: productions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production production = db.productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // GET: productions/Create
        public ActionResult Create()
        {
            ViewBag.productid = new SelectList(db.stocks, "id", "id");
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid");
            return View();
        }

        // POST: productions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,decription,stockid,qty_bags,qty_items,insertdate,insertuser")] production production)
        {
            if (ModelState.IsValid)
            {
                var stock = db.stocks.Find(production.stockid);

                production.id = Setup.GenerateID.GetID();
                production.insertuser = Session["userid"].ToString();
                production.insertdate = DateTime.Now;
                production.name = stock.product.product_name;

                db.productions.Add(production);

                //post to stock
                
                stock.qty += Convert.ToInt32(production.qty_items);

                //Post to added stock
                db.stocks_added.Add(new stocks_added
                {
                    id = Setup.GenerateID.GetID(),
                    stock_id = production.stockid,
                    qty_added = production.qty_items,
                    insertdate = DateTime.Now,
                    insertuser = Session["userid"].ToString()
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productid = new SelectList(db.products, "id", "product_name", production.stockid);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", production.insertuser);
            return View(production);
        }

        // GET: productions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production production = db.productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.products, "id", "product_name", production.stockid);
            ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", production.insertuser);
            return View(production);
        }

        // POST: productions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,decription,stockid,qty_bags,qty_items,insertdate,insertuser")] production production, string id, int qty_bags, int qty_items)
        {
            //if (ModelState.IsValid)
            //{

            //post to production
            //production.id = Setup.GenerateID.GetID();
            var prod = db.productions.Find(id);

            prod.insertuser = Session["userid"].ToString();
            prod.insertdate = DateTime.Now;
            prod.qty_items = qty_items;
            prod.qty_bags = qty_bags;
            //production.name = production.stock.product.product_name;
            //db.Entry(production).State = EntityState.Modified;

            //post to stock
            //var stock = db.stocks.Find(production.stockid);
            //stock.qty += Convert.ToInt32(production.qty_items);

            //Post to added stock
            //db.stocks_added.Add(new stocks_added
            //{
            //    id = Setup.GenerateID.GetID(),
            //    stock_id = production.stockid,
            //    qty_added = production.qty_items,
            //    insertdate = DateTime.Now,
            //    insertuser = Session["userid"].ToString()
            //}); 




            db.SaveChanges();
                return RedirectToAction("Index");
            
            //ViewBag.productid = new SelectList(db.products, "id", "product_name", production.stockid);
            //ViewBag.insertuser = new SelectList(db.useraccounts, "id", "branchid", production.insertuser);
            //return View(production);
        }

        // GET: productions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production production = db.productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: productions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            production production = db.productions.Find(id);
            db.productions.Remove(production);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Post Production
        public ActionResult postProduction()
        {
            return View(db.productions.ToList());
        }
        [HttpPost]
        public ActionResult postProduction (string productionid, string stockid, int qty_bags, int qty_items)
        {
            var stocks = db.stocks.FirstOrDefault(p => p.id == stockid);
            int qty = Convert.ToInt32(qty_bags * qty_items);

            stocks.qty += Convert.ToInt32(qty);

            //get production
            // var prod = db.productions.Find(productionid);

            //Add Stock
            var stockin = new stocks_added
            {
                id = Guid.NewGuid().ToString(),
                stock_id = stockid,
                qty_added = qty,
                insertdate = DateTime.Now,
                insertuser = Session["userid"].ToString()
            };
            //add productionlog
            db.production_log.Add(new production_log
            {
                id = Guid.NewGuid().ToString(),
                productionid = productionid,
                insertdate = DateTime.Now,
                insertuser = Session["email"].ToString()
            });

            db.stocks_added.Add(stockin);
            db.SaveChanges();
            return View(db.productions.ToList());
        }
        public ActionResult ProductionReport()
        {
            return View(db.production_log.OrderByDescending(p=>p.insertdate).ToList());
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
