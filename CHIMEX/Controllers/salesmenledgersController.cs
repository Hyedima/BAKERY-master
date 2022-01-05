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
    public class salesmenledgersController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: salesmenledgers
        public ActionResult Index()
        {
            var salesmenledgers = db.salesmenledgers.Include(s => s.salesman);
            return View(salesmenledgers.ToList());
        }

        // GET: salesmenledgers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salesmenledger salesmenledger = db.salesmenledgers.Find(id);
            if (salesmenledger == null)
            {
                return HttpNotFound();
            }
            return View(salesmenledger);
        }

        // GET: salesmenledgers/Create
        public ActionResult Create()
        {
            ViewBag.salesmanid = new SelectList(db.salesmen, "id", "firstname");
            return View();
        }

        // POST: salesmenledgers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,salesmanid,item,openingstock,stockreceived,additionalstock,totalstock,soldstock,stockreturned,emptiesreturned,empties_owings,store_attendant,remarks,cash_taken,date,store_keeper,Receipt_no,approved_by,cashier,week,store_supervisor,recoreded_by")] salesmenledger salesmenledger)
        {
            if (ModelState.IsValid)
            {
                db.salesmenledgers.Add(salesmenledger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.salesmanid = new SelectList(db.salesmen, "id", "firstname", salesmenledger.salesmanid);
            return View(salesmenledger);
        }

        // GET: salesmenledgers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salesmenledger salesmenledger = db.salesmenledgers.Find(id);
            if (salesmenledger == null)
            {
                return HttpNotFound();
            }
            ViewBag.salesmanid = new SelectList(db.salesmen, "id", "firstname", salesmenledger.salesmanid);
            return View(salesmenledger);
        }

        // POST: salesmenledgers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,salesmanid,item,openingstock,stockreceived,additionalstock,totalstock,soldstock,stockreturned,emptiesreturned,empties_owings,store_attendant,remarks,cash_taken,date,store_keeper,Receipt_no,approved_by,cashier,week,store_supervisor,recoreded_by")] salesmenledger salesmenledger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesmenledger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.salesmanid = new SelectList(db.salesmen, "id", "firstname", salesmenledger.salesmanid);
            return View(salesmenledger);
        }

        // GET: salesmenledgers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salesmenledger salesmenledger = db.salesmenledgers.Find(id);
            if (salesmenledger == null)
            {
                return HttpNotFound();
            }
            return View(salesmenledger);
        }

        // POST: salesmenledgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            salesmenledger salesmenledger = db.salesmenledgers.Find(id);
            db.salesmenledgers.Remove(salesmenledger);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Leger(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salesman salesman = db.salesmen.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View(salesman);
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
