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
        public ActionResult Edit([Bind(Include = "id,name,decription,stockid,qty_bags,qty_items,insertdate,insertuser")] production production)
        {
            if (ModelState.IsValid)
            {

                //post to production
                production.id = Setup.GenerateID.GetID();
                production.insertuser = Session["userid"].ToString();
                production.insertdate = DateTime.Now;
                production.name = production.stock.product.product_name;
                db.Entry(production).State = EntityState.Modified;

                //post to stock
                var stock = db.stocks.Find(production.stockid);
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
