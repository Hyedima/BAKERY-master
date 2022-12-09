using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Testing.Models;
using Testing.Setup;

namespace Testing.Controllers
{
    public class useraccountsController : Controller
    {
        private BakeryEntities db = new BakeryEntities();

        // GET: useraccounts
        public ActionResult Index()
        {
            var useraccounts = db.useraccounts.Include(u => u.Branch);
            return View(useraccounts.ToList());
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
            ViewBag.branchid = new SelectList(db.Branches, "id", "name");
            return View();
        }

        // POST: useraccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,branchid,firstname,lastname,othernames,phone,address,dob,email,password,regdate,usertype,isactive")] useraccount useraccount)
        {
            if (ModelState.IsValid)
            {
                db.useraccounts.Add(useraccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.branchid = new SelectList(db.Branches, "id", "name", useraccount.branchid);
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
            ViewBag.branchid = new SelectList(db.Branches, "id", "name", useraccount.branchid);
            return View(useraccount);
        }

        // POST: useraccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewBag.branchid = new SelectList(db.Branches, "id", "name", useraccount.branchid);
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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password, string ReturnUrl)
        {
            try
            {
                var User = db.useraccounts.FirstOrDefault(u => u.email == email);
                if (User != null)
                {
                    string clearpassword = Testing.Setup.CryptoEngine.Decrypt(User.password);
                    if (password == clearpassword)
                    {
                        FormsAuthentication.SetAuthCookie(email, true);
                        HttpCookie loginCookie = new HttpCookie("login");
                        loginCookie.Values["userid"] = CryptoEngine.Encrypt(User.id);
                        loginCookie.Values["email"] = CryptoEngine.Encrypt(User.email);
                        loginCookie.Values["usertype"] = CryptoEngine.Encrypt(User.usertype);
                        loginCookie.Path = Request.ApplicationPath;
                        //Set the Expiry date.
                        loginCookie.Expires = DateTime.Now.AddDays(7);
                        //Add the Cookie to Browser.
                        Response.Cookies.Add(loginCookie);

                        Session["userid"] = User.id.ToString();
                        Session["email"] = User.email.ToString();
                        Session["usertype"] = User.usertype;
                        //Session["branchid"] = User.branchid;

                        TempData["success"] = "true";
                        TempData["message"] = "Logged in Successfully.";
                        //
                        //if(ReturnUrl == null)
                        //{
                        //    return RedirectToAction("Index", "Home");
                        //}
                        //else
                        //{
                        //    return Redirect(ReturnUrl);
                        //}
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["success"] = "false";
                        TempData["message"] = "Login failed, please review the fields and try again."; // err.Message
                        return View();
                    }
                }
                else
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Login failed, please review the fields and try again."; // err.Message
                    return View();
                }
            }
            catch (Exception err)
            {
                TempData["success"] = "false";
                TempData["message"] = "Login failed, please review the fields and try again." + err;
                return View();
            }
        }
        public ActionResult Logout()
        {
            HttpCookie loginCookie = new HttpCookie("login");
            loginCookie.Values["userid"] = null;
            loginCookie.Values["email"] = null;
            loginCookie.Values["usertype"] = null;
            loginCookie.Path = Request.ApplicationPath;
            //Set the Expiry date.
            loginCookie.Expires = DateTime.Now.AddDays(-1);
            //Add the Cookie to Browser.
            Response.Cookies.Add(loginCookie);

            Session.Abandon();
            return RedirectToAction("Login");
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
