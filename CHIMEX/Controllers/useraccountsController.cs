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
    public class useraccountsController : Controller
    {
        
        private chimexerpEntities db = new chimexerpEntities();

        //[CheckAuthentication]
        // GET: useraccounts
        public ActionResult Index()
        {
            return View(db.useraccounts.ToList());
        }

        // GET: useraccounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useraccount useraccount = db.useraccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        // GET: useraccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: useraccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,branchid,firstname,lastname,othernames,phone,address,dob,email,password,regdate,usertype,isactive")] useraccount useraccount)
        {
            useraccount.id = Guid.NewGuid().ToString();
            useraccount.isactive = "true";
            useraccount.regdate = DateTime.Now;
            useraccount.usertype = "USER";
            useraccount.password = Setup.CryptoEngine.Encrypt(useraccount.password);
            db.useraccounts.Add(useraccount);
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(useraccount);
        }

        // GET: useraccounts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useraccount useraccount = db.useraccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        // POST: useraccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,branchid,firstname,lastname,othernames,phone,address,dob,email,password,regdate,usertype,isactive")] useraccount useraccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(useraccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(useraccount);
        }

        // GET: useraccounts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useraccount useraccount = db.useraccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        // POST: useraccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            useraccount useraccount = db.useraccounts.Find(id);
            db.useraccounts.Remove(useraccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Login Page
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string email = form["email"];
            string password = form["password"];
            List<string> Meassage = new List<string>();
            string clearpassword;
            Meassage.Add("Login failed. Please review your credentials and login again");

            var User = db.useraccounts.FirstOrDefault(u => u.email == email);
            if (User != null)
            {
                clearpassword = Setup.CryptoEngine.Decrypt(User.password);

                if (password == clearpassword)
                {
                    Session["userid"] = User.id.ToString();
                    Session["email"] = User.email.ToString();
                    Session["usertype"] = User.usertype;
                    Session["branchid"] = User.branchid;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = Meassage[0];
                    return View();
                }
            }
            else
            {
                ViewBag.error = Meassage[0];
                return View();
            }
        }
        //Log out User from the system
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "UserAccounts");
        }
        public ActionResult ChangeUserType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeUserType(FormCollection form)
        {
            string Userid = form["userid"];
            string Type = form["usertype"];
            string branchid = form["branchid"];
            var User = db.useraccounts.FirstOrDefault(p => p.id == Userid);
            User.usertype = Type;
            User.branchid = branchid;
            db.SaveChanges();
            ViewBag.message = "User Type Updated Successfully. User: " + User.firstname + " " + User.lastname + " User Type: " + Type;
            return View();
        }
        public ActionResult ChangePassword(string id)
        {
            //var UserAccount = db.useraccounts.FirstOrDefault(u => u.id == id);
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            string passoword = form["password"];
            string userID = form["userid"];
            var UserAccount = db.useraccounts.FirstOrDefault(u => u.id == userID);
            string EncPass = Setup.CryptoEngine.Encrypt(passoword);
            UserAccount.password = EncPass;
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        // GET: useraccounts/Edit/5 my profile
        public ActionResult Editprofile(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useraccount useraccount = db.useraccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        // POST: useraccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editprofile([Bind(Include = "id,branchid,firstname,lastname,othernames,phone,address,dob,email,password,regdate,usertype,isactive")] useraccount useraccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(useraccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(useraccount);
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
