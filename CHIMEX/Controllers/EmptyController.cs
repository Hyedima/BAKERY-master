using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHIMEX.Models;
using CHIMEX.Setup;

namespace CHIMEX.Controllers
{
    [CheckAuthentication]
    public class EmptyController : Controller
    {
        // GET: Empty
        chimexerpEntities db = new chimexerpEntities();
        // GET: LAR
        public ActionResult Index()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var cust = db.customers.ToList();
            return View(cust);
        }
        [HttpPost]
        public ActionResult Index(string startdate, string enddate, string customerid)
        {
            if (customerid == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var cust = db.customers.ToList();
                return View(cust);
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var cust = db.customers.Where(p => p.id == customerid).ToList();
                return View(cust);
            }
        }
        public ActionResult ManageEmpty()
        {
            DateTime sdate = DateTime.Now.Date;
            DateTime edate = DateTime.Now.Date.AddDays(1);
            Session["startdate"] = sdate;
            Session["enddate"] = edate;
            ViewBag.sdate = sdate;
            ViewBag.edate = edate;
            var cust = db.customers.ToList();
            return View(cust);
        }
        [HttpPost]
        public ActionResult ManageEmpty(string startdate, string enddate, string customerid)
        {
            if (customerid == "ALL")
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var cust = db.customers.ToList();
                return View(cust);
            }
            else
            {
                DateTime sdate = Convert.ToDateTime(startdate).Date;
                DateTime edate = Convert.ToDateTime(enddate).Date.AddDays(1);
                Session["startdate"] = sdate;
                Session["enddate"] = edate;
                ViewBag.sdate = sdate;
                ViewBag.edate = edate;
                var cust = db.customers.Where(p => p.id == customerid).ToList();
                return View(cust);
            }

        }
       
        [HttpPost]
        public ActionResult Owing(string itemid, string customerid, string qty)
        {
            db.EspectedEmpties.Add(new EspectedEmpty
            {
                id = Guid.NewGuid().ToString(),
                customerid = customerid,
                itemid = itemid,
                qty = Convert.ToInt32(qty),
                amount = 0,
                notes = "",
                insertdate = DateTime.Now,
                insertuser = Session["userid"].ToString()
            });
            db.SaveChanges();
            return RedirectToActionPermanent("ManageEmpty", db.customers.ToList());
        }

        public ActionResult EmptiesStock()
        {
            return View();
        }
       
    }
}