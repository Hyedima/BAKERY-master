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
    public class staffsController : Controller
    {
        private chimexerpEntities db = new chimexerpEntities();

        // GET: staffs
        public ActionResult Index()
        {
            return View(db.staffs.ToList());
        }

        // GET: staffs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            staff staff = db.staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: staffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,phone,address,insertdate,updatedate,status,notes")] staff staff)
        {
            if (ModelState.IsValid)
            {
                staff.id = Setup.GenerateID.GetID();
                staff.updatedate = DateTime.Now;
                staff.insertdate = DateTime.Now;
                staff.status = "TRUE";
                staff.notes = "";
                db.staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: staffs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            staff staff = db.staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,phone,address,insertdate,updatedate,status,notes")] staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: staffs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            staff staff = db.staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            staff staff = db.staffs.Find(id);
            db.staffs.Remove(staff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Payrol(string id)
        {
            return View(db.staffs.FirstOrDefault(p=>p.id == id));
        }
        public ActionResult staffSalary(string id)
        {
            return View(db.Salaries.Where(p=>p.staffid == id).ToList());
        }
        public ActionResult salary(string id, decimal amount, string description)
        {
            db.Salaries.Add(new Salary
            {
                id = Setup.GenerateID.GetID(),
                salary1 = amount,
                decription = description,
                insertdate = DateTime.Now,
                staffid = id,
                status = "Posted",

            });
            db.SaveChanges();
            return RedirectToAction("payrol/"+id, "staffs");
        }
        public ActionResult bonus(string id, decimal amount, string description)
        {
            db.bonus.Add(new bonu
            {
                id = Setup.GenerateID.GetID(),
                walfare = amount,
                decription = description,
                insertdate = DateTime.Now,
                staffid = id,
                status = "Posted"
            });
            db.SaveChanges();
            return RedirectToAction("payrol/" + id, "staffs");
        }

        public ActionResult fines(string id, decimal amount, string description)
        {
            db.deductions.Add(new deduction
            {
                id = Setup.GenerateID.GetID(),
                item = amount,
                decription = description,
                insertdate = DateTime.Now,
                staffid = id,
                status = "Posted",

            });
            db.SaveChanges();
            return RedirectToAction("payrol/" + id, "staffs");
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
