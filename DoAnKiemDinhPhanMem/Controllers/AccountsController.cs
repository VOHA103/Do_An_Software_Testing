using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnKiemDinhPhanMem.Models;

namespace DoAnKiemDinhPhanMem.Controllers
{
    public class AccountsController : Controller
    {
        private cuasneakerEntities db = new cuasneakerEntities();

        // GET: Accounts
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Accounts.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Accounts accounts)
        {
            var obj = db.Accounts.Where(a => a.Email.Equals(accounts.Email)).FirstOrDefault();
            accounts.Password = Models.User.EncodeMD5(accounts.Password);
            //accounts.Password = accounts.Password;
            //accounts.User.EncodeSHA1(accounts.Password);
            if (ModelState.IsValid && obj == null)
            {
                db.Accounts.Add(accounts);
                db.SaveChanges();
               // HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Accounts", accounts).Result;
                return RedirectToAction("Login1");
            }
            Session["Messege"] = "Email already exist";
            return View();
        }
        [HttpPost]
        public JsonResult XuLyDangKi(Accounts accounts)
        {

            if (db.Accounts.SingleOrDefault(a => a.Email.Equals(accounts.Email)) != null)
                return Json(new
                {
                    status = false,
                    message = "Trung email"
                });
            if (db.Accounts.SingleOrDefault(a => a.Username.Equals(accounts.Username)) != null)
                return Json(new
                {
                    status = false,
                    message = "Trung tai khoan"
                });
            accounts.Status = false;
            accounts.IsAdmin = false;
            accounts.Password = Models.User.EncodeMD5(accounts.Password);
            db.Accounts.Add(accounts);
            db.SaveChanges();
            return Json(new
            {
                status = true,
                url = "Login"
            });
        }
        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Accounts.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accounts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accounts);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Accounts.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Accounts accounts = db.Accounts.Find(id);
            db.Accounts.Remove(accounts);
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
        public ActionResult Login()
        {
            if (Session["Email"] != null)
            {
                Response.Redirect("~/Home/Index", false);
            }
            return View();
        }
        [HttpPost]
        public JsonResult XuLyDangNhap(string taiKhoan, string matKhau)
        {
            matKhau = DoAnKiemDinhPhanMem.Models.User.EncodeMD5(matKhau.Trim());
            Accounts obj = db.Accounts.Where(x => x.Username.Equals(taiKhoan.Trim()) && x.Password.Equals(matKhau)).FirstOrDefault();
            if (obj != null)
            {
                if (!obj.Status)
                {
                    Session["Email"] = obj.Email.ToString();
                    Session["id"] = obj.Id.ToString();
                    Session["IsAdmin"] = obj.IsAdmin;
                    Session["Username"] = obj.Username;
                    Session["Fullname"] = obj.Fullname;
                    if (!obj.IsAdmin)
                    {

                        return Json(new
                        {
                            status = true,
                            url = "/Home/Index"
                        });
                    }


                    return Json(new
                    {
                        status = true,
                        url = "/Home/Index"
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Tai khoan bi khoa."
                });

            }
            return Json(new
            {
                status = false,
                message = "Tai khoan hoac mat khau khong chinh xac."
            });

        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Home/Index");
            return View();
        }
        //
    }
}
