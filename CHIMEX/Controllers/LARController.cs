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
    public class LARController : Controller
    {
        chimexerpEntities db = new chimexerpEntities();
        // GET: LAR
        public ActionResult Index()
        {
            var lars = db.customers.Where(p => p.customertype == "LAR").ToList();
            return View(lars);
        }
        public ActionResult LarStock(string id)
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
        public ActionResult LarReqisition(string id)
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
        [HttpPost]
        public ActionResult newlarstock(string customerid, string userid, string productid)
        {
            db.Larstocks.Add(new Larstock
            {
                id = Guid.NewGuid().ToString(),
                customerid = customerid,
                product_id = productid,
                qty = 0,
                update_date = DateTime.Now,
                insertuser = userid
            });
            db.SaveChanges();
            return RedirectToActionPermanent("larstock/" + customerid);
        }
        public ActionResult addlarstock(string customerid, string stockid, string userid, string productid, string qty)
        {
            Larstock stk = db.Larstocks.Find(stockid);
            stk.qty += Convert.ToInt32(qty);

            //keeps a log of added stock
            db.larstock_added.Add(new larstock_added
            {
                id = Guid.NewGuid().ToString(),
                customerid = customerid,
                product_id = productid,
                qty = Convert.ToInt32(qty),
                update_date = DateTime.Now,
                insertuser = userid
            });
            db.SaveChanges();

            return RedirectToActionPermanent("larstock/" + customerid);
        }
        public ActionResult LarStockReport(string id)
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
        public ActionResult LarStockReport(string startdate, string enddate, string id)
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
    }
}